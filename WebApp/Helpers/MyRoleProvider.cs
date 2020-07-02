using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace WebApp.Helpers
{
    public class MyRoleProvider : RoleProvider
    {
        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            string[] roles = new string[] { "Users", "Analista" };
            return roles;
        }

        public override string[] GetRolesForUser(string username)
        {            
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return null;
            }
            string[] roles;
            using (var repo = new AnalistaRepository())
            {
                var Analista = repo.GetByEmail(username);
                if (Analista != null)
                {
                    return roles = new string[] { "Analista" };
                }
            }            
            roles = new string[] { "Users" };
            return roles;
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            if (roleName == "Admin")
            {
                if (username == "admin@admin.com.br")
                {
                    return true;
                }
            }
            else if (roleName == "Users")
            {
                if (username != "admin@admin.com.br" && username != null)
                {
                    return true;
                }
            }
            return false;
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}