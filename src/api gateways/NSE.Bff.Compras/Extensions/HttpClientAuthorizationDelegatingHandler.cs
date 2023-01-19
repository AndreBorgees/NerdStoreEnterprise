using NSE.WebAPI.Core.User;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace NSE.Bff.Compras.Extensions
{
    public class HttpClientAuthorizationDelegatingHandler : DelegatingHandler
    {
        private readonly IAspNetUser _aspNetUser;

        public HttpClientAuthorizationDelegatingHandler(IAspNetUser aspNetUser)
        {
            _aspNetUser = aspNetUser;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage, CancellationToken cancellationToken)
        {
            var authorizationHeader = _aspNetUser.GetHttpContext().Request.Headers["Authorization"];

            if(!string.IsNullOrEmpty(authorizationHeader))
            {
                requestMessage.Headers.Add("Authorization", new List<string>() { authorizationHeader });
            }

            var token = _aspNetUser.GetUserToken();

            if(token != null)
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(requestMessage, cancellationToken);
        }
    }
}
