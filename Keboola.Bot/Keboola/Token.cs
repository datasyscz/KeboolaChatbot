using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Keboola.Bot.Keboola
{
    public interface IKeboolaClient
    {
        Task<string> RefreshTokenAsync(string token, int id);
        Task<Responses.VerifyTokenResponse> VerifyTokenAsync(string token);
    }

    [Serializable]
    public class KeboolaClient : IKeboolaClient
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string baseUrl;

        public KeboolaClient(string baseUrl)
        {
            this.baseUrl = baseUrl;
        }

        public async Task<string> RefreshTokenAsync(string token, int id)
        {
            var baseAddress = new Uri(baseUrl);
            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("x-storageapi-token", token);
                using (var content = new StringContent(""))
                {
                    using (var response = await httpClient.PostAsync($"v2/storage/tokens/{id}/refresh", content))
                    {
                        string responseData = await response.Content.ReadAsStringAsync();
                        try
                        {
                            var obj = JsonConvert.DeserializeObject<Responses.RefreshTokenResponse>(responseData);
                            return obj.token;
                        }
                        catch (Exception ex)
                        {
                            logger.Error(ex);
                            Debug.Fail(ex.Message);
                        }
                    }
                }
            }
            return null;
        }


        public async Task<Responses.VerifyTokenResponse> VerifyTokenAsync(string token)
        {
            var baseAddress = new Uri(baseUrl);
            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("x-storageapi-token", token);

                using (var response = await httpClient.GetAsync("v2/storage/tokens/verify"))
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    try
                    {
                        var obj = JsonConvert.DeserializeObject<Responses.VerifyTokenResponse>(responseData);
                        if (obj.token == null)
                            return null;
                        return obj;
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex);
                        Debug.Fail(ex.Message);
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
        
        }

        public class VerifyTokenResponse
        {
            public string id { get; set; }
            public string token { get; set; }
        }
    }
}