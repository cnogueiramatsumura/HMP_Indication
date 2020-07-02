using DataAccess.Entidades;
using DataAccess.Interfaces;
using DataAccess.Serialized_Objects;
using System;
using System.Collections.Generic;
using System.Linq;


namespace DataAccess.Repository
{
    public class OrdemRepository : RepositoryBase<Ordem>, IOrdemRepository
    {
        public Ordem GetWith_Chamada_and_Symbol(int Id)
        {
            return (from p in db.ordem.Include("Chamada.Symbol") where p.Id == Id select p).FirstOrDefault();
        }
        public Ordem EntradaByBinanceOrderID(string OrderID)
        {
            var res = (from p in db.ordem.Include("Chamada.Symbol").Include("OrdemStatus") where p.OrderID == OrderID select p).FirstOrDefault();
            return res;
        }
        public Ordem SelecionarbyChamadaID(int ChamadaID, int userID)
        {
            var res = (from p in db.ordem where p.Usuario_Id == userID && p.Chamada_Id == ChamadaID && p.TipoOrdem_Id == 1 select p).FirstOrDefault();
            return res;
        }
        public Ordem OcoOrderByBinanceOrderID(string orderID)
        {
            //colocar ordenar descending
            var res = (from p in db.ordem.Include("Chamada") where (p.LimitOrder_ID == orderID || p.StopOrder_ID == orderID) && p.TipoOrdem_Id == 2 orderby p.Id descending select p).FirstOrDefault();
            return res;
        }
        public Ordem GetOcoOrder(int UserId, int chamadaId)
        {
            //obs: && p.BinanceStatus_Id == 1 
            var res = (from p in db.ordem.Include("Chamada.Symbol") where p.Usuario_Id == UserId && p.Chamada_Id == chamadaId && p.TipoOrdem_Id == 2 orderby p.Id descending select p).FirstOrDefault();
            return res;
        }
        public List<Ordem> OrdemsAbertas(int userId)
        {
            var res = (from p in db.ordem.Include("chamada.symbol") where p.Usuario_Id == userId && (p.OrdemStatus_Id == 1 || p.OrdemStatus_Id == 3) && p.MainOrderID == null select p).ToList();
            return res;
        }
        public List<Ordem> GetAllOcosAbertas(int userId)
        {
            var res = (from p in db.ordem where p.Usuario_Id == userId && p.BinanceStatus_Id == 1 && p.MainOrderID != null select p).ToList();
            return res;
        }
        public List<Usuario> EditadasOneSignalIds(int chamadaId)
        {
            var UserIds = (from p in db.ordem where p.BinanceStatus_Id == 1 && p.Chamada_Id == chamadaId && p.TipoOrdem_Id == 2 select p.Usuario_Id).Distinct().ToList();
            var Usuarios = (from p in db.usuario where UserIds.Contains(p.Id) select p).ToList();
            return Usuarios;
        }
        public List<Usuario> TodosUsuarioPosicionados()
        {
            var ordemsAbertas = (from p in db.ordem where (p.OrdemStatus_Id == 1 || p.OrdemStatus_Id == 3) && p.MainOrderID == null select p.Usuario_Id).ToList();
            var usuariosPosicionados = (from p in db.usuario where ordemsAbertas.Contains(p.Id) select p).ToList();
            return usuariosPosicionados;
        }
        public List<Ordem> PosicionadosPorChamada(int chamadaId)
        {
            var ordemsAguardandoEntrada = (from p in db.ordem.Include("Chamada.Symbol").Include("Usuario") where p.Chamada_Id == chamadaId && p.OrdemStatus_Id == 1 && p.MainOrderID == null select p).ToList();
            return ordemsAguardandoEntrada;
        }
        public bool IsValidOrderID(string orderID)
        {
            var res = (from p in db.ordem where p.LimitOrder_ID == orderID || p.StopOrder_ID == orderID || p.OrderID == orderID select p).FirstOrDefault();
            return res != null;
        }
        public List<string> UsuariosPosicionados(int chamadaId)
        {
            var res = (from p in db.ordem where p.BinanceStatus_Id == 1 && p.Chamada_Id == chamadaId && p.TipoOrdem_Id == 2 select p.Usuario_Id.ToString()).ToList();
            return res;
        }
        public List<RelatorioIndividual> RelatorioIndividual(int userId, DateTime datainicio, DateTime datafim)
        {
            var dateformat = db.Database.SqlQuery<string>("select dateformat from syslanguages where name = @@language;").FirstOrDefault();          
            var stringformat = (dateformat == "mdy") ? "MM-dd-yyyy HH:mm:ss" : "dd-MM-yyyy HH:mm:ss";
            var res = db.Database.SqlQuery<RelatorioIndividual>("select Ordem.Id,OrdemStatus.Descricao, Ordem.OrdemStatus_Id, symbol,Symbol.baseAsset, Symbol.quoteAsset, Ordem.DataCadastro, Ordem.PrecoVendaMercado, Chamada.PrecoEntrada, Chamada.PrecoGain, Chamada.PrecoLoss, ChamadaEditada.NewGain, ChamadaEditada.NewLoss, Analista.Nome as 'NomeAnalista' from Ordem inner join Chamada on Ordem.chamada_Id = Chamada.Id inner join Analista on Analista.Id = Chamada.Analista_Id inner join Symbol on symbol.Id = Chamada.Symbol_id inner join OrdemStatus on Ordem.OrdemStatus_Id = OrdemStatus.Id left join ChamadaEditada on ChamadaEditada.Chamada_Id = Chamada.Id and ChamadaEditada.Id =  (select top(1) EdicaoAceita.chamadaEditada_ID from EdicaoAceita where Usuario_Id = " + userId + " and EdicaoAceita.Chamada_ID = Chamada.Id order by id desc) where (Ordem.OrdemStatus_Id = 2  or  Ordem.OrdemStatus_Id = 5 or Ordem.OrdemStatus_Id = 6) and Ordem.MainOrderID is null and Ordem.DataCadastro > '" + datainicio.ToString(stringformat) + "' and Ordem.DataCadastro <  '" + datafim.ToString(stringformat) + "'").ToList();
            return res;
        }
             

