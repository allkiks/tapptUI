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
    public class GetCustomerBillViewComponent : ViewComponent
    {
        private readonly IConfiguration _configuration;
        public GetCustomerBillViewComponent(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var baseUrl = _configuration["ApiBaseUrl"];
            var url = baseUrl + "wsp/GetCustomerBill/" + id;

            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("Content-Type", "application/json");
            var response = await client.ExecuteTaskAsync(request);
            var res = response.Content;
            IEnumerable<BillingDownloadSerializer> objectResponse = JsonConvert.DeserializeObject<IEnumerable<BillingDownloadSerializer>>(res);

            var urlString = baseUrl + "wsp/History/" + id;
            Uri url2 = new Uri(urlString, UriKind.Absolute);
            var client2 = new RestClient(url2);
            var request2 = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("Content-Type", "application/json");
            var response2 = await client2.ExecuteTaskAsync(request2);
            var res2 = response2.Content;
            IEnumerable<WSPCustomerDownloadSerializer> objectResponse2 = JsonConvert.DeserializeObject<IEnumerable<WSPCustomerDownloadSerializer>>(res2);
            ViewBag.Model = objectResponse2;
            return View(objectResponse);
        }
    }
}