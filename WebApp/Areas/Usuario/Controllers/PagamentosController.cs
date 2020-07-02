using DataAccess.Entidades;
using DataAccess.Helpers;
using DataAccess.Interfaces;
using DataAccess.Serialized_Objects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;
using WebApp.ActionFilters;
using WebApp.Helpers;

namespace WebApp.Areas.Usuario.Controllers
{
    public class PagamentosController : MyBaseController
    {
        private readonly IUsuarioRepository _UserRepo;
        private readonly IPagamentoLicencaRepository _PagamentoRepo;
        private readonly IServerConfigRepository _ServerConfigRepo;
        //quando subir pra produçao setar as variaveis abaixo para false
        private readonly bool PagseguroisSandbox = false;
        private readonly bool bitpayisSandbox = true;


        public PagamentosController(IUsuarioRepository UserRepository, IPagamentoLicencaRepository _PagamentosRepository, IServerConfigRepository _ServerConfigRepository)
        {
            _UserRepo = UserRepository;
            _PagamentoRepo = _PagamentosRepository;
            _ServerConfigRepo = _ServerConfigRepository;
        }

        [UsuarioAuthorize]
        [LoadViewBags]
        public ActionResult Index()
        {            
            ViewBag.price = _ServerConfigRepo.GetLicencePrice();
            return View();
        }

        [HttpPost]
        [UsuarioAuthorize]
        public async Task<MyJsonResult> PagSeguroPayment()
        {
            var user = _UserRepo.GetByEmail(User.Identity.Name);
            var config = _ServerConfigRepo.GetAllConfig();
            var tokenPagament = Guid.NewGuid().ToString();
            var PagamentoLicenca = new PagamentoLicenca()
            {
                Usuario_Id = user.Id,
                DataCriacaoInvoice = DateTime.UtcNow,
                MetodoPagamentoId = 1,
                PagamentoLicencaStatusId = 1,
                ValoraReceber = config.PrecoLicenca,
                TokenPagamento = tokenPagament
            };
            _PagamentoRepo.Add(PagamentoLicenca);

            var apiPagSeguro = new ApiPagSeguro(config.PagseguroToken, config.AppServer, PagseguroisSandbox);
            var response = apiPagSeguro.GeneratePayment(PagamentoLicenca.Id, config.PrecoLicenca);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var xmlSerializer = new XmlSerializer(typeof(checkout));
                using (var textReader = new StringReader(result))
                {
                    var objret = (checkout)xmlSerializer.Deserialize(textReader);
                    return new MyJsonResult(apiPagSeguro.pagseguroUrl + objret.code, JsonRequestBehavior.AllowGet);
                }
            }
            Response.TrySkipIisCustomErrors = true;
            Response.StatusCode = 400;
            return new MyJsonResult("Erro no metodo de Pagamento", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        public HttpResponse pagseguroSuccesso(string notificationCode)
        {
            var config = _ServerConfigRepo.GetAllConfig();
            var apiPagSeguro = new ApiPagSeguro(config.PagseguroToken, config.AppServer, PagseguroisSandbox);
            Response.AppendHeader("Access-Control-Allow-Origin", "https://sandbox.pagseguro.uol.com.br");
            var res = apiPagSeguro.CheckPayment(notificationCode);
            if (res.IsSuccessStatusCode)
            {
                var obj = res.Content.ReadAsStringAsync().Result;
                var xmlSerializer = new XmlSerializer(typeof(transaction));
                using (var textReader = new StringReader(obj))
                {
                    var objret = (transaction)xmlSerializer.Deserialize(textReader);
                    var pagamentoLicenca = _PagamentoRepo.GetById(objret.reference);
                    if (objret.status == 3)//Pago
                    {
                        pagamentoLicenca.DataPagamento = DateTime.UtcNow;
                        pagamentoLicenca.CodigoPagSeguro = objret.code;
                        pagamentoLicenca.PagamentoLicencaStatusId = 3;
                        pagamentoLicenca.ValorPago = objret.netAmount;
                        _PagamentoRepo.Update(pagamentoLicenca);
                        var user = _UserRepo.GetById(pagamentoLicenca.Usuario_Id);
                        if (user.DataVencimentoLicenca < DateTime.UtcNow)
                        {
                            user.DataVencimentoLicenca = DateTime.UtcNow.AddMonths(1);
                        }
                        else
                        {
                            user.DataVencimentoLicenca = user.DataVencimentoLicenca.AddMonths(1);
                        }
                        _UserRepo.Update(user);
                    }
                }
            }

            var response = new HttpResponse(TextWriter.Null);
            response.ContentType = "application/json";
            response.StatusCode = 201;
            return response;
        }

        [HttpPost]
        [UsuarioAuthorize]
        public MyJsonResult BitpayPayment()
        {
            var user = _UserRepo.GetByEmail(User.Identity.Name);
            var config = _ServerConfigRepo.GetAllConfig();
            var tokenPagament = Guid.NewGuid().ToString();
            var PagamentoLicenca = new PagamentoLicenca()
            {
                Usuario_Id = user.Id,
                DataCriacaoInvoice = DateTime.UtcNow,
                MetodoPagamentoId = 2,
                PagamentoLicencaStatusId = 1,
                ValoraReceber = config.PrecoLicenca,
                TokenPagamento = tokenPagament
            };
            _PagamentoRepo.Add(PagamentoLicenca);

            ApiBitPay bitpay = new ApiBitPay(config.BitpayToken, config.BitpayIdentity, bitpayisSandbox);
            var pagamento = bitpay.GeneratePayment(user, PagamentoLicenca.Id, config.PrecoLicenca, tokenPagament);
            if (pagamento.IsSuccessStatusCode)
            {
                var content = pagamento.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<BitpaiInvoiceResponse>(content);
                return new MyJsonResult(obj.data.Url, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Response.TrySkipIisCustomErrors = true;
                Response.StatusCode = 400;
                Response.StatusDescription = "Erro no metodo de Pagamento";
                return new MyJsonResult("Erro no metodo de Pagamento", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public HttpResponse BitpaySuccesso(BitpayPaymentResponse res)
        {
            try
            {
                var pagamento = _PagamentoRepo.GetById(res.orderId);
                if (res.posData == pagamento.TokenPagamento)
                {
                    pagamento.CodigoBitPay = res.id;
                    pagamento.PagamentoLicencaStatusId = 3;
                    pagamento.DataPagamento = DateTime.UtcNow;
                    pagamento.Qtd_BTC_Pago = res.amountPaid;
                    _PagamentoRepo.Update(pagamento);

                    var user = _UserRepo.GetById(pagamento.Usuario_Id);
                    if (user.DataVencimentoLicenca < DateTime.UtcNow)
                    {
                        user.DataVencimentoLicenca = DateTime.UtcNow.AddMonths(1);
                    }
                    else
                    {
                        user.DataVencimentoLicenca = user.DataVencimentoLicenca.AddMonths(1);
                    }
                    _UserRepo.Update(user);           
                }
            }
            catch
            {

            }
            var response = new HttpResponse(TextWriter.Null);
            response.ContentType = "application/json";
            response.StatusCode = 201;
            return response;
        }

        [LoadViewBags]
        public ActionResult check()
        {
            return View();
        }
    }
}