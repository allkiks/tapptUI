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
    public class AdminDashBoardViewComponent : ViewComponent
    {
        private readonly IConfiguration _configuration;
        public AdminDashBoardViewComponent(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var baseUrl = _configuration["ApiBaseUrl"];
            var localbaseUrl = _configuration["LocalApiUrl"];
            var urlString = baseUrl + "admin/GetAdminDashboard";
            Uri url = new Uri(urlString, UriKind.Absolute);
             var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("Content-Type", "application/json");
            var response = await client.ExecuteTaskAsync(request);

            var res = response.Content;
            GetAdminDashboardDownloadSerializer objectResponse = JsonConvert.DeserializeObject<GetAdminDashboardDownloadSerializer>(res);
            return View(objectResponse);

        }
    }
}
