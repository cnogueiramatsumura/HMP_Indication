using DataAccess.Repository;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Web;
using WebAPI.Helpers;

namespace WebAPI.SignalR
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string GetUserId(IRequest request)
        {
            var role = request.QueryString["role"];
            if (role == "admin")
            {
                return "admin";
            }
            else
            {               
                var tokenString = request.QueryString["token"];
                var handler = new JwtSecurityTokenHandler();
                if (tokenString != null && Helper.ValidadeJWTToken(tokenString))
                {
                    var token = handler.ReadJwtToken(tokenString);
                    var payloaditem = token.Payload.FirstOrDefault(x => x.Key == "id");
                    return payloaditem.Value.ToString();
                }
                else
                {
                    return "anonimo";
                }
            }
        }
    }
}