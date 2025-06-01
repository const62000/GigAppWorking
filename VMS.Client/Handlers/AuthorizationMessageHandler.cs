using Blazored.LocalStorage;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace VMS.Client.Handlers
{
    public class AuthorizationMessageHandler : DelegatingHandler
    {
        private readonly ILocalStorageService _localStorage;

        public AuthorizationMessageHandler(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Retrieve the access token from local storage
            var accessToken = await _localStorage.GetItemAsync<string>("access_token");

            // If the token exists, add it to the Authorization header
            if (!string.IsNullOrEmpty(accessToken))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    } 
}