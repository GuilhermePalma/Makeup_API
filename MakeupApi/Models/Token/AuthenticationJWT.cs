using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Http.Results;

namespace MakeupApi.Models.Token
{
    public class AuthenticationJWT : AuthorizeAttribute, IAuthenticationFilter
    {
        public bool AllowMultiple
        {
            get { return false; }
        }

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            // Obtem o Header da Solicitação
            HttpRequestMessage request = context.Request;
            AuthenticationHeaderValue authorizationHeader = request.Headers.Authorization;

            // Verifica a Autorização do Header é Valida
            if(authorizationHeader == null)
            {
                context.ErrorResult =
                     new AuthenticationFailureResult("Header não Encontrado ou Invalido", request);
                return;
            }

            if(authorizationHeader.Scheme != "Bearer")
            {
                context.ErrorResult =
                     new AuthenticationFailureResult("Esquema de Autenticação Invalido", request);
                return;
            }

            // Obtem e Valida os Parametros passado  Header
            string authenticationPatameter = authorizationHeader.Parameter;
            if (string.IsNullOrEmpty(authenticationPatameter))
            {
                context.ErrorResult =
                     new AuthenticationFailureResult("Token não Encontrado", request);
                return;
            }

            // Valida se as informações do Token são Validas
            if (!HandlerJWT.validationToken(authenticationPatameter))
            {
                context.ErrorResult =
                     new AuthenticationFailureResult("Autenticação do Token Invalido", request);
                return;
            }

            // Finaliza Validando Novamente se o Token (JWT) é valido
            context.Principal = HandlerJWT.GetPrincipal(authenticationPatameter);
        }

        public async Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            var result = await context.Result.ExecuteAsync(cancellationToken);
            context.Result = new ResponseMessageResult(result);
        }
    }

    // Retorna o Erro (Unauthorized)
    public class AuthenticationFailureResult : IHttpActionResult
    {
        // Construtor da Classe ---> Instancia os Itens
        public AuthenticationFailureResult(string reasonError, HttpRequestMessage requestMesage)
        {
            Error_operation = reasonError;
            RequestMessageError = requestMesage;
        }

        // Get e Setter de Variaveis Usadas no Retorno
        public string Error_operation { get; set; }
        public HttpRequestMessage RequestMessageError { get; set; }

        // Metodo Herdado do 'IHttpActionResult'
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(ReturnErrorUnauthorized());
        }

        // Retorna o Erro de Header não Autorizado
        public HttpResponseMessage ReturnErrorUnauthorized()
        {
            return RequestMessageError.CreateErrorResponse(
                System.Net.HttpStatusCode.Unauthorized, Error_operation, 
                new Exception(RequestMessageError.RequestUri.ToString() +
                " Header:" + RequestMessageError.Headers.ToString()));
        }

    }
}