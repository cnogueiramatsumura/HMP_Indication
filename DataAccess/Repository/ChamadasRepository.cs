using System.Collections.Generic;
using DataAccess.Entidades;
using DataAccess.Interfaces;
using System.Linq;
using System;
using DataAccess.Serialized_Objects;
using System.Data.Entity;

namespace DataAccess.Repository
{
    public class ChamadasRepository : RepositoryBase<Chamada>, IChamadasRepository
    {
        public void AtualizarStatusAoReiniciar()
        {
            var TodasChamadasAbertas = (from p in db.chamada where (p.ChamadaStatus_Id == 1 || p.ChamadaStatus_Id == 2) select p).ToList();
            var chamadasIDs = TodasChamadasAbertas.Select(x => x.Id).ToList();
            var Ordens = (from p in db.ordem.Include("OrdemStatus") where chamadasIDs.Contains(p.Chamada_Id) && p.MainOrderID == null select p).ToList();
            if (Ordens != null)
            {
                foreach (var chamadaId in chamadasIDs)
                {
                    var statusOrdens = Ordens.Where(x => x.Chamada_Id == chamadaId).Select(x => x.OrdemStatus_Id).Distinct().ToList();
                    if (statusOrdens.Contains(5))
                    {
                        var chamada = (from p in db.chamada where p.Id == chamadaId select p).FirstOrDefault();
                        chamada.ChamadaStatus_Id = 4;
                        chamada.ResultadoChamada_Id = 1;
                        this.Update(chamada);
                    }
                    else if (statusOrdens.Contains(6))
                    {
                        var chamada = (from p in db.chamada where p.Id == chamadaId select p).FirstOrDefault();
                        chamada.ChamadaStatus_Id = 4;
                        chamada.ResultadoChamada_Id = 2;
                        this.Update(chamada);
                    }
                    else if (statusOrdens.Contains(3))
                    {
                        var chamada = (from p in db.chamada where p.Id == chamadaId select p).FirstOrDefault();
                        chamada.ChamadaStatus_Id = 2;
                        this.Update(chamada);
                    }
                }
            }
        }
        public Chamada GetWith_Symbol(int Id)
        {
            var res = (from p in db.chamada.Include("symbol") where p.Id == Id select p).FirstOrDefault();
            return res;
        }
        public Chamada GetWith_Symbol_and_Filter(int Id)
        {
            var res = (from p in db.chamada.Include("symbol.filters").Include("Analista") where p.Id == Id select p).FirstOrDefault();
            return res;
        }
        public List<Chamada> GetAllOpen()
        {
            var res = (from p in db.chamada.Include("symbol") where (p.ChamadaStatus_Id == 1 || p.ChamadaStatus_Id == 2) select p).ToList();
            return res;
        }
        public List<string> NaoRecusaram_e_nao_Aceitaram_Chamada(int chamadaId)
        {
            //id das pessoas que recusaram a entrada
            var IdRecusaramChamada = (from p in db.chamadasRecusadas where p.Chamada_ID == chamadaId select p.Usuario_ID).ToList();
            //id das pessoas que aceitaram a entrada mas clicaram em cancelar a entrada
            var idsOrdensAceitas = (from p in db.ordem where p.Chamada_Id == chamadaId select p.Usuario_Id).ToList();
            var usersIds = (from p in db.usuario where !IdRecusaramChamada.Contains(p.Id) && !idsOrdensAceitas.Contains(p.Id) select p.Id.ToString()).Distinct().ToList();
            return usersIds;
        }
        public List<Usuario> UsuariosAguardandoEntrada(int chamadaId)
        {
            var idsPosicionados = (from p in db.ordem where p.OrdemStatus_Id == 1 && p.BinanceStatus_Id == 1 && p.Chamada_Id == chamadaId && p.MainOrderID == null select p.Usuario_Id).ToList();
            var users = (from p in db.usuario where idsPosicionados.Contains(p.Id) select p).ToList();
            return users;
        }
        public List<Chamada> AnalistaCancelOrEdit(int Analista_Id)
        {
            var idsOrdens = (from p in db.ordem where p.OrdemStatus_Id == 3 && p.MainOrderID == null select p.Chamada_Id).Distinct().ToList();
            var res = (from p in db.chamada.Include("symbol").Include("Ordems") where ((p.ChamadaStatus_Id == 1) || (p.ChamadaStatus_Id == 2 && idsOrdens.Contains(p.Id))) && p.Analista_Id == Analista_Id select p).ToList();
            return res;
        }
        public bool ChangetoEdit(int chamadaId)
        {
            var ordens = (from p in db.ordem where p.Chamada_Id == chamadaId select p).ToList();
            return ordens.Count > 0;
        }
        public bool isActive(int chamadaid)
        {
            var res = (from p in db.chamada where p.Id == chamadaid && p.ChamadaStatus_Id == 1 select p).FirstOrDefault();
            return res != null;
        }

