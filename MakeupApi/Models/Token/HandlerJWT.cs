using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace MakeupApi.Models.Token
{
    public class HandlerJWT
    {
        // todo: alterar p/ o Heroku
        private string locationAPI = ConfigurationsToken.GetIssuer();
        private string mobileUsed = ConfigurationsToken.GetAudience();
        private SecurityKey securityKey = ConfigurationsToken.GetSecurityKey();

        public string generateToken(string name, int id)
        {
            
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, name),
                    new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                }),
                Issuer = locationAPI,
                IssuedAt = DateTime.Today,
                Expires = DateTime.UtcNow.AddDays(5),
                Audience = mobileUsed,
                // Obtem a Chave da Classe 'JsonWebKeyApp' 
                SigningCredentials = new SigningCredentials(
                    securityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken securityToken = handler.CreateJwtSecurityToken(tokenDescriptor);
            return handler.WriteToken(securityToken);   
        }

        public static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                // Obtem e Lê o Token Recebido
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtTokenReciver = 
                    (JwtSecurityToken)tokenHandler.ReadToken(token);

                if (jwtTokenReciver == null) return null;

                // Cria os parametros que serão validados
                TokenValidationParameters parameters = new TokenValidationParameters()
                {
                    ValidIssuer = ConfigurationsToken.GetIssuer(),
                    ValidateIssuer = true,

                    ValidAudience = ConfigurationsToken.GetAudience(),
                    ValidateAudience = true,

                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,

                    IssuerSigningKey = ConfigurationsToken.GetSecurityKey(),
                    ValidateIssuerSigningKey = true
                };

                // Compara/Valida os Parametros Esperados e o Token Recebido
                SecurityToken securityToken;
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, parameters,
                    out securityToken);

                return principal;
            }
            catch
            {
                return null;
            }
        }

        public static bool validationToken(string token)
        {
            // Obtem os valores do Token (caso seja valido)
            ClaimsPrincipal principal = GetPrincipal(token);
            if (principal == null) return false;

            // Obtem as declarações do TOken
            ClaimsIdentity identity;
            try
            {
                identity = (ClaimsIdentity)principal.Identity;
            }
            catch (NullReferenceException)
            {
                return false;
            }

            // Obtem os Valores do Token
            Claim usernameClaim = identity.FindFirst(ClaimTypes.Name);
            Claim idClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

            if (usernameClaim == null || idClaim == null) return false;

            string username = usernameClaim.Value;
            int id;

            try
            {
                id = int.Parse(idClaim.Value);
            }
            catch(FormatException)
            {
                // Erro no formato do String p/ Int
                return false;
            }

            // todo: adicionar validação se o usuario e id existe no bd
            // Valida o Usuario
            if(!string.IsNullOrEmpty(username) && id != 0)
            {
                return username.Equals("admin") && id == 1;
            }
            return false;
        }

    }
}