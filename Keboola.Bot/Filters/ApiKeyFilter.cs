using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Http.Results;

namespace Keboola.Bot.Filters
{
    public class HMACAuthenticationAttribute : Attribute, IAuthenticationFilter
    {
        public Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var req = context.Request;

            if (req.Headers.Contains("api-key"))
            {
                var apiKey = WebConfigurationManager.AppSettings["KeboolaApiKey"];
                var requestApiKey = req.Headers.GetValues("api-key").FirstOrDefault();
                if (apiKey.Equals(requestApiKey))
                {
                    var currentPrincipal = new GenericPrincipal(new GenericIdentity("Keboola"), null);
                    context.Principal = currentPrincipal;
                    return Task.FromResult(0);
                }
            }

            context.ErrorResult = new UnauthorizedResult(new AuthenticationHeaderValue[0], context.Request);
            return Task.FromResult(0);
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            // context.Result = new ResultWithChallenge(context.Result);
            return Task.FromResult(0);
        }

        public bool AllowMultiple
        {
            get { return false; }
        }

        private string[] GetAutherizationHeaderValues(string rawAuthzHeader)
        {
            var credArray = rawAuthzHeader.Split(':');

            if (credArray.Length == 4)
                return credArray;
            return null;
        }
    }

    public class ResultWithChallenge : IHttpActionResult
    {
        private readonly IHttpActionResult next;

        public ResultWithChallenge(IHttpActionResult next)
        {
            this.next = next;
        }

        public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = await next.ExecuteAsync(cancellationToken);
            return response;
        }
    }
}