        //O formato do parse date time esta no formato para servidor em ingles, servidor br trocar para ToString("dd-MM-yyyy HH:mm:ss")
        public List<RelatorioGeral> RelatorioGeral(DateTime datainicio, DateTime datafim)
        {
            var dateformat = db.Database.SqlQuery<string>("select dateformat from syslanguages where name = @@language;").FirstOrDefault();
            var stringformat = (dateformat == "mdy") ? "MM-dd-yyyy HH:mm:ss" : "dd-MM-yyyy HH:mm:ss";
            var res = db.Database.SqlQuery<RelatorioGeral>("select Chamada.Id, Chamada.ResultadoChamada_Id, Chamada.ChamadaStatus_Id, Chamada.DataCadastro, Chamada.PrecoEntrada, Chamada.PrecoGain, Chamada.PrecoLoss,  Symbol.symbol, symbol.baseAsset, symbol.quoteAsset, ChamadaEditada.NewGain, ChamadaEditada.NewLoss, Analista.Nome as 'NomeAnalista' from Chamada inner join Symbol on symbol.Id = Chamada.Symbol_id inner join Analista on Analista.Id = Chamada.Analista_Id left join ChamadaEditada on ChamadaEditada.Chamada_Id = Chamada.Id and ChamadaEditada.Id = (select top(1) ChamadaEditada.Id from ChamadaEditada  where ChamadaEditada.Chamada_Id = Chamada.Id  order by Id desc) where ChamadaStatus_Id = 4 and chamada.DataCadastro > '" + datainicio.ToString(stringformat) + "' and chamada.DataCadastro < '" + datafim.ToString(stringformat) + "'").ToList();
            return res;
        }

        public List<RelatorioGeral> RelatorioGeralAnalista(DateTime datainicio, DateTime datafim, int AnalistaId)
        {
            var dateformat = db.Database.SqlQuery<string>("select dateformat from syslanguages where name = @@language;").FirstOrDefault();
            var stringformat = (dateformat == "mdy") ? "MM-dd-yyyy HH:mm:ss" : "dd-MM-yyyy HH:mm:ss";
            var res = db.Database.SqlQuery<RelatorioGeral>("select Chamada.Id, Chamada.ResultadoChamada_Id, Chamada.ChamadaStatus_Id, Chamada.DataCadastro, Chamada.PrecoEntrada, Chamada.PrecoGain, Chamada.PrecoLoss,  Symbol.symbol, symbol.baseAsset, symbol.quoteAsset, ChamadaEditada.NewGain, ChamadaEditada.NewLoss from Chamada inner join Symbol on symbol.Id = Chamada.Symbol_id left join ChamadaEditada on ChamadaEditada.Chamada_Id = Chamada.Id and ChamadaEditada.Id = (select top(1) ChamadaEditada.Id from ChamadaEditada  where ChamadaEditada.Chamada_Id = Chamada.Id  order by Id desc) where ChamadaStatus_Id = 4 and Analista_Id = " + AnalistaId + " and chamada.DataCadastro > '" + datainicio.ToString(stringformat) + "' and chamada.DataCadastro < '" + datafim.ToString(stringformat) + "'").ToList();
            return res;
        }
        public Chamada RelatorioAceitaramAChamada(int chamadaId)
        {
            var res = db.chamada.Include("Symbol").Include("ChamadaStatus").Where(x => x.Id == chamadaId).FirstOrDefault();
            res.Ordems = db.ordem.Include("Usuario").Include("OrdemStatus").Where(x => x.Chamada_Id == chamadaId && x.MainOrderID == null).ToList();
            return res;
        }



        public List<Chamada> GetAllOpen(int Userid)
        {
            //var datalimite = DateTime.Today;
            var id_chamadasaceitas = db.ordem.Where(x => x.Usuario_Id == Userid).Select(x => x.Chamada_Id).ToList();
            var id_chamadasrecusadas = db.chamadasRecusadas.Where(x => x.Usuario_ID == Userid).Select(x => x.Chamada_ID).ToList();
            var res = (from p in db.chamada.Include("Symbol.filters").Include("Analista") where p.ChamadaStatus_Id == 1 && (!id_chamadasaceitas.Contains(p.Id) && !id_chamadasrecusadas.Contains(p.Id)) orderby p.DataCadastro descending select p).ToList();
            return res;

        }
        public List<Chamada> GetAllClosed(int Userid)
        {
            var datalimite = DateTime.Now.AddDays(-7);
            var id_chamadasaceitas = db.ordem.Where(x => x.Usuario_Id == Userid).Select(x => x.Chamada_Id).ToList();
            var id_chamadasrecusadas = db.chamadasRecusadas.Where(x => x.Usuario_ID == Userid).Select(x => x.Chamada_ID).ToList();
            var res = (from p in db.chamada.Include("Symbol").Include("Analista") where p.DataCadastro > datalimite && p.ChamadaStatus_Id != 1 && (!id_chamadasaceitas.Contains(p.Id) && !id_chamadasrecusadas.Contains(p.Id)) select p).ToList();
            return res;
        }
        public List<Chamada> GetAllRefused(int Userid)
        {
            var datalimite = DateTime.Now.AddDays(-7);
            var id_chamadasrecusadas = db.chamadasRecusadas.Where(x => x.Usuario_ID == Userid).Select(x => x.Chamada_ID).ToList();
            var res = (from p in db.chamada.Include("Symbol").Include("Analista") where p.DataCadastro > datalimite && (id_chamadasrecusadas.Contains(p.Id)) orderby p.DataCadastro descending select p).ToList();
            return res;
        }
    }
}
