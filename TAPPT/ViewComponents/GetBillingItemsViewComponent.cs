using TAPPT.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace TAPPT.Web.ViewComponents
{
    public class GetBillingItemsViewComponent : ViewComponent
    {
        private readonly IConfiguration _configuration;
        public GetBillingItemsViewComponent(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            IEnumerable<BillingItemDownLoadSerializer> model;
            var emptymodel = new BillingItemDownLoadSerializer();
            var baseUrl = _configuration["ApiBaseUrl"];
            var url = baseUrl + "/admin/GetBillingItems";

            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("Content-Type", "application/json");
            var response = await client.ExecuteTaskAsync(request);
            if (response.IsSuccessful)
            {
                var res = response.Content;
                IEnumerable<BillingItemDownLoadSerializer> objectResponse = JsonConvert.DeserializeObject<IEnumerable<BillingItemDownLoadSerializer>>(res);
                model = objectResponse;

                return View(model);
            }
            else
            {
                return View(emptymodel);
            }
        }
    }
}
