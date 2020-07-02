using DataAccess.Entidades;
using DataAccess.Repository;
using DataAccess.Serialized_Objects;
using System;
using System.Collections.Generic;


namespace DataAccess.Interfaces
{
    public interface IChamadasRepository : IRepositoryBase<Chamada>
    {
        Chamada GetWith_Symbol(int id);
        Chamada GetWith_Symbol_and_Filter(int Id);
     
        bool ChangetoEdit(int chamadaId);
        List<Chamada> GetAllOpen(int Userid);
        List<Chamada> GetAllClosed(int Userid);
        List<Chamada> GetAllRefused(int Userid);
        List<Chamada> GetAllOpen();
        List<Chamada> AnalistaCancelOrEdit(int Analista_Id);
        List<string> NaoRecusaram_e_nao_Aceitaram_Chamada(int chamadaId);
        List<Usuario> UsuariosAguardandoEntrada(int chamadaId);
        List<RelatorioGeral> RelatorioGeral(DateTime datainicio, DateTime datafim);
        List<RelatorioGeral> RelatorioGeralAnalista(DateTime datainicio, DateTime datafim, int Analista_Id);
        Chamada RelatorioAceitaramAChamada(int chamadaId);       

    }
}
