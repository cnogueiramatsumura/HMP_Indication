using DataAccess.Entidades;
using DataAccess.Helpers;
using DataAccess.Interfaces;
using DataAccess.Repository;
using DataAccess.Serialized_Objects;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using WebAPI.Helpers.JWT;

namespace WebAPI.Helpers
{
    public class Helper
    {
        public static string GetJWTPayloadValue(HttpRequestMessage Request, string payloadkey)
        {
            string tokenString;
            IEnumerable<string> authzHeaders;
            Request.Headers.TryGetValues("Authorization", out authzHeaders);
            var bearerToken = authzHeaders.ElementAt(0);
            tokenString = bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(tokenString);
            var payloaditem = token.Payload.FirstOrDefault(x => x.Key == payloadkey);
            return payloaditem.Value.ToString();
        }

        public static bool ValidadeJWTToken(string token)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes("chavedaapimvpinvest"));
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters()
                {
                    ValidAudience = "http://dev.test.com.br:90",
                    ValidIssuer = "http://dev.test.com.br:90",
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    //LifetimeValidator = LifetimeValidator,
                    LifetimeValidator = new TokenValidationHandler().LifetimeValidator,
                    IssuerSigningKey = securityKey
                };
                SecurityToken validatedToken;
                tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
                if (validatedToken != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (SecurityTokenValidationException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static string EncryptSha512(string inputString)
        {
            SHA512 sha512 = SHA512Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);
            byte[] hash = sha512.ComputeHash(bytes);
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }

        public static string GenerateRandomOcoOrderID(int length)
        {
            const string alphanumericCharacters = "abcdefghijklmnopqrstuvwxyz" + "0123456789";
            //const string alphanumericCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ" + "abcdefghijklmnopqrstuvwxyz" + "0123456789";
            if (length < 0)
                throw new ArgumentException("length must not be negative", "length");
            if (length > int.MaxValue / 8)
                throw new ArgumentException("length is too big", "length");

            var characterArray = alphanumericCharacters.Distinct().ToArray();
            if (characterArray.Length == 0)
                throw new ArgumentException("characterSet must not be empty", "characterSet");

            var bytes = new byte[length * 8];

            using (var rNGCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                rNGCryptoServiceProvider.GetBytes(bytes);
                var result = new char[length];
                for (int i = 0; i < length; i++)
                {
                    var value = BitConverter.ToUInt64(bytes, i * 8);
                    result[i] = characterArray[value % (uint)characterArray.Length];
                }
                return new string(result);
            }
        }

   


        public static decimal ArredondarQuantidadeVenda(List<OrdemComission> Comissoes, Ordem MainOrdem)
        {
            using (var FilterRepo = new FiltersRepository())
            {
                var filtro = FilterRepo.GetBySymbol_Type("LOT_SIZE", MainOrdem.Chamada.Symbol_id);
                if (Comissoes.Count == 1)
                {
                    var asset = Comissoes.FirstOrDefault();
                    if (asset.ComissionAsset != "BNB")
                    {
                        if (filtro.stepSize == 1)
                        {
                            return (int)(MainOrdem.Quantidade - asset.ComissionAmount);
                        }
                        else
                        {
                            var numerocasas = filtro.stepSize.ToString().IndexOf("1") - 1;
                            var multiplicador = Math.Pow(10, numerocasas);
                            var n1 = (MainOrdem.Quantidade - asset.ComissionAmount) * (decimal)multiplicador;
                            return Math.Floor(n1) / (decimal)multiplicador;
                        }
                    }
                    else
                    {
                        return MainOrdem.Quantidade;
                    }
                }
                else
                {
                    var symbol = MainOrdem.Chamada.Symbol.symbol;
                    var symbolAsset = symbol.Substring(0, symbol.Length - 3).ToUpper();
                    var group = Comissoes.GroupBy(x => x.ComissionAsset);
                    //var ComissoesBNB = Comissoes.Where(x => x.ComissionAsset == "BNB").Sum(x => x.ComissionAmount);
                    var ComissoesMoedaCorrente = Comissoes.Where(x => x.ComissionAsset == symbolAsset).Sum(x => x.ComissionAmount);
                    if (ComissoesMoedaCorrente > 0)
                    {
                        if (filtro.stepSize == 1)
                        {
                            return (int)(MainOrdem.Quantidade - ComissoesMoedaCorrente);
                        }
                        else
                        {
                            var numerocasas = filtro.stepSize.ToString().IndexOf("1") - 1;
                            var multiplicador = Math.Pow(10, numerocasas);
                            var n1 = (MainOrdem.Quantidade - ComissoesMoedaCorrente) * (decimal)multiplicador;
                            return Math.Floor(n1) / (decimal)multiplicador;
                        }
                    }
                    else
                    {
                        return MainOrdem.Quantidade;
                    }
                }
            }
        }

        public static decimal OcoStopLimitWithPercent(int symbolID, decimal LimitPriece, decimal Percent)
        {
            var FilterRepo = new FiltersRepository();
            var filtro = FilterRepo.GetBySymbol_Type("PRICE_FILTER", symbolID);
            var numerocasas = filtro.tickSize.ToString().IndexOf("1") - 1;
            var umporcento = (LimitPriece / 100);
            var Limitaceito = LimitPriece - (umporcento * Percent);
            return Math.Round(Limitaceito, numerocasas);
        }

        public static void AtualizarOrdens(Usuario user)
        {
            using (var _OrdemRepo = new OrdemRepository())
            {
                var ordensabertas = _OrdemRepo.OrdemsAbertas(user.Id);
                if (ordensabertas.Count > 0)
                {
                    foreach (var item in ordensabertas)
                    {
                        var res = BinanceRestApi.RecentTraders(user.BinanceAPIKey, user.BinanceAPISecret, item.Chamada.Symbol.symbol);
                        if (res.IsSuccessStatusCode)
                        {
                            var jsonObj = res.Content.ReadAsStringAsync().Result;
                            var lista = JsonConvert.DeserializeObject<List<order>>(jsonObj);
                            var oco = _OrdemRepo.GetOcoOrder(user.Id, item.Chamada_Id);
                            if (oco != null && oco.MotivoCancelamento == null)
                            {
                                var ocoorder1 = lista.Where(x => x.clientOrderId == oco.LimitOrder_ID).FirstOrDefault();
                                var ocoorder2 = lista.Where(x => x.clientOrderId == oco.StopOrder_ID).FirstOrDefault();
                                // se bateu gain ou loss
                                if (ocoorder1.status == "FILLED" || ocoorder2.status == "FILLED")
                                {
                                    var filledOrder = lista.Where(x => x.clientOrderId == oco.LimitOrder_ID || x.clientOrderId == oco.StopOrder_ID && x.status == "FILLED").FirstOrDefault();
                                    if (filledOrder != null && filledOrder.executedQty > 0)
                                    {
                                        if (filledOrder.price >= item.Chamada.PrecoEntrada)
                                        {
                                            var mymain = _OrdemRepo.SelecionarbyChamadaID(item.Chamada_Id, user.Id);
                                            mymain.OrdemStatus_Id = 5;
                                            mymain.BinanceStatus_Id = 3;
                                            mymain.DataExecucao = new DateTimeOffset((10000 * filledOrder.time) + 621355968000000000, TimeSpan.Zero);
                                            _OrdemRepo.Update(mymain);
                                            //oco.ValorExecutado = binanceOrder.price;                                     
                                            oco.OrdemStatus_Id = 5;
                                            oco.BinanceStatus_Id = 3;
                                            _OrdemRepo.Update(oco);
                                        }
                                        else if (filledOrder.price <= item.Chamada.PrecoEntrada)
                                        {
                                            var mymain = _OrdemRepo.SelecionarbyChamadaID(item.Chamada_Id, user.Id);
                                            mymain.OrdemStatus_Id = 6;
                                            mymain.BinanceStatus_Id = 3;
                                            mymain.DataExecucao = new DateTimeOffset((10000 * filledOrder.time) + 621355968000000000, TimeSpan.Zero);
                                            _OrdemRepo.Update(mymain);
                                            //oco.ValorExecutado = binanceOrder.price;                                     
                                            oco.OrdemStatus_Id = 6;
                                            oco.BinanceStatus_Id = 3;
                                            _OrdemRepo.Update(oco);
                                        }
                                    }
                                }
                                //se a ordem foi canelada
                                else if (ocoorder1.status == "CANCELED" && ocoorder2.status == "CANCELED")
                                {
                                    oco.BinanceStatus_Id = 4;
                                    oco.OrdemStatus_Id = 4;
                                    _OrdemRepo.Update(oco);

                                    item.DataCancelamento = new DateTimeOffset((10000 * ocoorder1.updateTime) + 621355968000000000, TimeSpan.Zero);
                                    item.BinanceStatus_Id = 4;
                                    item.OrdemStatus_Id = 4;
                                    _OrdemRepo.Update(item);
                                }
                            }
                            else
                            {
                                var ordemCancelada = lista.Where(x => x.clientOrderId == item.OrderID).FirstOrDefault();
                                if (ordemCancelada != null && ordemCancelada.status == "CANCELED")
                                {
                                    item.DataCancelamento = new DateTimeOffset((10000 * ordemCancelada.updateTime) + 621355968000000000, TimeSpan.Zero);
                                    item.BinanceStatus_Id = 4;
                                    item.OrdemStatus_Id = 4;
                                    _OrdemRepo.Update(item);
                                }
                            }
                        }
                    }
                }
                //else
                //{
                //    var OcosAbertas = _OrdemRepo.GetAllOcosAbertas(user.Id);
                //    foreach (var ocoAberta in OcosAbertas)
                //    {
                //        var mainOrder = _OrdemRepo.GetById((int)ocoAberta.MainOrderID);
                //        ocoAberta.BinanceStatus_Id = mainOrder.BinanceStatus_Id;
                //        ocoAberta.OrdemStatus_Id = mainOrder.OrdemStatus_Id;
                //        //obs: da pra colocar ocoOrdem data de cancelament e data de execuçao mas nao fiz para evitar request na binance;                              
                //        _OrdemRepo.Update(ocoAberta);
                //    }
                //}
            }
        }

        public static void ReconectarAtivosDesconectados()
        {
            using (var _chamadasRepo = new ChamadasRepository())
            {
                _chamadasRepo.AtualizarStatusAoReiniciar();
                var chamadasabertas = _chamadasRepo.GetAllOpen();
                var monitor = Market_Monitor.Instancia;
                monitor.LimparMonitoramento();
                foreach (var chamada in chamadasabertas)
                {
                    _chamadasRepo.AddReference(chamada, "Symbol");
                    monitor.AddMonitor(chamada, chamada.Symbol.symbol.ToLower());
                    _chamadasRepo.Detach(chamada);
                }
            }
        }

        public static void ReconectarClientesComOrdensAbertas()
        {
            using (var _OrdemRepo = new OrdemRepository())
            {
                var usuariosPosicionados = _OrdemRepo.TodosUsuarioPosicionados();
                var wsMonitor = WSMonitor.Instancia;
                foreach (var usuario in usuariosPosicionados)
                {
                    wsMonitor.AddMonitor(usuario);
                }
            }
        }

        public static void CreateLogFolder()
        {
            Logs.CreateLogFolder();
        }

        /// <summary>
        /// Objeto de Error da Api Binance
        /// </summary>
        /// <param name="BinanceResult"></param>
        /// <returns></returns>
        public static BinanceErrors GetBinanceErrorObj(HttpResponseMessage BinanceResult)
        {
            var result = BinanceResult.Content.ReadAsStringAsync().Result;
            var ErrorObj = JsonConvert.DeserializeObject<BinanceErrors>(result);
            var returnErrorObj = ApiBinanceErrorMsgs.GetError(ErrorObj);
            return returnErrorObj;
        }

    }
}