        //Api
        public object SelecionarPosicionadas(int userID)
        {
            var res = db.Database.SqlQuery<ActionOrders>("select ordem.Id, ordem.DataCadastro, ordem.Quantidade, ordem.chamada_Id, Ordem.Usuario_Id, OrdemStatus_Id, ordem.DataExecucao,ordem.DataCancelamento,ordem.DataEntrada,ordem.PrecoVendaMercado, Symbol.symbol, Symbol.baseAsset, Symbol.quoteAsset, Chamada.PrecoEntrada, Chamada.RangeEntrada, Chamada.PrecoGain, Chamada.PrecoLoss, ChamadaEditada.NewGain as newgain, ChamadaEditada.NewLoss as newloss, Chamada.Observacao, OrdemStatus.Descricao from Ordem inner join Chamada on Chamada.Id = Ordem.chamada_Id inner join Symbol on Chamada.Symbol_id = Symbol.Id inner join OrdemStatus on Ordem.OrdemStatus_Id = OrdemStatus.Id left join ChamadaEditada on ChamadaEditada.Chamada_Id = Ordem.chamada_Id and ChamadaEditada.Id = (select top(1) chamadaEditada_ID from EdicaoAceita where tipoEdicao_ID = 1 and Usuario_Id = " + userID + " and EdicaoAceita.Chamada_ID = Chamada.Id order by id desc) where ordem.Usuario_Id = " + userID + " and TipoOrdem_Id = 1 and (OrdemStatus_Id = 1 or OrdemStatus_Id = 3)").ToList();
            return res;
        }
        public object SelecionarFinalizadas(int userID)
        {
            var res = db.Database.SqlQuery<ActionOrders>("select ordem.Id, ordem.DataCadastro, ordem.Quantidade, ordem.chamada_Id, Ordem.Usuario_Id, OrdemStatus_Id, ordem.DataExecucao,ordem.DataCancelamento,ordem.DataEntrada,ordem.PrecoVendaMercado, Symbol.symbol, Symbol.baseAsset, Symbol.quoteAsset, Chamada.PrecoEntrada, Chamada.RangeEntrada, Chamada.PrecoGain, Chamada.PrecoLoss, ChamadaEditada.NewGain as newgain, ChamadaEditada.NewLoss as newloss, Chamada.Observacao, OrdemStatus.Descricao from Ordem inner join Chamada on Chamada.Id = Ordem.chamada_Id inner join Symbol on Chamada.Symbol_id = Symbol.Id inner join OrdemStatus on Ordem.OrdemStatus_Id = OrdemStatus.Id left join ChamadaEditada on ChamadaEditada.Chamada_Id = Ordem.chamada_Id and ChamadaEditada.Id = (select top(1) chamadaEditada_ID from EdicaoAceita where tipoEdicao_ID = 1 and Usuario_Id = " + userID + " and EdicaoAceita.Chamada_ID = Chamada.Id order by id desc) where ordem.Usuario_Id = " + userID + " and TipoOrdem_Id = 1 and (OrdemStatus_Id = 2 or OrdemStatus_Id = 5 or OrdemStatus_Id = 6) order by ordem.id desc").ToList();
            return res;
        }
        public object SelecionarCanceladas(int userID)
        {
            var res = db.Database.SqlQuery<ActionOrders>("select ordem.Id, ordem.DataCadastro, ordem.Quantidade, ordem.chamada_Id, Ordem.Usuario_Id, OrdemStatus_Id, ordem.DataExecucao,ordem.DataCancelamento,ordem.DataEntrada,ordem.PrecoVendaMercado, Symbol.symbol, Symbol.baseAsset, Symbol.quoteAsset, Chamada.PrecoEntrada, Chamada.RangeEntrada, Chamada.PrecoGain, Chamada.PrecoLoss, ChamadaEditada.NewGain as newgain, ChamadaEditada.NewLoss as newloss, Chamada.Observacao, OrdemStatus.Descricao from Ordem inner join Chamada on Chamada.Id = Ordem.chamada_Id inner join Symbol on Chamada.Symbol_id = Symbol.Id inner join OrdemStatus on Ordem.OrdemStatus_Id = OrdemStatus.Id left join ChamadaEditada on ChamadaEditada.Chamada_Id = Ordem.chamada_Id and ChamadaEditada.Id = (select top(1) chamadaEditada_ID from EdicaoAceita where tipoEdicao_ID = 1 and Usuario_Id = " + userID + " and EdicaoAceita.Chamada_ID = Chamada.Id order by id desc) where ordem.Usuario_Id = " + userID + " and TipoOrdem_Id = 1 and (OrdemStatus_Id = 4 or OrdemStatus_Id = 7) order by ordem.id desc").ToList();
            return res;
        }
    }
}
