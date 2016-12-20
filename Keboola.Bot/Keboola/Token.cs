using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Keboola.Bot.Service;
using Newtonsoft.Json;

namespace Keboola.Bot.Keboola
{
    public interface IKeboolaClient
    {
        Task<string> RefreshTokenAsync(string token);
    }

    [Serializable]
    public class KeboolaClient : IKeboolaClient
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
                        try
                        {
                            var obj = JsonConvert.DeserializeObject<Responses.RefreshTokenResponse>(responseData);
                            return obj.token;
                        }
                        catch (Exception ex)
                        {
                            Debug.Fail(ex.Message);
                        }
                        return null;
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