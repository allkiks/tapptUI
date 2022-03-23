using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TAPPT.Web.Helpers;
using TAPPT.Web.Identity.Database;
using TAPPT.Web.Identity.Models;
using TAPPT.Web.Identity.ViewModels;
using TAPPT.Web.Mailer;
using TAPPT.Web.Models;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Newtonsoft.Json;
using RestSharp;

namespace TAPPT.Web.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        private readonly IEmailService _emailService;
        private readonly IEmailConfiguration _emailConfiguration;
        public AdminController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager,
                                 RoleManager<IdentityRole> roleManager,
                                 IConfiguration configuration,
                                 IEmailService emailService,
                                 IWebHostEnvironment env,
                                 IEmailConfiguration emailConfiguration)

        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _env = env;
            _emailService = emailService;
            _emailConfiguration = emailConfiguration;
        }

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
        public IActionResult RevenueManagement(int id)
        {
            ViewBag.Id = id;
            return View();
        }
        public IActionResult BillsPaidlast24(int id)
        {
            ViewBag.Id = id;
            return View();
        }
        public IActionResult TotalWSPlist(int id)
        {
            ViewBag.Id = id;
            return View();
        }
        public IActionResult AdminBilling(int id)
        {
            ViewBag.Id = id;
            return View();
        }
        public IActionResult MeterList(int id)
        {
            ViewBag.Id = id;
            return View();
        }
        public IActionResult TransmittingCurrent(int id)
        {
            ViewBag.Id = id;
            return View();
        }
        public IActionResult TransmittingPast24(int id)
        {
            ViewBag.Id = id;
            return View();
        }
        public IActionResult TodaysConsumption(int id)
        {
            ViewBag.Id = id;
            return View();
        }
        public async Task<IActionResult> Billing()
        {
            var baseUrl = _configuration["ApiBaseUrl"];
            var localbaseUrl = _configuration["LocalApiUrl"];
            var urlString = baseUrl + "admin/GetUtilities";
            Uri url = new Uri(urlString, UriKind.Absolute);
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("Content-Type", "application/json");
            var response = await client.ExecuteTaskAsync(request);
            var res = response.Content;
            IEnumerable<UtlitiesDownloadSerializer> objectResponse = JsonConvert.DeserializeObject<IEnumerable<UtlitiesDownloadSerializer>>(res);
            return View(objectResponse);
        }
        public async Task<IActionResult> MeterByWsp(int id)
        {
            var baseUrl = _configuration["ApiBaseUrl"];
            var localbaseUrl = _configuration["LocalApiUrl"];
            var urlString = baseUrl + "admin/GetMeters/" + id;
            Uri url = new Uri(urlString, UriKind.Absolute);
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("Content-Type", "application/json");
            var response = await client.ExecuteTaskAsync(request);
            var res = response.Content;
            IEnumerable<MeterDownloadSerializer> objectResponse = JsonConvert.DeserializeObject<IEnumerable<MeterDownloadSerializer>>(response.Content);
            return View(objectResponse);

        }
        #region User Management
        public async Task<IActionResult> Users()
        {
            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            var management = await _userManager.GetUsersInRoleAsync("Management");
            var support = await _userManager.GetUsersInRoleAsync("Support");
            var wspAdmins = await _userManager.GetUsersInRoleAsync("WSPAdmin");

            ViewBag.Admins = admins;
            ViewBag.Management = management;
            ViewBag.Support = support;
            ViewBag.WSPAdmins = wspAdmins;
            return View();
        }
        public async Task<IActionResult> ManageUser([FromRoute] string id)
        {
            ViewBag.User = await _userManager.FindByIdAsync(id);
            ViewBag.Roles = _roleManager.Roles.ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> EditUser([FromForm] RegisterViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UId);
            user.FirstName = model.FirstName;
            user.MiddleName = model.MiddleName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;
            user.Email = model.EmailAddress;
            user.UserName = model.EmailAddress;
            await _userManager.UpdateAsync(user);
            ViewBag.User = user;
            ViewBag.Roles = _roleManager.Roles.ToList();

            var postModel = new AuditLogUploadViewModel()
            {
                CreatedBy = User.FindFirst("DisplayName")?.Value,
                Description = $"Edited {user.DisplayName}"
            };
            var baseUrl = _configuration["ApiBaseUrl"];
            var url = baseUrl + "admin/PostAuditLog";
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            string json = JsonConvert.SerializeObject((AuditLogUploadViewModel)postModel);
            request.AddJsonBody(json);
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("Accept", "application/json");
            request.AddParameter("Content-Type", "application/json");
            var response = await client.ExecuteTaskAsync(request);
            var res = response.Content;
            return RedirectToAction("ManageUser", "Admin", new { id = user.Id });
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromForm] RegisterViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UId);
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            //var defaultUserPassword = RandomCodeGenerator.GeneratePassword();
            var defaultUserPassword = "@GosoftUser1";
            IdentityResult result = _userManager.ResetPasswordAsync(user, code, defaultUserPassword).Result;
            try { await SendCredentials(user, defaultUserPassword); } catch{ };
            
            ViewBag.User = user;
            ViewBag.Roles = _roleManager.Roles.ToList();

            var postModel = new AuditLogUploadViewModel()
            {
                CreatedBy = User.FindFirst("DisplayName")?.Value,
                Description = $"Reset Password for {user.Email}"
            };
            var baseUrl = _configuration["ApiBaseUrl"];
            var url = baseUrl + "admin/PostAuditLog";
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            string json = JsonConvert.SerializeObject((AuditLogUploadViewModel)postModel);
            request.AddJsonBody(json);
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("Accept", "application/json");
            request.AddParameter("Content-Type", "application/json");
            var response = await client.ExecuteTaskAsync(request);
            var res = response.Content;

            return RedirectToAction("ManageUser", "Admin", new { id = user.Id });
        }
        [HttpPost]
        public async Task<IActionResult> ChangeRole([FromForm] RegisterViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UId);
            var newrole = await _roleManager.FindByIdAsync(model.RoleName);

            var postModel = new AuditLogUploadViewModel()
            {
                CreatedBy = User.FindFirst("DisplayName")?.Value,
                Description = $"Changed role of {user.Email} from {user.RoleName} to {newrole.Name}"
            };
            var baseUrl = _configuration["ApiBaseUrl"];
            var url = baseUrl + "admin/PostAuditLog";
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            string json = JsonConvert.SerializeObject((AuditLogUploadViewModel)postModel);
            request.AddJsonBody(json);
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("Accept", "application/json");
            request.AddParameter("Content-Type", "application/json");

            string roleName = "";
            if (user.RoleName == "Operator Admin")
            {
                var oldrole = await _roleManager.FindByNameAsync("Admin");
                await _userManager.RemoveFromRoleAsync(user, oldrole.Name);
            }
            if (user.RoleName == "WSP Admin")
            {
                var oldrole = await _roleManager.FindByNameAsync("WSPAdmin");
                await _userManager.RemoveFromRoleAsync(user, oldrole.Name);
            }
            if (user.RoleName == "WSP Clark")
            {
                var oldrole = await _roleManager.FindByNameAsync("WSPUser");
                await _userManager.RemoveFromRoleAsync(user, oldrole.Name);
            }
            if (user.RoleName == "Customer")
            {
                var oldrole = await _roleManager.FindByNameAsync("Customer");
                await _userManager.RemoveFromRoleAsync(user, oldrole.Name);
            }
            if (user.RoleName == "Operator Management")
            {
                var oldrole = await _roleManager.FindByNameAsync("Management");
                await _userManager.RemoveFromRoleAsync(user, oldrole.Name);
            }
            if (user.RoleName == "Operator Support")
            {
                var oldrole = await _roleManager.FindByNameAsync("Support");
                await _userManager.RemoveFromRoleAsync(user, oldrole.Name);
            }

            if (newrole.Name == "Admin")
            {
                roleName = "Operator Admin";
            }
            if (newrole.Name == "WSPAdmin")
            {
                roleName = "WSP Admin";
            }
            if (newrole.Name == "WSPUser")
            {
                roleName = "WSP Clark";
            }
            if (newrole.Name == "Customer")
            {
                roleName = "Customer";
            }
            if (newrole.Name == "Management")
            {
                roleName = "Operator Admin";
            }
            if (newrole.Name == "Support")
            {
                roleName = "Operator Support";
            }

            await _userManager.AddToRoleAsync(user, newrole.Name);
            user.RoleName = roleName;
            await _userManager.UpdateAsync(user);
            ViewBag.User = user;
            ViewBag.Roles = _roleManager.Roles.ToList();

            var response = await client.ExecuteTaskAsync(request);
            var res = response.Content;
            return RedirectToAction("ManageUser", "Admin", new { id = user.Id });
        }
        #endregion
        #region Bouquetes
        public async Task<IActionResult> Bouquets()
        {
            IEnumerable<BouquetViewModel> model;
            var baseUrl = _configuration["ApiBaseUrl"];
            var url = baseUrl + "/admin/GetBouquets";
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("Content-Type", "application/json");
            var response = await client.ExecuteTaskAsync(request);
            var res = response.Content;
            IEnumerable<BouquetViewModel> objectResponse = JsonConvert.DeserializeObject<IEnumerable<BouquetViewModel>>(res);
            model = objectResponse;

            return View(model);

        }
        public IActionResult AddBouquet()
        {
            return View();
        }
        public async Task<IActionResult> ManageBauquet(int id)
        {
            var baseUrl = _configuration["ApiBaseUrl"];
            var url = baseUrl + "/admin/GetBouquet/" + id;
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("Content-Type", "application/json");
            var response = await client.ExecuteTaskAsync(request);
            var res = response.Content;
            BouquetViewModel objectResponse = JsonConvert.DeserializeObject<BouquetViewModel>(res);

            return View(objectResponse);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBouquet([FromForm] BouquetViewModel model)
        {
            var postModel = new BouquetViewModel()
            {
                Name = model.Name,
                Price = model.Price
            };
            var baseUrl = _configuration["ApiBaseUrl"];
            var url = baseUrl + "admin/CreateBouquet";
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            string json = JsonConvert.SerializeObject((BouquetViewModel)postModel);
            request.AddJsonBody(json);
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("Accept", "application/json");
            request.AddParameter("Content-Type", "application/json");
            await client.ExecuteTaskAsync(request);

            var postModel2 = new AuditLogUploadViewModel()
            {
                CreatedBy = User.FindFirst("DisplayName")?.Value,
                Description = $"edit Bouquet {model.Name}"
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
            return RedirectToAction("Bouquets", "Admin");
        }
        public async Task<IActionResult> EditBouquet([FromForm] BouquetViewModel model)
        {
            var postModel = new BouquetViewModel()
            {
                Id = model.Id,
                Name = model.Name,
                Price = model.Price
            };
            var baseUrl = _configuration["ApiBaseUrl"];
            var url = baseUrl + "admin/EditBouquet";
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            string json = JsonConvert.SerializeObject((BouquetViewModel)postModel);
            request.AddJsonBody(json);
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("Accept", "application/json");
            request.AddParameter("Content-Type", "application/json");
            await client.ExecuteTaskAsync(request);

            var postModel2 = new AuditLogUploadViewModel()
            {
                CreatedBy = User.FindFirst("DisplayName")?.Value,
                Description = $"edit Bouquet {model.Name}"
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
            return RedirectToAction("Bouquets", "Admin");
        }
        #endregion
        #region Alarms
        public async Task<IActionResult> Alarms()
        {
            var baseUrl = _configuration["ApiBaseUrl"];
            var url = baseUrl + "/admin/GetAlarms";
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("Content-Type", "application/json");
            var response = await client.ExecuteTaskAsync(request);
            IEnumerable<AlarmViewModel> objectResponse = JsonConvert.DeserializeObject<IEnumerable<AlarmViewModel>>(response.Content);
            return View(objectResponse);

        }
        public IActionResult AddAlarm()
        {
            return View();
        }
        public async Task<IActionResult> MapAlarms(int id)
        {
            var baseUrl = _configuration["ApiBaseUrl"];
            var url = baseUrl + "/admin/GetBouquet/" + id;
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("Content-Type", "application/json");
            var response = await client.ExecuteTaskAsync(request);

            BouquetViewModel bouquet = JsonConvert.DeserializeObject<BouquetViewModel>(response.Content);

            var urls = baseUrl + "/admin/GetAlarms";
            var clients = new RestClient(urls);
            var requests = new RestRequest(Method.GET);
            requests.AddHeader("cache-control", "no-cache");
            requests.AddParameter("Content-Type", "application/json");
            var responses = await clients.ExecuteTaskAsync(requests);
            IEnumerable<AlarmViewModel> alarms = JsonConvert.DeserializeObject<IEnumerable<AlarmViewModel>>(responses.Content);

            ViewBag.Bouquet = bouquet;
            ViewBag.Alarms = alarms;

            return View();
        }
        public async Task<IActionResult> ManageAlarm(int id)
        {
            var baseUrl = _configuration["ApiBaseUrl"];
            var url = baseUrl + "/admin/GetAlarm/" + id;
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("Content-Type", "application/json");
            var response = await client.ExecuteTaskAsync(request);
            var res = response.Content;
            AlarmViewModel objectResponse = JsonConvert.DeserializeObject<AlarmViewModel>(res);

            return View(objectResponse);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAlarm([FromForm] AlarmViewModel model)
        {
            var postModel = new AlarmViewModel()
            {
                Name = model.Name
            };
            var baseUrl = _configuration["ApiBaseUrl"];
            var url = baseUrl + "admin/CreateAlarm";
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            string json = JsonConvert.SerializeObject((AlarmViewModel)postModel);
            request.AddJsonBody(json);
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("Accept", "application/json");
            request.AddParameter("Content-Type", "application/json");
            await client.ExecuteTaskAsync(request);

            var postModel2 = new AuditLogUploadViewModel()
            {
                CreatedBy = User.FindFirst("DisplayName")?.Value,
                Description = $"Create Alarm {model.Name}"
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
            return RedirectToAction("Alarms", "Admin");
        }
        public async Task<IActionResult> EditAlarm([FromForm] AlarmViewModel model)
        {
            var postModel = new AlarmViewModel()
            {
                Id = model.Id,
                Name = model.Name,
            };
            var baseUrl = _configuration["ApiBaseUrl"];
            var url = baseUrl + "admin/EditAlarm";
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            string json = JsonConvert.SerializeObject((AlarmViewModel)postModel);
            request.AddJsonBody(json);
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("Accept", "application/json");
            request.AddParameter("Content-Type", "application/json");
            await client.ExecuteTaskAsync(request);

            var postModel2 = new AuditLogUploadViewModel()
            {
                CreatedBy = User.FindFirst("DisplayName")?.Value,
                Description = $"Edit Alarm {model.Name}"
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
            return RedirectToAction("Alarms", "Admin");
        }
        public async Task<IActionResult> PostSelected([FromForm]PostSelectedViewModel model)
        {
            var baseUrl = _configuration["ApiBaseUrl"];
            var url = baseUrl + "/admin/GetBouquet/";
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("Content-Type", "application/json");
            var response = await client.ExecuteTaskAsync(request);

            BouquetViewModel bouquet = JsonConvert.DeserializeObject<BouquetViewModel>(response.Content);

            var urls = baseUrl + "/admin/GetAlarms";
            var clients = new RestClient(urls);
            var requests = new RestRequest(Method.GET);
            requests.AddHeader("cache-control", "no-cache");
            requests.AddParameter("Content-Type", "application/json");
            var responses = await clients.ExecuteTaskAsync(requests);
            IEnumerable<AlarmViewModel> alarms = JsonConvert.DeserializeObject<IEnumerable<AlarmViewModel>>(responses.Content);

            ViewBag.Bouquet = bouquet;
            ViewBag.Alarms = alarms;

            return Ok();
        }
        #endregion
        #region audit Logs
        public async Task<IActionResult> AuditLogs()
        {
            var baseUrl = _configuration["ApiBaseUrl"];
            var url = baseUrl + "/admin/GetAuditLogs";
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("Content-Type", "application/json");
            var response = await client.ExecuteTaskAsync(request);
            IEnumerable<AuditLogUploadViewModel> objectResponse = JsonConvert.DeserializeObject<IEnumerable<AuditLogUploadViewModel>>(response.Content);
            return View(objectResponse);

        }
        #endregion
        #region Meters
        public IActionResult MeterDetails(int id)
        {
            ViewBag.Id = id;
            return View();
        }
        public async Task<IActionResult> WSPMeters(int id)
        {
            var baseUrl = _configuration["ApiBaseUrl"];
            var url = baseUrl + "admin/GetMeters/" + id;
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
        private async Task SendCredentials(ApplicationUser user, string password)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var hostname = $"{this.Request.Scheme}://{this.Request.Host}";
            var title = "TAPPT Smart Water Platform";
            var callbackUrl = Url.Action("Login", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
            var callbackLink = $"Please confirm a succesfull password reset by clicking this link: <a href='{callbackUrl}' class='btn btn-primary'>Confirmation Link</a>";
            var pathToTemplate = Path.Combine(_env.ContentRootPath, "Mailer", "Templates", "ResetPassword.html");
            var builder = new BodyBuilder();

            using (var SourceReader = System.IO.File.OpenText(pathToTemplate))
            {

                builder.HtmlBody = SourceReader.ReadToEnd();

            }
            string messageBody = builder.HtmlBody.Replace("@Name", user.DisplayName)
                                        .Replace("@Title", title)
                                        .Replace("@CallbackLink", callbackLink)
                                        .Replace("@Host", title)
                                        .Replace("@UserName", user.Email)
                                        .Replace("@Password", password)
                                        .Replace("@CallbackUrl", callbackUrl);
            var emailMessage = new EmailMessage
            {
                Content = messageBody
            };
            emailMessage.ToAddresses.Add(new EmailAddress { Name = user.DisplayName, Address = user.Email });
            emailMessage.FromAddresses.Add(new EmailAddress { Name = hostname, Address = _emailConfiguration.SmtpUsername });
            emailMessage.Subject = "Password Reset";
            var emailService = _emailService as EmailService;
            var emailConfiguration = _emailConfiguration as EmailConfiguration;
            MailerActions.SendEmail(emailMessage, emailService, emailConfiguration);
            BackgroundJob.Enqueue(() => MailerActions.SendEmail(emailMessage, emailService, emailConfiguration));
        }
    }
}

