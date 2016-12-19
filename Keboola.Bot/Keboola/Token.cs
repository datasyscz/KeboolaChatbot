using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Keboola.Bot.Keboola
{
    [Serializable]
    public class KeboolaClient
    {
        private string baseUrl;

        public KeboolaClient(string baseUrl = "https://private-anon-805f109e37-keboola.apiary-mock.com/")
        {
            this.baseUrl = baseUrl;
        }

        public async Task<string> RefreshTokenAsync(string token)
        {
            var baseAddress = new Uri(baseUrl);
            using (var httpClient = new HttpClient {BaseAddress = baseAddress})
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("x-storageapi-token", token);
                using (var content = new StringContent(""))
                {
                    using (var response = await httpClient.PostAsync("v2/storage/tokens/id_token/refresh", content))
                    {
                        string responseData = await response.Content.ReadAsStringAsync();
                        return responseData;
                    }
                }
            }
            return null;
        }
    }

    public static class Responses
    {
        public class RefreshTokenResponse
        {
            public string id { get; set; }
            public string token { get; set; }
            public string description { get; set; }
            public string uri { get; set; }
            public bool isMasterToken { get; set; }
            public Bucketpermissions bucketPermissions { get; set; }
        }

        public class Bucketpermissions
        {
            public string incmain { get; set; }
            public string outcmain { get; set; }
            public string inctwitter { get; set; }
        }

    }
}