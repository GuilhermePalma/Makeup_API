using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace MakeupApi.Models.Token
{
    public class KeyJWT
    {
        // Local do arquivo com a Chave Gerada e Salva
        private static readonly string myJWKeyPath= Path.
            Combine(Environment.CurrentDirectory,"Template", "my_secret_key_jwt.json");
        // Gera uma sequencia de Numeros Aleatorios
        private static RandomNumberGenerator randomNumberGenerator =
            RandomNumberGenerator.Create();

        private SecurityKey securityKey;


        public KeyJWT()
        {
            this.securityKey = loadKey();
        }


        private static SecurityKey loadKey()
        {
            if (File.Exists(myJWKeyPath))
            {
                // Recupera/Desserializa e Retorna os Valores da Key do JSON Armazenada
                var storedJsonWebKey = JsonConvert.
                    DeserializeObject<Microsoft.IdentityModel.Tokens.JsonWebKey>
                    (File.ReadAllText(myJWKeyPath));
                return storedJsonWebKey;
            }
            else
            {
                var newKey = createJsonWebKey();
                File.WriteAllText(myJWKeyPath, JsonConvert.SerializeObject(newKey));
                return newKey;
            }
            
        }


        private static JsonWebKey createJsonWebKey()
        {
            // Criptografa a chave gerada na base SHA256 e usa na geração de uma Chave Simetrica
            var symetricKey = new HMACSHA256(generateRandonKey(64));
            SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(symetricKey.Key);

            // Converte da chave Simetrica para JsonWebKey
            var jsonWebKey = JsonWebKeyConverter.ConvertFromSymmetricSecurityKey(symmetricSecurityKey);

            // Gera um ID para a o JsonWebKey
            jsonWebKey.KeyId = Base64UrlEncoder.Encode(generateRandonKey(16));
            return jsonWebKey;
        }

        private static byte[] generateRandonKey(int bytes)
        {
            // Gera e Criptografa em Cima da Sequencia de 64Bytes 
            byte[] data = new byte[bytes];
            randomNumberGenerator.GetBytes(data);
            return data;
        }

        // Obtem e Define o Valor da Chave
        public SecurityKey SecurityKey { get => securityKey; set => securityKey = value; }
    }
}