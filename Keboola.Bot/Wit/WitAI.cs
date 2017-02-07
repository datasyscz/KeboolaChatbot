using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AI;

namespace API
{
    public interface IWitAI
    {
        Task<WitModels.Response> Analyze(string message);
    }

    public class WitAI : IWitAI
    {
        private readonly HttpClient client = new HttpClient();

        public WitAI(string token, string accept)
        {
            // client.BaseAddress = new Uri("https://api.wit.ai");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(accept));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<WitModels.Response> Analyze(string message)
        {
            WitModels.Response entities = null;
            var res = await client.GetAsync($"https://api.wit.ai/message?q={message}");
            if (res.IsSuccessStatusCode)
            {
                entities = await res.Content.ReadAsAsync<WitModels.Response>();
            }
            return entities;
        }
    }
}