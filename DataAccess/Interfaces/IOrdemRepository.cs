using DataAccess.Entidades;
using DataAccess.Serialized_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface IOrdemRepository : IRepositoryBase<Ordem>
    {
        Ordem GetWith_Chamada_and_Symbol(int Id);
        Ordem SelecionarbyChamadaID(int ChamadaID,int userID);
        Ordem EntradaByBinanceOrderID(string OrderID);
        Ordem OcoOrderByBinanceOrderID(string OrdemId);
        Ordem GetOcoOrder(int UserId, int chamadaId);
        bool IsValidOrderID(string OrdemId);
        List<string> UsuariosPosicionados(int ChamadaID);
        List<Ordem> OrdemsAbertas(int userId);
        List<RelatorioIndividual> RelatorioIndividual(int userId, DateTime datainicio, DateTime datafim);
        List<Usuario> EditadasOneSignalIds(int chamadaId);
        List<Ordem> PosicionadosPorChamada(int chamadaId);

        object SelecionarPosicionadas(int userID);
        object SelecionarFinalizadas(int userID);
        object SelecionarCanceladas(int userID);

    }
}
