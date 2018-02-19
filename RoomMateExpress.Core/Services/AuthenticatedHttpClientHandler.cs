using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RoomMateExpress.Core.Helpers;

namespace RoomMateExpress.Core.Services
{
    public class AuthenticatedHttpClientHandler : HttpClientHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", GetToken());

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }

        private static string GetToken()
        {
            return Settings.ApplicationData.AccessToken;
        }
    }
}
