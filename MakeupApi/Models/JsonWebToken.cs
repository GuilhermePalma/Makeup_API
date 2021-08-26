using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace MakeupApi.Models
{
    public class JsonWebToken
    {
        // todo: alterar p/ o Heroku
        private const string locationAPI = "https://github.com/guilhermepalma/makeup_api";
        private const string mobileUsed = "https://github.com/guilhermepalma/makeup";

        public string generateToken(string name, int id)
        {
            JsonWebKeyApp key = new JsonWebKeyApp();
            
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, name),
                    new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                }),
                Issuer = locationAPI,
                IssuedAt = DateTime.Today,
                Expires = DateTime.Now.AddDays(7),
                Audience = mobileUsed,
                // Obtem a Chave da Classe 'JsonWebKeyApp' 
                SigningCredentials = new SigningCredentials(new JsonWebKeyApp().SecurityKey,
                    SecurityAlgorithms.HmacSha256Signature)

            };

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken securityToken = handler.CreateJwtSecurityToken(tokenDescriptor);
            return handler.WriteToken(securityToken);
             
        }
    }
}