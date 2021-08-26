using MakeupApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MakeupApi.Controllers
{
    public class UserController : ApiController
    {

        // Intanciação de Listas p/ Testes
        List<User> users = new List<User>(new User[] {
            new User
            {
                Id = 2,
                Name = "Guilherme",
                Last_name = "Colifeu",
                Nickname = "gui_colifeu",
                Login = "loginGui",
                Password = "senhaGui"
            },
            new User("Gabriel", "Ramos", "gabriel_ramos", "loginGabriel", "senhaGabriel"),
            new User("Isabela", "Santos", "isabela_santos", "loginIsa", "senhaIsa")
        });

        [HttpGet]
        [ActionName("JsonWebToken")]
        public HttpResponseMessage Teste(string name, int id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, 
                new JsonWebToken().generateToken(name,id));

        }

        // GET: api/User/users ---> Lista todos os Usuarios
        [ActionName("list_user")]
        [Route("~/api/user")]
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK, users);
        }

        // OKAY ---> FUNCIONANDO
        // GET: api/User/getbyid/ ---> Obtem um Usuario pelo ID
        [HttpGet]
        [ActionName("getbyid")]
        public HttpResponseMessage Get(int id)
        {
            var userId = users.FirstOrDefault((user) => user.Id == id);

            if (userId == null)
            {
                return Request.CreateErrorResponse(
                    HttpStatusCode.NotFound, string.Format("Usuario do ID = {0} não Encontrado", id));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, userId);
            }
        }



        // OKAY ---> Implementado
        // POST: api/User/exist_login ---> Busca um Login no Banco de Dados
        [HttpPost]
        [ActionName("check_login")]
        public HttpResponseMessage PostLogin([FromBody] User loginPost)
        {

            if (loginPost == null || loginPost.Login == null || loginPost.Login.Equals("")
                || loginPost.Password == null || loginPost.Password.Equals(""))
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                    "Dados Invalidos: Sem Parametros e/ou Querry Vazia");
            }
            else
            {
                // Busca na Lista o Primeiro Item que correponda o Login Informado
                var resultLogin = users.FirstOrDefault((u) => u.Login.Equals(loginPost.Login));

                // Pega a Posição desse Item na Lista --> Não Existe = -1
                int indexItem = users.IndexOf(resultLogin);


                if (indexItem != -1)
                {
                    if (users[indexItem].Password == loginPost.Password)
                    {
                        // Censura a Senha do Usuario e Retorna o Usuario
                        var returnAPI = new User
                        {
                            Login = resultLogin.Login,
                            IsChangedLogin = resultLogin.IsChangedLogin
                        };

                        return Request.CreateResponse(HttpStatusCode.OK, returnAPI);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                       string.Format("Senha do usuario '{0}' Incorreta", loginPost.Login));
                    }

                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                       string.Format("Usuario '{0}' não Encontrado", loginPost.Login));
                }
            }
        }

        // OKAY ---> Implementado
        // POST: api/User/insert_user ---> Insere um Usuario
        [HttpPost]
        [ActionName("insert_user")]
        public HttpResponseMessage Post([FromBody] User valuePost)
        {
            if (valuePost == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                    "Dados Invalidos: Sem Parametros e/ou Querry Vazia");
            }
            else
            {
                // Busca na Lista o Primeiro Item que correponda o Login Informado
                var resultLogin = users.FirstOrDefault((u) => u.Login.Equals(valuePost.Login));

                if (resultLogin == default)
                {
                    users.Add(valuePost);
                    return Request.CreateResponse(HttpStatusCode.Created, users);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.Conflict,
                       string.Format("Usuario '{0}' já Cadastrado", valuePost.Login));
                }
            }
        }


        // PUT: api/User/update_user ---> Atualiza um Usuario
        [HttpPut]
        [ActionName("update_user")]
        public HttpResponseMessage Put([FromBody] User valuePost)
        {
            if (valuePost == null || valuePost.Login == null || valuePost.Login.Equals("")
                  || valuePost.Password == null || valuePost.Password.Equals(""))
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                    "Dados Invalidos: Sem Parametros e/ou Querry Vazia");
            }
            else
            {
                // Busca na Lista o Primeiro Item que correponda o Login e SenhaInformados
                var resultUser = users.FirstOrDefault((u) => u.Login.Equals(valuePost.Login));

                if (resultUser != default)
                {
                    if (valuePost.Password.Equals(resultUser.Password))
                    {
                        // Passa o Posição do Item na Lista. Caso não exista, retorna -1
                        int indexUser = users.IndexOf(resultUser);
                        users[indexUser] = valuePost;
                        return Request.CreateResponse(HttpStatusCode.OK, users);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                         string.Format("Senha do usuario '{0}' Incorreta", valuePost.Login));
                    }
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.Conflict,
                       string.Format("Usuario '{0}' não Encontrado", valuePost.Login));
                }
            }
        }

        // DELETE: api/User/delete_user/{id} ---> Exclui um Usuario
        [HttpDelete]
        [ActionName("delete_user")]
        public HttpResponseMessage Delete([FromBody] User valuePost)
        {
            if (valuePost == null || valuePost.Login == null || valuePost.Login.Equals("")
                || valuePost.Password == null || valuePost.Password.Equals(""))
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                    "Dados Invalidos: Sem Parametros e/ou Querry Vazia");
            }

            // Obtem o um User pelo ID para depois obter o Index do elemento na List
            var userDelete = users.FirstOrDefault((userWhere) =>
                userWhere.Login.Equals(valuePost.Login));

            if (userDelete != default)
            {
                if (valuePost.Password.Equals(userDelete.Password))
                {
                    users.Remove(userDelete);
                    return Request.CreateResponse(HttpStatusCode.OK, users);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                     string.Format("Senha do usuario '{0}' Incorreta", valuePost.Login));
                }

            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.Conflict,
                      string.Format("Usuario '{0}' não Encontrado", valuePost.Login));
            }
        }

    }
}
