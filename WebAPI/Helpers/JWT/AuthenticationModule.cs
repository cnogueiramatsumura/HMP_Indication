using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace WebAPI.Helpers.JWT
{
    public class AuthenticationModule
    {
        public static string EndPoint = "http://dev.test.com.br:90";
        public string CreateToken(int userid, string email)
        {
            //create a identity and add claims to the user which we want to log in
            //passa parametros para ser na API
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, email),
                new  Claim("id",userid.ToString()),
            });

            const string secret = "chavedaapimvpinvest";
            var now = DateTime.UtcNow;
            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(secret));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            return TokenString1(signingCredentials, claimsIdentity);
        }

        private string TokenString1(SigningCredentials SigningCredentials, ClaimsIdentity claimsIdentity)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            //create the jwt
            var token = (JwtSecurityToken)tokenHandler.CreateJwtSecurityToken(issuer: EndPoint, audience: EndPoint, subject: claimsIdentity, notBefore: DateTime.UtcNow, expires: DateTime.UtcNow.AddHours(12), signingCredentials: SigningCredentials);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }

        private string TokenString2(SigningCredentials SigningCredentials, int userid)
        {
            //  Finally create a Token
            var header = new JwtHeader(SigningCredentials);
            //Some PayLoad that contain information about the  customer
            var payload = new JwtPayload
           {
               { "id ", userid}
           };
            var secToken = new JwtSecurityToken(header, payload);
            var handler = new JwtSecurityTokenHandler();
            // Token to String so you can use it in your client
            var tokenString = handler.WriteToken(secToken);
            // And finally when  you received token from client
            // you can  either validate it or try to  read
            var token = handler.ReadJwtToken(tokenString);
            return tokenString;
        }

    }
}