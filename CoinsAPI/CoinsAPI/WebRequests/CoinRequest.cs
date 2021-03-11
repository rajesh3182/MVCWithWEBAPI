using CoinsAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CoinsAPI.WebRequests
{
    public class CoinRequest : Controller
    {
        private HttpClient client;
        private string _url;

        public CoinRequest(string url)
        {
            _url = url;
            client = new HttpClient
            {
                BaseAddress = new Uri(url)
            };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<IEnumerable<Coins>> GetCoins()
        {
            string result = string.Empty;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(_url);
                HttpResponseMessage response = await client.GetAsync(_url);

                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsStringAsync();
                }
            }
            return JsonConvert.DeserializeObject<List<Coins>>(result);
        }
    }
}