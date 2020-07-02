using DataAccess.Entidades;
using DataAccess.Interfaces;
using DataAccess.Serialized_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class CancelamentoChamadasRepository : RepositoryBase<CancelamentoChamada>, ICancelamentoChamadaRepository
    {
        public List<ChamadasCanceladasViewModel> GetAllOpen(int UsuarioId)
        {
            var res = db.Database.SqlQuery<ChamadasCanceladasViewModel>("select CancelamentoChamada.*, PrecoEntrada, RangeEntrada,PrecoGain,PrecoLoss,Observacao,Ordem.DataCadastro,Ordem.Id as 'OrdemId',Quantidade, symbol, baseAsset, quoteAsset from CancelamentoChamada inner join Chamada on CancelamentoChamada.Chamada_Id = Chamada.Id inner join Ordem on Ordem.chamada_Id = Chamada.Id inner join Symbol on Chamada.Symbol_id = Symbol.Id where CancelamentoChamada.Id not in (select CancelamentoChamada_Id from CancelamentoRecusado where Usuario_Id = " + UsuarioId + ") and Chamada.ChamadaStatus_Id = 3 and MainOrderID is null and OrdemStatus_Id = 1 and Usuario_Id = " + UsuarioId + ";").ToList();
            return res;
        }
    }
}
