using MakeupApi.Models;
using MakeupApi.Models.DAO;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MakeupApi.Controllers
{
    public class UserController : ApiController
    {
        // GET: api/User
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/User/Usuario/{id}
        // Obtem uma Instancia de um Usuario do Banco de Dados
        [HttpGet]
        [ActionName("Usuario")]
        public HttpResponseMessage GetUser(int id)
        {
            UserDAO userDAO = new UserDAO();
            User user = userDAO.SelectUser(id);

            if(user == null)
            {
                return Request.CreateErrorResponse(
                    HttpStatusCode.NotFound, userDAO.Error_operation);
            }
            else return Request.CreateResponse(HttpStatusCode.OK, user);
        }

        // POST: api/User
        public void Post([FromBody]string value)
        {
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
