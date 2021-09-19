using MakeupApi.Models;
using MakeupApi.Models.DAO;
using MakeupApi.Models.Token;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MakeupApi.Controllers
{
    public class FavoritesController : ApiController
    {
        // POST: api/favorites/newfavorite
        // Adiciona um Favorito (relacionado a um Usuario) do Banco de Dados
        // É necessario passar Makeup(Nome, Marca, Tipo) e User(Email e Senha) + JWT
        [HttpPost]
        [AuthenticationJWT]
        [ActionName("NewFavorite")]
        public HttpResponseMessage AddFavorite(JObject parametersPost)
        {
            // Valida se foi passado parametros via PSOT
            if (parametersPost == null) return Request.CreateErrorResponse(
                    HttpStatusCode.NotFound, "Parametros POST Invalido");

            string[] parametersKey = new string[]
            {
                "Name", "Brand", "Type", "Email", "Password"
            };

            // Verifica se os Parametros Necessarios foram passados
            foreach (string item in parametersKey)
            {
                if (!parametersPost.ContainsKey(item))
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                        "Parametros POST Invalido");
                }
            }

            // Instancia as Classes com os Parametros POST
            dynamic json = parametersPost;
            Makeup makeup = new Makeup()
            {
                Name = json.Name,
                Brand = json.Brand,
                Type = json.Type
            };
            User user = new User()
            {
                Email = json.Email,
                Password = json.Password
            };
            FavoriteDAO favoriteDAO = new FavoriteDAO();

            if (!favoriteDAO.InsertFavorite(makeup, user))
            {
                return Request.CreateErrorResponse(
                    HttpStatusCode.NotFound, favoriteDAO.Error_Operation);
            }

            // Obtem a Nova Maquiagem Inserida nos Favoritos
            int id_newFavorite = favoriteDAO.ReturnIdFavorite(makeup, user);
            makeup = favoriteDAO.SelectFavorite(id_newFavorite);

            if(makeup == null)
            {
                return Request.CreateErrorResponse(
                    HttpStatusCode.NotFound, favoriteDAO.Error_Operation);
            }
            else return Request.CreateResponse(HttpStatusCode.OK, makeup);
        }

        // POST: api/favorites/RemoveFavorite
        // Remove um Favorito (relacionado a um Usuario) do Banco de Dados
        // É necessario passar Makeup(Nome, Marca, Tipo) e User(Email e Senha) + JWT
        [HttpPost]
        [AuthenticationJWT]
        [ActionName("RemoveFavorite")]
        public HttpResponseMessage RemoveFavorite(JObject parametersPost)
        {
            // Valida se foi passado parametros via PSOT
            if (parametersPost == null) return Request.CreateErrorResponse(
                    HttpStatusCode.NotFound, "Parametros POST Invalido");

            string[] parametersKey = new string[]
            {
                "Name", "Brand", "Type", "Email", "Password"
            };

            // Verifica se os Parametros Necessarios foram passados
            foreach (string item in parametersKey)
            {
                if (!parametersPost.ContainsKey(item))
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                        "Parametros POST Invalido");
                }
            }

            // Instancia as Classes com os Parametros POST
            dynamic json = parametersPost;
            Makeup makeup = new Makeup()
            {
                Name = json.Name,
                Brand = json.Brand,
                Type = json.Type
            };
            User user = new User()
            {
                Email = json.Email,
                Password = json.Password
            };

            FavoriteDAO favoriteDAO = new FavoriteDAO();

            int id_favorite = favoriteDAO.ReturnIdFavorite(makeup, user);

            if (!favoriteDAO.DeleteFavorite(id_favorite))
            {
                return Request.CreateErrorResponse(
                    HttpStatusCode.NotFound, favoriteDAO.Error_Operation);
            }
            else return Request.CreateResponse(HttpStatusCode.OK, true);
        }


        // POST: api/favorites/ListFavorites
        // Remove um Favorito (relacionado a um Usuario) do Banco de Dados
        // É necessario passar um User(Email e Senha) + JWT
        [HttpPost]
        [AuthenticationJWT]
        [ActionName("ListFavorites")]
        public HttpResponseMessage UserMakeupFavorites([FromBody] User user)
        {
            if (user == null) return Request.CreateErrorResponse(
          HttpStatusCode.NotFound, "Parametros POST Invalido");

            FavoriteDAO favoriteDAO = new FavoriteDAO();

            List<Makeup> listFavoritesMakeup = favoriteDAO.ListFavoritesUser(user);

            if (listFavoritesMakeup == null)
            {
                return Request.CreateErrorResponse(
                    HttpStatusCode.NotFound, favoriteDAO.Error_Operation);
            }
            else return Request.CreateResponse(HttpStatusCode.OK, listFavoritesMakeup);
        }
    }
}
