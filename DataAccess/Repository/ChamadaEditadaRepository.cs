using DataAccess.Entidades;
using DataAccess.Interfaces;
using DataAccess.Serialized_Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Repository
{
    public class ChamadaEditadaRepository : RepositoryBase<ChamadaEditada>, IChamadaEditadaRepository
    {
        public List<EdicoesAbertas> GetAllOpen(int Userid)
        {
            var res = db.Database.SqlQuery<EdicoesAbertas>("select ChamadaEditada.*,baseAsset,quoteAsset,symbol, Chamada.PrecoEntrada,Chamada.RangeEntrada, Chamada.PrecoGain, Chamada.PrecoLoss from ChamadaEditada inner join Chamada on Chamada_Id = Chamada.Id inner join Symbol on Symbol.Id = Chamada.Symbol_id where ChamadaEditada.id not in (select chamadaEditada_ID from EdicaoAceita where Usuario_Id = " + Userid + ") and ChamadaEditada.id in (select max(id) from ChamadaEditada group by Chamada_Id) and Chamada_Id in (select distinct chamada_Id from ordem where TipoOrdem_Id = 1 and OrdemStatus_Id = 3 and Usuario_Id = " + Userid + " ) and Chamada.ChamadaStatus_Id <> 4 order by id desc;").ToList();
            return res;
        }

        public List<ChamadaEditada> GetListEdit(int chamadaId)
        {
            var res = db.chamadaEditada.Where(x => x.Chamada_Id == chamadaId).ToList();
            return res;
        }
    }
}
