using CoinsUX.Models;
using CoinsUX.WebRequests;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CoinsUX.Controllers
{
    public class CoinsController : Controller
    {
        private CoinRequest coinRequest;
        private string url;
        // GET: Coins
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetCoins()
        {
            try
            {
                url = Convert.ToString(ConfigurationManager.AppSettings["CoinUrl"]);

                coinRequest = new CoinRequest(url);
                var coinsList = await coinRequest.GetCoinsAsync();

                //pagination
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();

                //Find Order Column
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = coinsList.Count();

                //Filter
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();
                if (!string.IsNullOrEmpty(searchValue))
                {
                    coinsList = coinsList.Where(s => s.ID.Contains(searchValue) || s.Symbol.Contains(searchValue) || s.Name.Contains(searchValue));
                }

                //Sorting
                List<Coins> sortedCoins = new List<Coins>();
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    if (sortColumn.Equals("ID"))
                    {
                        if (sortColumnDir.Equals("asc"))
                        {
                           sortedCoins = coinsList.OrderBy(s => s.ID).ToList();
                        }
                        else
                        {
                            sortedCoins = coinsList.OrderByDescending(s => s.ID).ToList();
                        }
                    }

                    if (sortColumn.Equals("Symbol"))
                    {
                        if (sortColumnDir.Equals("asc"))
                        {
                            sortedCoins = coinsList.OrderBy(s => s.Symbol).ToList();
                        }
                        else
                        {
                            sortedCoins = coinsList.OrderByDescending(s => s.Symbol).ToList();
                        }
                    }

                    if (sortColumn.Equals("Name"))
                    {
                        if (sortColumnDir.Equals("asc"))
                        {
                            sortedCoins = coinsList.OrderBy(s => s.Name).ToList();
                        }
                        else
                        {
                            sortedCoins = coinsList.OrderByDescending(s => s.Name).ToList();
                        }
                    }
                }

                //Records to dispaly in Datatable based on page number and records per page
                var data = sortedCoins.Skip(skip).Take(pageSize);
                return Json(new { draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
