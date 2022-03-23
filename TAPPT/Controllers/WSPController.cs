using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAPPT.Web.Helpers;
using TAPPT.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;

namespace TAPPT.Web.Controllers
{
    [Authorize]
    public class WSPController : BaseController
    {
        private readonly IConfiguration _configuration;
        public WSPController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #region Zones
        public async Task<IActionResult> Zones()
        {
            var wspId = Convert.ToInt32(User.FindFirst("WSPId")?.Value);
            var baseUrl = _configuration["ApiBaseUrl"];
            var localbaseUrl = _configuration["LocalApiUrl"];
            var urlString = baseUrl + "wsp/zones/" + wspId;
            Uri url = new Uri(urlString, UriKind.Absolute);
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("Content-Type", "application/json");
            var response = await client.ExecuteTaskAsync(request);
            var res = response.Content;
            IEnumerable<ZoneViewModel> objectResponse = JsonConvert.DeserializeObject<IEnumerable<ZoneViewModel>>(res);
            return View(objectResponse);
        }
        public IActionResult AddZone()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddZone([FromForm]ZoneViewModel model)
        {
            var wspId = Convert.ToInt32(User.FindFirst("WSPId")?.Value);
            var zone = new ZoneViewModel()
            {
                Name = model.Name,
                WSPUtilityId = wspId
            };
            var baseUrl = _configuration["ApiBaseUrl"];
            var localbaseUrl = _configuration["LocalApiUrl"];
            var urlString = baseUrl + "WSP/CreateZone";
            Uri url = new Uri(urlString, UriKind.Absolute);
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.Parameters.Clear();
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("Accept", "application/json");
            request.AddParameter("Content-Type", "application/json");
            string json = JsonConvert.SerializeObject((ZoneViewModel)zone);
            request.AddParameter("application/json", json, ParameterType.RequestBody);
            var response = await client.ExecuteTaskAsync(request);
            return RedirectToAction("Zones");

        }
        public async Task<IActionResult> EditZone(int id)
        {
            var baseUrl = _configuration["ApiBaseUrl"];
            var localbaseUrl = _configuration["LocalApiUrl"];
            var urlString = baseUrl + "wsp/GetZone/" + id;
            Uri url = new Uri(urlString, UriKind.Absolute);
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("Content-Type", "application/json");
            var response = await client.ExecuteTaskAsync(request);
            var res = response.Content;
            IEnumerable<ZoneViewModel> objectResponse = JsonConvert.DeserializeObject<IEnumerable<ZoneViewModel>>(res);
            return View(objectResponse);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditZone([FromForm] string Name, int Id)
        {
            var zone = new ZoneViewModel()
            {
                Name = Name,
                Id = Id
            };
            var baseUrl = _configuration["ApiBaseUrl"];
            var localbaseUrl = _configuration["LocalApiUrl"];
            var urlString = baseUrl + "WSP/EditZone";
            Uri url = new Uri(urlString, UriKind.Absolute);
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.Parameters.Clear();
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("Accept", "application/json");
            request.AddParameter("Content-Type", "application/json");
            string json = JsonConvert.SerializeObject((ZoneViewModel)zone);
            request.AddParameter("application/json", json, ParameterType.RequestBody);
            var response = await client.ExecuteTaskAsync(request);
            return RedirectToAction("Zones");

        }
        #endregion
        #region Meters
        public IActionResult Meters(IEnumerable<MeterDownloadSerializer> meter)
        {

            IEnumerable<MeterDownloadSerializer> model = meter;
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Meters([FromForm]string ssearchTerm)
        {

            if (ssearchTerm != null)
            {
                var wspId = Convert.ToInt32(User.FindFirst("WSPId")?.Value);
                var postModel = new QueryableMetersSerializer()
                {
                    SearchTerm = ssearchTerm,
                    Id = wspId
                };

                var baseUrl = _configuration["ApiBaseUrl"];
                var localbaseUrl = _configuration["LocalApiUrl"];
                var urlString = baseUrl + "WSP/QueryableMeter";
                Uri url = new Uri(urlString, UriKind.Absolute);
                var client = new RestClient(url);
                var request = new RestRequest(Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.Parameters.Clear();
                request.AddHeader("cache-control", "no-cache");
                request.AddParameter("Accept", "application/json");
                request.AddParameter("Content-Type", "application/json");
                string json = JsonConvert.SerializeObject((QueryableMetersSerializer)postModel);
                request.AddParameter("application/json", json, ParameterType.RequestBody);
                var response = await client.ExecuteTaskAsync(request);

                var res = response.Content;
                IEnumerable<MeterDownloadSerializer> objectResponse = JsonConvert.DeserializeObject<IEnumerable<MeterDownloadSerializer>>(res);
                IEnumerable<MeterDownloadSerializer> model;
                model = objectResponse;
                return View(model);
            }
            else
            {
                IEnumerable<MeterDownloadSerializer> model = new List<MeterDownloadSerializer>();
                return View(model);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMeters(MeterUploadSerializer model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(modelState: ModelState);
            }
            var baseUrl = _configuration["ApiBaseUrl"];
            var url = baseUrl + $"WSP/UpdateMeter/{model.MeterId}";
            var client = new RestClient(url);
            var request = new RestRequest(Method.PATCH);
            string json = JsonConvert.SerializeObject((MeterUploadSerializer)model);
            request.AddJsonBody(json);
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("Accept", "application/json");
            request.AddParameter("Content-Type", "application/json");
            var response = await client.ExecuteTaskAsync(request);
            var res = response.Content;
            ViewBag.SearchTerm = model.SearchTerm;
            Alert(res, NotificationType.success, 5000);
            return View(new { searchTerm = model.SearchTerm });
        }
        public async Task<IActionResult> MetersByZone(int id)
        {
            var wspId = Convert.ToInt32(User.FindFirst("WSPId")?.Value);
            if (id > 0)
            {
                ViewBag.Id = wspId;
                var postModel = new MeterByZoneViewModel()
                {
                    ZoneId = id,
                    WSPId = wspId
                };
                var baseUrl = _configuration["ApiBaseUrl"];
                var localbaseUrl = _configuration["LocalApiUrl"];
                var urlString = baseUrl + "WSP/MeterByZone";
                Uri url = new Uri(urlString, UriKind.Absolute);
                var client = new RestClient(url);
                var request = new RestRequest(Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.Parameters.Clear();
                request.AddHeader("cache-control", "no-cache");
                request.AddParameter("Accept", "application/json");
                request.AddParameter("Content-Type", "application/json");
                string json = JsonConvert.SerializeObject((MeterByZoneViewModel)postModel);
                request.AddParameter("application/json", json, ParameterType.RequestBody);
                var response = await client.ExecuteTaskAsync(request);
                IEnumerable<MeterDownloadSerializer> objectResponse = JsonConvert.DeserializeObject<IEnumerable<MeterDownloadSerializer>>(response.Content);
                return View(objectResponse);
            }
            else
            {
                ViewBag.Id = wspId;
            }
            IEnumerable<MeterDownloadSerializer> objectResponse2;
            List<MeterDownloadSerializer> model2 = new List<MeterDownloadSerializer>();
            objectResponse2 = model2;
            return View(objectResponse2);
        }
        public async Task<IActionResult> ViewTelemetry(int id)
        {
            var baseUrl = _configuration["ApiBaseUrl"];
            var localbaseUrl = _configuration["LocalApiUrl"];
            var urlString = baseUrl + "wsp/QueryableMeter/" + id;
            Uri url = new Uri(urlString, UriKind.Absolute);
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("Content-Type", "application/json");
            var response = await client.ExecuteTaskAsync(request);
            var res = response.Content;
            IEnumerable<MeterDownloadSerializer> objectResponse = JsonConvert.DeserializeObject<IEnumerable<MeterDownloadSerializer>>(res);
            return View(objectResponse);
        }

        #endregion
        #region Transmissions
        public IActionResult Transmitting()
        {
            return View();
        }
        public IActionResult TransmittingList(int id)
        {
            ViewBag.Id = id;
            return View();
        }
        public IActionResult TrasmittingMeter(int id)
        {
            ViewBag.Id = id;
            return View();
        }
        public IActionResult NotTransmitting()
        {
            return View();
        }
        public IActionResult NotTransmittingList(int id)
        {
            ViewBag.Id = id;
            return View();
        }
        #endregion
        #region Customers
        public IActionResult Customers(IEnumerable<WSPCustomerDownloadSerializer> customer)
        {
            IEnumerable<WSPCustomerDownloadSerializer> model = customer;
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Customers([FromForm]string searchTerm)
        {

            if (searchTerm != null)
            {
                var wspId = Convert.ToInt32(User.FindFirst("WSPId")?.Value);
                var postModel = new QueryableMetersSerializer()
                {
                    SearchTerm = searchTerm,
                    Id = wspId
                };

                var baseUrl = _configuration["ApiBaseUrl"];
                var localbaseUrl = _configuration["LocalApiUrl"];
                var urlString = baseUrl + "WSP/SearchAccount";
                Uri url = new Uri(urlString, UriKind.Absolute);
                var client = new RestClient(url);
                var request = new RestRequest(Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.Parameters.Clear();
                request.AddHeader("cache-control", "no-cache");
                request.AddParameter("Accept", "application/json");
                request.AddParameter("Content-Type", "application/json");
                string json = JsonConvert.SerializeObject((QueryableMetersSerializer)postModel);
                request.AddParameter("application/json", json, ParameterType.RequestBody);
                var response = await client.ExecuteTaskAsync(request);

                var res = response.Content;
                IEnumerable<WSPCustomerDownloadSerializer> objectResponse = JsonConvert.DeserializeObject<IEnumerable<WSPCustomerDownloadSerializer>>(res);
                IEnumerable<WSPCustomerDownloadSerializer> model;
                model = objectResponse;
                return View(model);
            }
            else
            {
                IEnumerable<WSPCustomerDownloadSerializer> model = new List<WSPCustomerDownloadSerializer>();
                return View(model);
            }
        }
        public async Task<IActionResult> CustomersByZone(int id)
        {
            var wspId = Convert.ToInt32(User.FindFirst("WSPId")?.Value);
            if (id > 0)
            {
                ViewBag.Id = wspId;
                var postModel = new MeterByZoneViewModel()
                {
                    ZoneId = id,
                    WSPId = wspId
                };
                var baseUrl = _configuration["ApiBaseUrl"];
                var localbaseUrl = _configuration["LocalApiUrl"];
                var urlString = baseUrl + "WSP/CustomersByZone";
                Uri url = new Uri(urlString, UriKind.Absolute);
                var client = new RestClient(url);
                var request = new RestRequest(Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.Parameters.Clear();
                request.AddHeader("cache-control", "no-cache");
                request.AddParameter("Accept", "application/json");
                request.AddParameter("Content-Type", "application/json");
                string json = JsonConvert.SerializeObject((MeterByZoneViewModel)postModel);
                request.AddParameter("application/json", json, ParameterType.RequestBody);
                var response = await client.ExecuteTaskAsync(request);
                IEnumerable<WSPCustomerDownloadSerializer> objectResponse = JsonConvert.DeserializeObject<IEnumerable<WSPCustomerDownloadSerializer>>(response.Content);
                return View(objectResponse);
            }
            else
            {
                ViewBag.Id = wspId;
            }
            IEnumerable<WSPCustomerDownloadSerializer> objectResponse2;
            List<WSPCustomerDownloadSerializer> model2 = new List<WSPCustomerDownloadSerializer>();
            objectResponse2 = model2;
            return View(objectResponse2);
        }
        public IActionResult GetBill(int id)
        {
            ViewBag.Id = id;
            return View();
        }
        public async Task<IActionResult> BillHistory(int id)
        {
            var baseUrl = _configuration["ApiBaseUrl"];
            var localbaseUrl = _configuration["LocalApiUrl"];
            var urlString = baseUrl + "wsp/History/" + id;
            Uri url = new Uri(urlString, UriKind.Absolute);
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("Content-Type", "application/json");
            var response = await client.ExecuteTaskAsync(request);
            var res = response.Content;
            IEnumerable<WSPCustomerDownloadSerializer> objectResponse = JsonConvert.DeserializeObject<IEnumerable<WSPCustomerDownloadSerializer>>(res);
            return View(objectResponse);
        }
        #endregion
        #region WSP Tariff
        public IActionResult Tariffs()
        {
            return View();
        }
        public IActionResult AddTariffRate(int id)
        {
            ViewBag.TariffId = id;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTariffRate(AddtariffRateUploadSerializer model)
        {
            var wspId = Convert.ToInt32(User.FindFirst("WSPId")?.Value);
            var postModel = new AddtariffRateUploadSerializer()
            {
                WSPUtilityId = wspId,
                TariffId = model.TariffId,
                MinimumFlow = model.MinimumFlow,
                MaximumFlow = model.MaximumFlow,
                Rate = model.Rate
            };
            var baseUrl = _configuration["ApiBaseUrl"];
            var url = baseUrl + "wsp/CreateTarriffRate";
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            string json = JsonConvert.SerializeObject((AddtariffRateUploadSerializer)postModel);
            request.AddJsonBody(json);
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("Accept", "application/json");
            request.AddParameter("Content-Type", "application/json");
            var response = await client.ExecuteTaskAsync(request);
            if (response.IsSuccessful)
            {
                Alert(response.Content, NotificationType.success, 5000);
                return RedirectToAction("Tariffs", "WSP");
            }
            else
            {
                Alert(response.Content, NotificationType.error, 5000);
                return RedirectToAction("Tariffs", "WSP");
            }

        }
        #endregion
        #region Query Meters
        public IActionResult QueryMeters(IEnumerable<QueriedMetersDownloadSerializer> model)
        {
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> QueryMeters([FromForm]QueryMetersViewModel model)
        {
            var wspId = Convert.ToInt32(User.FindFirst("WSPId")?.Value);
            var postModel = new QueryMetersViewModel()
            {
                WSPId = wspId,
                QueryStatus = model.QueryStatus,
                StartDate = model.StartDate,
                EndDate = model.EndDate
            };
            var baseUrl = _configuration["ApiBaseUrl"];
            var url = baseUrl + "wsp/QueryMeters";
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            string json = JsonConvert.SerializeObject((QueryMetersViewModel)postModel);
            request.AddJsonBody(json);
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("Accept", "application/json");
            request.AddParameter("Content-Type", "application/json");
            var response = await client.ExecuteTaskAsync(request);
            if (response.IsSuccessful)
            {
                var res = response.Content;
                IEnumerable<QueriedMetersDownloadSerializer> objectResponse = JsonConvert.DeserializeObject<IEnumerable<QueriedMetersDownloadSerializer>>(res);
                Alert(response.Content, NotificationType.success, 5000);
                return View(objectResponse);
            }
            else
            {
                List<QueriedMetersDownloadSerializer> emptyList = new List<QueriedMetersDownloadSerializer>();
                IEnumerable<QueriedMetersDownloadSerializer> returnModel = emptyList;
                Alert(response.Content, NotificationType.error, 5000);
                return View(returnModel);
            }
        }
        [HttpGet]
        public IActionResult MeterDetails(int id)
        {
            ViewBag.Id = id;
            return View();
        }
        #endregion
        #region Query Consumption
        public IActionResult QueryConsumption(IEnumerable<QueryConsumptionDownloadSerializer> model)
        {
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> QueryConsumption([FromForm]QueryMetersViewModel model)
        {
            var wspId = Convert.ToInt32(User.FindFirst("WSPId")?.Value);
            var postModel = new QueryMetersViewModel()
            {
                WSPId = wspId,
                QueryStatus = model.QueryStatus,
                StartDate = model.StartDate,
                EndDate = model.EndDate
            };
            var baseUrl = _configuration["ApiBaseUrl"];
            var url = baseUrl + "wsp/QueryConsumption";
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            string json = JsonConvert.SerializeObject((QueryMetersViewModel)postModel);
            request.AddJsonBody(json);
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("Accept", "application/json");
            request.AddParameter("Content-Type", "application/json");
            var response = await client.ExecuteTaskAsync(request);

            var res = response.Content;
            IEnumerable<QueryConsumptionDownloadSerializer> objectResponse = JsonConvert.DeserializeObject<IEnumerable<QueryConsumptionDownloadSerializer>>(res);
            Alert(response.Content, NotificationType.success, 5000);
            return View(objectResponse);
        }
        #endregion
        #region Billing
        public async Task<IActionResult> BillingByZone(int id)
        {
            IEnumerable<BillingDownloadSerializer> objectResponse2;
            List<BillingDownloadSerializer> model2 = new List<BillingDownloadSerializer>();
            var wspId = Convert.ToInt32(User.FindFirst("WSPId")?.Value);
            if (id > 0)
            {
                ViewBag.Id = wspId;
                var postModel = new MeterByZoneViewModel()
                {
                    ZoneId = id,
                    WSPId = wspId
                };
                var baseUrl = _configuration["ApiBaseUrl"];
                var localbaseUrl = _configuration["LocalApiUrl"];
                var urlString = baseUrl + "WSP/BillsByzone";
                Uri url = new Uri(urlString, UriKind.Absolute);
                var client = new RestClient(url);
                var request = new RestRequest(Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.Parameters.Clear();
                request.AddHeader("cache-control", "no-cache");
                request.AddParameter("Accept", "application/json");
                request.AddParameter("Content-Type", "application/json");
                string json = JsonConvert.SerializeObject((MeterByZoneViewModel)postModel);
                request.AddParameter("application/json", json, ParameterType.RequestBody);
                var response = await client.ExecuteTaskAsync(request);
                if (response.Content.Length > 0)
                {
                    IEnumerable<BillingDownloadSerializer> objectResponse = JsonConvert.DeserializeObject<IEnumerable<BillingDownloadSerializer>>(response.Content);
                    return View(objectResponse);
                }
                else
                {
                    objectResponse2 = model2;
                    return View(objectResponse2);
                }
            }
            else
            {
                ViewBag.Id = wspId;
                objectResponse2 = model2;
                return View(objectResponse2);
            }

        }
        public IActionResult MonthlyBills()
        {
            IEnumerable<BillingDownloadSerializer> objectResponse2;
            List<BillingDownloadSerializer> model2 = new List<BillingDownloadSerializer>();
            objectResponse2 = model2;
            ViewBag.Model = objectResponse2;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> MonthlyBills([FromForm] string month)
        {
            var wspId = Convert.ToInt32(User.FindFirst("WSPId")?.Value);
            var postModel = new MonthlyBillsViewModel()
            {
                Id = wspId,
                Month = month
            };
            var baseUrl = _configuration["ApiBaseUrl"];
            var url = baseUrl + "wsp/MonthlyBills";
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            string json = JsonConvert.SerializeObject((MonthlyBillsViewModel)postModel);
            request.AddJsonBody(json);
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("Accept", "application/json");
            request.AddParameter("Content-Type", "application/json");
            var response = await client.ExecuteTaskAsync(request);

            var res = response.Content;
            IEnumerable<BillingDownloadSerializer> objectResponse = JsonConvert.DeserializeObject<IEnumerable<BillingDownloadSerializer>>(res);
            Alert(response.Content, NotificationType.success, 5000);
            ViewBag.Model = objectResponse;
            return View();

        }
        public IActionResult MonthPaidBills(IEnumerable<BillingDownloadSerializer> model)
        {

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> MonthPaidBills([FromForm] string month)
        {

            var wspId = Convert.ToInt32(User.FindFirst("WSPId")?.Value);
            var postModel = new MonthlyBillsViewModel()
            {
                Id = wspId,
                Month = month
            };
            var baseUrl = _configuration["ApiBaseUrl"];
            var url = baseUrl + "wsp/MonthPaidBills";
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            string json = JsonConvert.SerializeObject((MonthlyBillsViewModel)postModel);
            request.AddJsonBody(json);
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("Accept", "application/json");
            request.AddParameter("Content-Type", "application/json");
            var response = await client.ExecuteTaskAsync(request);

            var res = response.Content;
            IEnumerable<BillingDownloadSerializer> objectResponse = JsonConvert.DeserializeObject<IEnumerable<BillingDownloadSerializer>>(res);
            Alert(response.Content, NotificationType.success, 5000);
            return View(objectResponse);
        }
        public IActionResult MonthUnPaidBills(IEnumerable<BillingDownloadSerializer> model)
        {

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> MonthUnPaidBills([FromForm] string month)
        {

            var wspId = Convert.ToInt32(User.FindFirst("WSPId")?.Value);
            var postModel = new MonthlyBillsViewModel()
            {
                Id = wspId,
                Month = month
            };
            var baseUrl = _configuration["ApiBaseUrl"];
            var url = baseUrl + "wsp/MonthUnPaidBills";
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            string json = JsonConvert.SerializeObject((MonthlyBillsViewModel)postModel);
            request.AddJsonBody(json);
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("Accept", "application/json");
            request.AddParameter("Content-Type", "application/json");
            var response = await client.ExecuteTaskAsync(request);

            var res = response.Content;
            IEnumerable<BillingDownloadSerializer> objectResponse = JsonConvert.DeserializeObject<IEnumerable<BillingDownloadSerializer>>(res);
            Alert(response.Content, NotificationType.success, 5000);
            return View(objectResponse);
        }
        #endregion
        public async Task<IActionResult> BilledVolumeWSP()
        {
            var wspId = Convert.ToInt32(User.FindFirst("WSPId")?.Value);
            var baseUrl = _configuration["ApiBaseUrl"];
            var localbaseUrl = _configuration["LocalApiUrl"];
            var urlString = baseUrl + "wsp/TodaysRevenue/" + wspId;
            Uri url = new Uri(urlString, UriKind.Absolute);
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("Content-Type", "application/json");
            var response = await client.ExecuteTaskAsync(request);
            var res = response.Content;
            IEnumerable<WSPGetConsumptionAndRevenue> objectResponse = JsonConvert.DeserializeObject<IEnumerable<WSPGetConsumptionAndRevenue>>(res);
            return View(objectResponse);
        }
        public async Task<IActionResult> UnpaidBills()
        {
            var wspId = Convert.ToInt32(User.FindFirst("WSPId")?.Value);
            var baseUrl = _configuration["ApiBaseUrl"];
            var localbaseUrl = _configuration["LocalApiUrl"];
            var urlString = baseUrl + "wsp/UnPaidBills/" + wspId;
            Uri url = new Uri(urlString, UriKind.Absolute);
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("Content-Type", "application/json");
            var response = await client.ExecuteTaskAsync(request);
            var res = response.Content;
            IEnumerable<BillingDownloadSerializer> objectResponse = JsonConvert.DeserializeObject<IEnumerable<BillingDownloadSerializer>>(res);
            return View(objectResponse);
        }
        public async Task<IActionResult> PaidBills()
        {
            var wspId = Convert.ToInt32(User.FindFirst("WSPId")?.Value);
            var baseUrl = _configuration["ApiBaseUrl"];
            var localbaseUrl = _configuration["LocalApiUrl"];
            var urlString = baseUrl + "wsp/PaidBills/" + wspId;
            Uri url = new Uri(urlString, UriKind.Absolute);
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("Content-Type", "application/json");
            var response = await client.ExecuteTaskAsync(request);
            var res = response.Content;
            IEnumerable<BillingDownloadSerializer> objectResponse = JsonConvert.DeserializeObject<IEnumerable<BillingDownloadSerializer>>(res);
            return View(objectResponse);
        }
        public async Task<IActionResult> Yesterday()
        {
            var wspId = Convert.ToInt32(User.FindFirst("WSPId")?.Value);
            var baseUrl = _configuration["ApiBaseUrl"];
            var localbaseUrl = _configuration["LocalApiUrl"];
            var urlString = baseUrl + "wsp/Yesterday/" + wspId;
            Uri url = new Uri(urlString, UriKind.Absolute);
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("Content-Type", "application/json");
            var response = await client.ExecuteTaskAsync(request);
            var res = response.Content;
            IEnumerable<MeterDownloadSerializer> objectResponse = JsonConvert.DeserializeObject<IEnumerable<MeterDownloadSerializer>>(res);
            return View(objectResponse);
        }
        public async Task<IActionResult> Today()
        {
            var wspId = Convert.ToInt32(User.FindFirst("WSPId")?.Value);
            var baseUrl = _configuration["ApiBaseUrl"];
            var localbaseUrl = _configuration["LocalApiUrl"];
            var urlString = baseUrl + "wsp/Today/" + wspId;
            Uri url = new Uri(urlString, UriKind.Absolute);
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("Content-Type", "application/json");
            var response = await client.ExecuteTaskAsync(request);
            var res = response.Content;
            IEnumerable<MeterDownloadSerializer> objectResponse = JsonConvert.DeserializeObject<IEnumerable<MeterDownloadSerializer>>(res);
            return View(objectResponse);
        }
        public async Task<IActionResult> TotalNumberOfCustomers()
        {
            var wspId = Convert.ToInt32(User.FindFirst("WSPId")?.Value);
            var baseUrl = _configuration["ApiBaseUrl"];
            var localbaseUrl = _configuration["LocalApiUrl"];
            var urlString = baseUrl + "wsp/Customers/" + wspId;
            Uri url = new Uri(urlString, UriKind.Absolute);
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("Content-Type", "application/json");
            var response = await client.ExecuteTaskAsync(request);
            var res = response.Content;
            IEnumerable<WSPCustomerDownloadSerializer> objectResponse = JsonConvert.DeserializeObject<IEnumerable<WSPCustomerDownloadSerializer>>(res);
            return View(objectResponse);
        }
        public async Task<IActionResult> TodaysRevenueWSP()
        {
            var wspId = Convert.ToInt32(User.FindFirst("WSPId")?.Value);
            var baseUrl = _configuration["ApiBaseUrl"];
            var localbaseUrl = _configuration["LocalApiUrl"];
            var urlString = baseUrl + "wsp/TodaysRevenue/" + wspId;
            Uri url = new Uri(urlString, UriKind.Absolute);
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("Content-Type", "application/json");
            var response = await client.ExecuteTaskAsync(request);
            var res = response.Content;
            IEnumerable<WSPGetConsumptionAndRevenue> objectResponse = JsonConvert.DeserializeObject<IEnumerable<WSPGetConsumptionAndRevenue>>(res);
            return View(objectResponse);

        }

        public IActionResult MapAndLocation()
        {
            return View();
        }


    }
}