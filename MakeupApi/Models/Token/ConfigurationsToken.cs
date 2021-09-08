using Microsoft.IdentityModel.Tokens;

namespace MakeupApi.Models.Token
{
    public class ConfigurationsToken
    {
        public static string GetIssuer()
        {
            string issuer = System.Configuration.ConfigurationManager.AppSettings["issuer"];
            return issuer;
        }

        public static string GetAudience()
        {
            string audience = System.Configuration.ConfigurationManager.AppSettings["audience"];
            return audience;
        }

        public static SecurityKey GetSecurityKey()
        {
            // Obtem a Chave do Token da Classe 'JsonWebKeyApp' 
            KeyJWT jsonWebKey = new KeyJWT();
            return jsonWebKey.SecurityKey;
        }

    }
}