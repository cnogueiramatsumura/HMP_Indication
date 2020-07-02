using DataAccess.Entidades;
using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class ServerConfigRepository : RepositoryBase<ServerConfig>, IServerConfigRepository
    {
        public ServerConfig GetAllConfig()
        {
            return db.serverConfig.FirstOrDefault();
        }

        public string GetApiServer()
        {
            return db.serverConfig.FirstOrDefault().ApiServer;
        }

        public string GetAppServer()
        {
            return db.serverConfig.FirstOrDefault().AppServer;
        }
        
        public string GetOneSignalToken()
        {
            return db.serverConfig.FirstOrDefault().OneSignalToken;
        }

        public decimal GetLicencePrice()
        {
            return db.serverConfig.FirstOrDefault().PrecoLicenca;
        }
    }
}
