using DataAccess.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface IServerConfigRepository : IRepositoryBase<ServerConfig>
    {
        ServerConfig GetAllConfig();
        string GetApiServer();
        string GetAppServer();
        string GetOneSignalToken();
        decimal GetLicencePrice();
    }
}
