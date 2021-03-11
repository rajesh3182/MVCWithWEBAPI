using CoinsAPI.Models;
using CoinsAPI.WebRequests;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Http;

namespace CoinsAPI.Controllers
{
    public class CoinController : ApiController
    {
        private CoinRequest coinRequest;
        private string url;
        // GET: api/Coin

        public async Task<IEnumerable<Coins>> GetAsync()
        {
            url = Convert.ToString(ConfigurationManager.AppSettings["CoinUrl"]);

            coinRequest = new CoinRequest(url);
            return await coinRequest.GetCoins();
        }
    }
}
