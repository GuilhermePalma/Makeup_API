using MakeupApi.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Results;

namespace MakeupApi.Controllers
{
    public class UserController : ApiController
    {

        List<User> users = new List<User>(new User[] {
            new User(1, "Guilherme", "Colifeu", "gui_colifeu", "loginGui", "senhaGui"),
            new User(2, "Gabriel", "Ramos", "gabriel_ramos", "loginGabriel", "senhaGabriel"),
            new User(3, "Isabela", "Santos", "isabela_santos", "loginIsa", "senhaIsa")
        });

        List<User> userLogin = new List<User>()
        {
            new User("login1", "senha123"),
            new User("login2", "senha456"),
            new User("login3", "senha789")
        };

        // GET: api/User
        [ActionName("GetAllUsers")]
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.Found, users);
        }

        // GET: api/User/{id}
        [HttpGet]
        [ActionName("GetUserById")]
        public HttpResponseMessage Get(int id)
        {
            var userId = users.FirstOrDefault((user) => user.Id == id);

            if (userId == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound,string.Format("Usuario do ID = {0} não Encontrado", id));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.Found, userId);
            }   
        }

        // POST: api/User
        [HttpPost]
        [ActionName("InsertUser")]
        public HttpResponseMessage Post([FromBody]List<User> value)
        {
        /*    User user = new User()
            {
                Name = value.Name,
                Last_name = value.Last_name,
                Nickname = value.Nickname,
                Login = value.Login,
                Password = value.Password
            };*/

            /*if (value == null || !value.avalaibleUser())
            {
                // Dados Vazios ou Nulos
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Dados Invalidos: Sem Parametros e/ou Querry Vazia");
            }
            else
            {
                
                // Adiciona o Elemento no Final da Lista
                users.Add(new User(value.Id, value.Name, value.Last_name, value.Nickname, value.Login, value.Password));
                var response = Request.CreateResponse(HttpStatusCode.Created, value);
                string uri = Url.Link("DefaultApi", new { id = value.Id });
                response.Headers.Location = new Uri(uri);
                return response;
            }*/

            if (value == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Dados Invalidos: Sem Parametros e/ou Querry Vazia");
            }
            else
            {
                users.AddRange(value);
                return Request.CreateResponse(HttpStatusCode.Created, users);
            }

            

        }

        // PUT: api/User/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/User/5
        public void Delete(int id)
        {
        }
    }
}
