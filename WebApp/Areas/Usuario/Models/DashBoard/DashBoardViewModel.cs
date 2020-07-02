using DataAccess.Entidades;
using DataAccess.Serialized_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Usuario.Models.DashBoard
{
    public class DashBoardViewModel
    {
        public List<Chamada> Chamadas { get; set; }
        public List<ActionOrders> Ordems { get; set; }
        public List<EdicoesAbertas> ChamadaEditadas { get; set; }
        public List<ChamadasCanceladasViewModel> ChamadasCanceladas { get; set; }
    }
}