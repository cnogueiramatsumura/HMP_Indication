using DataAccess.Entidades;
using DataAccess.Serialized_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class ChamadasAtivasViewModel
    {
        public List<Chamada> Ativas { get; set; }
        public List<EdicoesAbertas> ChamadaEditadas { get; set; }
        public List<ChamadasCanceladasViewModel> CancelamentoChamadas { get; set; }
    }
}
