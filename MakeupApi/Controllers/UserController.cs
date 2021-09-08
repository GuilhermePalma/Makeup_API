using MakeupApi.Models;
using MakeupApi.Models.DAO;
using MakeupApi.Models.Token;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MakeupApi.Controllers
{
    public class UserController : ApiController
    {

        // POST: api/user/GenerateTokenLogin
        // Se o Usuario Existir no Banco de Dados ---> Gera o Token
        [HttpPost]
        [ActionName("GenerationToken")]
        public HttpResponseMessage GenerationTokenUser([FromBody] User user)
        {
            if (user == null) return Request.CreateErrorResponse(
                     HttpStatusCode.NotFound, "Parametros POST Invalido");

            UserDAO userDAO = new UserDAO();

            // Obtem o Id do Usuario Informado (Atraves do E-mail e Senha)
            user.Id = userDAO.ReturnIdUser(user.Email, user.Password);

            User userDatabase = new User();
            userDatabase = userDAO.SelectUser(user.Id);

            if (userDatabase == null)
            {
                return Request.CreateErrorResponse(
                    HttpStatusCode.NotFound, userDAO.Error_operation);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK,
                    new HandlerJWT().GenerateToken(userDatabase.Nickname, userDatabase.Id));
            }
        }

        // POST: api/user/Login
        // A partir de um Email + Senha + Token e Valida o Usuario. Retorna um Booleano
        [HttpPost]
        [AuthenticationJWT]
        [ActionName("Login")]
        public HttpResponseMessage ValidateLogin([FromBody] User user)
        {
            if (user == null) return Request.CreateErrorResponse(
                        HttpStatusCode.NotFound, "Parametros POST Invalido");

            UserDAO userDAO = new UserDAO();

            // Obtem o Id do Usuario Informado (Atraves do E-mail e Senha)
            bool exist_user = userDAO.ExistsUser(user.Email, user.Password);

            if (!exist_user)
            {
                return Request.CreateErrorResponse(
                    HttpStatusCode.NotFound, userDAO.Error_operation);
            }
            else return Request.CreateResponse(HttpStatusCode.OK, true);
        }

        // GET: api/user/Information/{id}
        // Obtem informações do Usuario atraves do Email e Senha
        [HttpGet]
        [ActionName("Information")]
        [AuthenticationJWT]
        public HttpResponseMessage GetInformation(int id)
        {
            UserDAO userDAO = new UserDAO();

            User userDatabase = new User();
            userDatabase = userDAO.SelectUser(id);

            // Obtem o Id do Usuario Informado (Atraves do E-mail e Senha)
            if (userDatabase == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                    "ID do Usuario não Encontado");
            }
            else return Request.CreateResponse(HttpStatusCode.OK, userDatabase);
        }

        // POST: api/user/Register
        // Cadastra o Usuario (Caso não Exista)
        [HttpPost]
        [ActionName("Register")]
        public HttpResponseMessage Insert([FromBody] User user)
        {
            if (user == null) return Request.CreateErrorResponse(
                        HttpStatusCode.NotFound, "Parametros POST Invalido");

            UserDAO userDAO = new UserDAO();

            if (!userDAO.InsertUser(user))
            {
                return Request.CreateErrorResponse(
                    HttpStatusCode.NotFound, userDAO.Error_operation);
            }

            // Obtem o Codigo do Usuario
            user.Id = userDAO.ReturnIdUser(user.Email, user.Password);

            // Gera um Token (JWT) caso o Codigo seja valido
            if (user.Id <= 0)
            {
                return Request.CreateErrorResponse(
                    HttpStatusCode.NotFound, userDAO.Error_operation);
            }
            else return Request.CreateResponse(HttpStatusCode.OK,
              new HandlerJWT().GenerateToken(user.Nickname, user.Id));
        }

        // PUT: api/user/Update/
        // Atualiza Todos os Dados do usuario (Necessario Email, Senha e ID)
        [HttpPut]
        [AuthenticationJWT]
        [ActionName("Update")]
        public HttpResponseMessage Update([FromBody] User user)
        {
            if (user == null) return Request.CreateErrorResponse(
                     HttpStatusCode.NotFound, "Parametros PUT Invalido");

            UserDAO userDAO = new UserDAO();
            bool isUpdated = userDAO.UpdateUser(user);
            if (!isUpdated)
            {
                return Request.CreateErrorResponse(
                    HttpStatusCode.NotFound, userDAO.Error_operation);
            }
            else return Request.CreateResponse(HttpStatusCode.OK, isUpdated);
        }

        // PUT: api/user/UpdateNickname
        // Atualiza apenas o Nickname/Nome de Usuario (Necessario Email, Senha, ID e Nickname)
        [HttpPut]
        [AuthenticationJWT]
        [ActionName("UpdateNickname")]
        public HttpResponseMessage UpdateNickname([FromBody] User user)
        {
            if (user == null) return Request.CreateErrorResponse(
                     HttpStatusCode.NotFound, "Parametros PUT Invalido");

            UserDAO userDAO = new UserDAO();

            if (!userDAO.UpdateNickname(user))
            {
                return Request.CreateErrorResponse(
                    HttpStatusCode.NotFound, userDAO.Error_operation);
            }

            // Obtem o Usuario atualizado para Gerar um novo Token (JWT)
            User userUpdate = new User();
            userUpdate = userDAO.SelectUser(user.Id);

            if (userUpdate == null)
            {
                return Request.CreateErrorResponse(
                    HttpStatusCode.NotFound, userDAO.Error_operation);
            }
            else return Request.CreateResponse(HttpStatusCode.OK,
                    new HandlerJWT().GenerateToken(userUpdate.Nickname, userUpdate.Id));
        }

        // PUT: api/user/UpdateEmail
        // Atualiza somente o Email (Necessario Email, Senha, ID e Email)
        [HttpPut]
        [AuthenticationJWT]
        [ActionName("UpdateEmail")]
        public HttpResponseMessage UpdateEmail([FromBody] User user)
        {
            if (user == null) return Request.CreateErrorResponse(
                     HttpStatusCode.NotFound, "Parametros PUT Invalido");

            UserDAO userDAO = new UserDAO();

            if (!userDAO.UpdateEmail(user))
            {
                return Request.CreateErrorResponse(
                    HttpStatusCode.NotFound, userDAO.Error_operation);
            }
            else return Request.CreateResponse(HttpStatusCode.OK, true);
        }

        // DELETE: api/User/Delete
        // Exclui o Usuario do Banco de Dados (Necessario Email, Senha e ID)
        [HttpDelete]
        [AuthenticationJWT]
        [ActionName("Delete")]
        public HttpResponseMessage Delete([FromBody] User user)
        {
            if (user == null) return Request.CreateErrorResponse(
                     HttpStatusCode.NotFound, "Parametros DELETE Invalido");

            UserDAO userDAO = new UserDAO();
            bool is_deletedUser = userDAO.DeleteUser(user);

            if (!is_deletedUser)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                    userDAO.Error_operation);
            }
            else return Request.CreateResponse(HttpStatusCode.OK, is_deletedUser);
        }
    }
}
