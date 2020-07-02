using DataAccess.Entidades;
using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class EdicaoAceitaRepository : RepositoryBase<EdicaoAceita>, IEdicaoAceitaRepository
    {
        public EdicaoAceita AceitouEdicao(int UserID, int ChamadaID)
        {
            var res = (from p in db.edicaoAceita.Include("ChamadaEditada") where p.Usuario_Id == UserID && p.Chamada_ID == ChamadaID orderby p.Id descending select p).FirstOrDefault();
            return res;
        }
    }
}
