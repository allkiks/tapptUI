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
    public class EditUtilityViewComponent : ViewComponent
    {
        private readonly IConfiguration _configuration;
        public EditUtilityViewComponent(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var model = new UtilityUploadSerializer();
            var baseUrl = _configuration["ApiBaseUrl"];
            var url = baseUrl + $"admin/GetUtility/{id}";

            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("Content-Type", "application/json");
            var response = await client.ExecuteTaskAsync(request);
            var res = response.Content;
            UtlitiesDownloadSerializer objectResponse = JsonConvert.DeserializeObject<UtlitiesDownloadSerializer>(res);
            model.Name = objectResponse.Name;
            model.PhysicalAddress = objectResponse.PhysicalAddress;
            model.PostalAddress = objectResponse.PostalAddress;
            model.PhoneNumber = objectResponse.PhoneNumber;
            model.Email = objectResponse.Email;
            model.County = objectResponse.County;
            model.Id = objectResponse.Id;
            model.KRAPin = objectResponse.KRAPin;
            return View(model);

        }
    }
}
