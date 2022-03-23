using TAPPT.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAPPT.Web.ViewComponents
{
    public class MeterDetailsViewComponent : ViewComponent
    {
        private readonly IConfiguration _configuration;
        public MeterDetailsViewComponent(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            IEnumerable<WSPGetMeter> model;
            List<WSPGetMeter> emptymodel = new List<WSPGetMeter>();
            var baseUrl = _configuration["ApiBaseUrl"];
            var url = baseUrl + $"wsp/QueryMeterWSP/{id}";

            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("Content-Type", "application/json");
            var response = await client.ExecuteTaskAsync(request);
            if (response.IsSuccessful)
            {
                var res = response.Content;
                IEnumerable<WSPGetMeter> objectResponse = JsonConvert.DeserializeObject<IEnumerable<WSPGetMeter>>(res);
                model = objectResponse;

                return View(model);
            }
            else
            {
                model = emptymodel;
                return View(model);
            }
        }
    }
}