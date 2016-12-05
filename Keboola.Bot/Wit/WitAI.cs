using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Configuration;
using AI;
using Microsoft.Bot.Builder.FormFlow;
using Newtonsoft.Json;

namespace API
{
    public interface IWitAI
    {
        Task<WitModels.Response> Analyze(string message);
    }

    public class WitAI : IWitAI
    {
        static HttpClient client = new HttpClient();

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
            HttpResponseMessage res = await client.GetAsync($"https://api.wit.ai/message?q={message}");
            if (res.IsSuccessStatusCode)
            {
                var reee = await res.Content.ReadAsStringAsync();
                entities = await res.Content.ReadAsAsync<WitModels.Response>();
            }
            return entities;
        }
    }
}