using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TAPPT.Web.Helpers;
using TAPPT.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;

namespace TAPPT.Web.Controllers
{
    [Authorize]
    public class SettingController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public SettingController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }
        #region Billing Items
        public IActionResult BillingItems()
        {
            return View();
        }
        public IActionResult AddBillingItem()
        {
            ViewBag.UserId = 1;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBillingItem(CreateBillingItemSerializer model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(modelState: ModelState);
            }
            var postModel = new CreateBillingItemSerializer()
            {
                ItemName = model.ItemName,
                Rate = model.Rate,
                UserId = model.UserId
            };
            var baseUrl = _configuration["ApiBaseUrl"];
            var url = baseUrl + "admin/CreateBillingItem";
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            string json = JsonConvert.SerializeObject((CreateBillingItemSerializer)postModel);
            request.AddJsonBody(json);
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("Accept", "application/json");
            request.AddParameter("Content-Type", "application/json");
            var response = await client.ExecuteTaskAsync(request);
            if (response.IsSuccessful)
            {
                var res = response.Content;
                ViewBag.UserId = model.UserId;
                ModelState.Clear();

                var postModel2 = new AuditLogUploadViewModel()
                {
                    CreatedBy = User.FindFirst("DisplayName")?.Value,
                    Description = $"Created Billing Item {model.ItemName}"
                };

                var url2 = baseUrl + "admin/PostAuditLog";
                var client2 = new RestClient(url2);
                var request2 = new RestRequest(Method.POST);
                string json2 = JsonConvert.SerializeObject((AuditLogUploadViewModel)postModel2);
                request2.AddJsonBody(json2);
                request2.AddHeader("cache-control", "no-cache");
                request2.AddParameter("Accept", "application/json");
                request2.AddParameter("Content-Type", "application/json");
                await client2.ExecuteTaskAsync(request2);

                Alert(res, NotificationType.success, 5000);
                return View();
            }
            else
            {
                var res = response.Content;
                ViewBag.UserId = model.UserId;
                Alert(res, NotificationType.error, 5000);
                return View();
            }
        }
        #endregion
        #region Tariffs
        public IActionResult Tariffs()
        {
            return View();
        }
        public IActionResult AddTariff()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTariff(TariffUploadSerializer model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(modelState: ModelState);
            }
            var postModel = new TariffUploadSerializer()
            {
                Name = model.Name
            };
            var baseUrl = _configuration["ApiBaseUrl"];
            var url = baseUrl + "admin/CreateTariff";
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            string json = JsonConvert.SerializeObject((TariffUploadSerializer)postModel);
            request.AddJsonBody(json);
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("Accept", "application/json");
            request.AddParameter("Content-Type", "application/json");
            var response = await client.ExecuteTaskAsync(request);
            if (response.IsSuccessful)
            {
                var res = response.Content;
                ModelState.Clear();
                Alert(res, NotificationType.success, 5000);

                var postModel2 = new AuditLogUploadViewModel()
                {
                    CreatedBy = User.FindFirst("DisplayName")?.Value,
                    Description = $"Created User Tariff {model.Name}"
                };

                var url2 = baseUrl + "admin/PostAuditLog";
                var client2 = new RestClient(url2);
                var request2 = new RestRequest(Method.POST);
                string json2 = JsonConvert.SerializeObject((AuditLogUploadViewModel)postModel2);
                request2.AddJsonBody(json2);
                request2.AddHeader("cache-control", "no-cache");
                request2.AddParameter("Accept", "application/json");
                request2.AddParameter("Content-Type", "application/json");
                await client2.ExecuteTaskAsync(request2);
                return View();
            }
            else
            {
                var res = response.Content;
                Alert(res, NotificationType.error, 5000); ;
                return View();
            }
        }
        public IActionResult EditTariff(int id)
        {
            ViewBag.Id = id;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTariff(TariffUploadSerializer model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(modelState: ModelState);
            }
            var postModel = new TariffUploadSerializer()
            {
                Name = model.Name
            };
            var baseUrl = _configuration["ApiBaseUrl"];
            var url = baseUrl + $"admin/UpdateTariff/{model.Id}";
            var client = new RestClient(url);
            var request = new RestRequest(Method.PATCH);
            string json = JsonConvert.SerializeObject((TariffUploadSerializer)postModel);
            request.AddJsonBody(json);
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("Accept", "application/json");
            request.AddParameter("Content-Type", "application/json");
            var response = await client.ExecuteTaskAsync(request);
            if (response.IsSuccessful)
            {
                var res = response.Content;
                ViewBag.Id = model.Id;
                ModelState.Clear();
                Alert(res, NotificationType.success, 5000);
                var postModel2 = new AuditLogUploadViewModel()
                {
                    CreatedBy = User.FindFirst("DisplayName")?.Value,
                    Description = $"Edit tariff to {model.Name}"
                };

                var url2 = baseUrl + "admin/PostAuditLog";
                var client2 = new RestClient(url2);
                var request2 = new RestRequest(Method.POST);
                string json2 = JsonConvert.SerializeObject((AuditLogUploadViewModel)postModel2);
                request2.AddJsonBody(json2);
                request2.AddHeader("cache-control", "no-cache");
                request2.AddParameter("Accept", "application/json");
                request2.AddParameter("Content-Type", "application/json");
                await client2.ExecuteTaskAsync(request2);
                return View();
            }
            else
            {
                var res = response.Content;
                ViewBag.Id = model.Id;
                Alert(res, NotificationType.error, 5000); ;
                return View();
            }
        }
        public IActionResult DeleteTariff(int id)
        {
            ViewBag.Id = id;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTariff(TariffUploadSerializer model)
        {
            var baseUrl = _configuration["ApiBaseUrl"];
            var url = baseUrl + $"admin/DeleteTariff/{model.Id}";
            var client = new RestClient(url);
            var request = new RestRequest(Method.DELETE);
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("Accept", "application/json");
            request.AddParameter("Content-Type", "application/json");
            var response = await client.ExecuteTaskAsync(request);
            if (response.IsSuccessful)
            {
                var res = response.Content;
                ViewBag.Id = model.Id;
                ModelState.Clear();
                Alert(res, NotificationType.success, 5000);

                var postModel2 = new AuditLogUploadViewModel()
                {
                    CreatedBy = User.FindFirst("DisplayName")?.Value,
                    Description = $"Delete tariff"
                };

                var url2 = baseUrl + "admin/PostAuditLog";
                var client2 = new RestClient(url2);
                var request2 = new RestRequest(Method.POST);
                string json2 = JsonConvert.SerializeObject((AuditLogUploadViewModel)postModel2);
                request2.AddJsonBody(json2);
                request2.AddHeader("cache-control", "no-cache");
                request2.AddParameter("Accept", "application/json");
                request2.AddParameter("Content-Type", "application/json");
                await client2.ExecuteTaskAsync(request2);
                return View();
            }
            else
            {
                var res = response.Content;
                ViewBag.Id = model.Id;
                Alert(res, NotificationType.error, 5000); ;
                return View();
            }
        }
        #endregion
        #region WSP Utilities
        public IActionResult Utilities()
        {
            return View();
        }

        public IActionResult DeleteUtility()
        {
            return View();
        }
        public IActionResult AddUtility()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUtility([FromForm]UtilityUploadSerializer model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(modelState: ModelState);
            }
            var logofile = model.LogoFile;
            var logofileName = $"{DateTime.Now.ToString("yyyyMMddHHmmss")}{logofile.FileName}";
            var logoFileLocation = $"~/logos/{logofileName}";
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\logos", logofileName);
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await logofile.CopyToAsync(fileStream);
            }
            
            var postModel = new UtilityUploadSerializer()
            {
                Name = model.Name,
                PhysicalAddress = model.PhysicalAddress,
                PostalAddress = model.PostalAddress,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                County = model.County,
                KRAPin = model.KRAPin,
                LogoUrl = logoFileLocation,
                BouquetId = model.BouquetId
            };
            var baseUrl = _configuration["ApiBaseUrl"];
            var url = baseUrl + "admin/CreateUtility";
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            string json = JsonConvert.SerializeObject((UtilityUploadSerializer)postModel);
            request.AddJsonBody(json);
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("Accept", "application/json");
            request.AddParameter("Content-Type", "application/json");
            var response = await client.ExecuteTaskAsync(request);
            if (response.IsSuccessful)
            {
                var res = response.Content;
                ModelState.Clear();
                Alert(res, NotificationType.success, 5000);

                var postModel2 = new AuditLogUploadViewModel()
                {
                    CreatedBy = User.FindFirst("DisplayName")?.Value,
                    Description = $"Added utility {model.Name}"
                };

                var url2 = baseUrl + "admin/PostAuditLog";
                var client2 = new RestClient(url2);
                var request2 = new RestRequest(Method.POST);
                string json2 = JsonConvert.SerializeObject((AuditLogUploadViewModel)postModel2);
                request2.AddJsonBody(json2);
                request2.AddHeader("cache-control", "no-cache");
                request2.AddParameter("Accept", "application/json");
                request2.AddParameter("Content-Type", "application/json");
                await client2.ExecuteTaskAsync(request2);
                return View();
            }
            else
            {
                var res = response.Content;
                Alert(res, NotificationType.error, 5000); ;
                return View();
            }
        }
        public IActionResult EditUtility(int id)
        {
            ViewBag.Id = id;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUtility(UtilityUploadSerializer model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(modelState: ModelState);
            }
            var postModel = new UtilityUploadSerializer()
            {
                Name = model.Name,
                PhysicalAddress = model.PhysicalAddress,
                PostalAddress = model.PostalAddress,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                County = model.County,
                KRAPin = model.KRAPin
            };
            var baseUrl = _configuration["ApiBaseUrl"];
            var url = baseUrl + $"admin/UpdateUtility/{model.Id}";
            var client = new RestClient(url);
            var request = new RestRequest(Method.PATCH);
            string json = JsonConvert.SerializeObject((UtilityUploadSerializer)postModel);
            request.AddJsonBody(json);
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("Accept", "application/json");
            request.AddParameter("Content-Type", "application/json");
            var response = await client.ExecuteTaskAsync(request);
            if (response.IsSuccessful)
            {
                var res = response.Content;
                ViewBag.Id = model.Id;
                ModelState.Clear();
                Alert(res, NotificationType.success, 5000);
                var postModel2 = new AuditLogUploadViewModel()
                {
                    CreatedBy = User.FindFirst("DisplayName")?.Value,
                    Description = $"Edit utility {model.Name}"
                };

                var url2 = baseUrl + "admin/PostAuditLog";
                var client2 = new RestClient(url2);
                var request2 = new RestRequest(Method.POST);
                string json2 = JsonConvert.SerializeObject((AuditLogUploadViewModel)postModel2);
                request2.AddJsonBody(json2);
                request2.AddHeader("cache-control", "no-cache");
                request2.AddParameter("Accept", "application/json");
                request2.AddParameter("Content-Type", "application/json");
                await client2.ExecuteTaskAsync(request2);
                return View();
            }
            else
            {
                var res = response.Content;
                ViewBag.Id = model.Id;
                ModelState.Clear();
                Alert(res, NotificationType.error, 5000); ;
                return View();
            }
        }
        #endregion
    }
}