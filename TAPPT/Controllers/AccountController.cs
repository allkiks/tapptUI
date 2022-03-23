using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TAPPT.Web.Helpers;
using TAPPT.Web.Identity.Models;
using TAPPT.Web.Identity.ViewModels;
using TAPPT.Web.Mailer;
using TAPPT.Web.Models;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Newtonsoft.Json;
using RestSharp;
using Microsoft.AspNetCore.Cors;

namespace TAPPT.Web.Controllers
{
    public class AccountController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        private readonly IEmailService _emailService;
        private readonly IEmailConfiguration _emailConfiguration;
        public AccountController(UserManager<ApplicationUser> userManager,
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
        [Authorize]
        public IActionResult Dashboard()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            var result = await _signInManager.PasswordSignInAsync(model.EmailAddress, model.Password, model.RememberMe, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Login failed,invalid Email address or Password ");
                return View(new LoginViewModel());
            }
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                var postModel = new AuditLogUploadViewModel()
                {
                    CreatedBy = User.FindFirst("DisplayName")?.Value,
                    Description = $"Signed in"
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
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var postModel = new AuditLogUploadViewModel()
                {
                    CreatedBy = User.FindFirst("DisplayName")?.Value,
                    Description = $"Signed In"
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
                return RedirectToUrl("Index", "Home", returnUrl);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToUrl("Index", "Home", returnUrl);
            }
        }
        [Authorize(Roles = "Admin")]
        public IActionResult AddAdmin()
        {
            ViewBag.Roles = _roleManager.Roles.ToList();
            return View(new RegisterViewModel());
        }
        [Authorize(Roles = "Admin")]
        public IActionResult AddWSPAdmin(int id)
        {
            ViewBag.Id = id;
            return View(new RegisterViewModel());
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> AddAdmin(RegisterViewModel model)
        {
           
            var defaultUserPassword = RandomCodeGenerator.GeneratePassword();
            var role = await _roleManager.FindByIdAsync(model.RoleName);
            var user = new ApplicationUser
            {
                UserName = model.EmailAddress,
                Email = model.EmailAddress,
                PhoneNumber = model.PhoneNumber,
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                RoleName = role.Name,
                Residence = model.Residence,
                PostalAddress = model.PostaAddress,
                WSPId =4.ToString()
            };
            var result = await _userManager.CreateAsync(user, defaultUserPassword);
            if (result.Succeeded)
            {
                result = await _userManager.AddToRoleAsync(user, role.NormalizedName);

                var postModel = new PostUserModel()
                {
                    WSPUtilityId = model.WSPId,
                    FirstName = model.FirstName,
                    MiddleName = model.MiddleName,
                    LastName = model.LastName,
                    PostalAddress = model.PostaAddress,
                    Residence = model.Residence,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.EmailAddress,
                    IdentityId = user.Id
                };
                var baseUrl = _configuration["ApiBaseUrl"];
                var url = baseUrl + "admin/AddWspUser";
                var client = new RestClient(url);
                var request = new RestRequest(Method.POST);
                string json = JsonConvert.SerializeObject((PostUserModel)postModel);
                request.AddJsonBody(json);
                request.AddHeader("cache-control", "no-cache");
                request.AddParameter("Accept", "application/json");
                request.AddParameter("Content-Type", "application/json");
                var response = await client.ExecuteTaskAsync(request);
                var res = response.Content;
                await SendCredentials(user, defaultUserPassword);
             
                var postModel2 = new AuditLogUploadViewModel()
                {
                    CreatedBy = User.FindFirst("DisplayName")?.Value,
                    Description = $"Created User {user.DisplayName} with role {user.RoleName}"
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
                return View("AddAdmin", new RegisterViewModel());
            }

            foreach (var error in result.Errors.Select(x => x.Description))
            {
                ModelState.AddModelError(string.Empty, error);
            }
            return View("AddAdmin", new RegisterViewModel());
        }
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> AddWSPAdmin(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(new RegisterViewModel());
            }
            var defaultUserPassword = RandomCodeGenerator.GeneratePassword();
            var user = new ApplicationUser
            {
                UserName = model.EmailAddress,
                Email = model.EmailAddress,
                PhoneNumber = model.PhoneNumber,
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                RoleName = "WSP Admin",
                Residence = model.Residence,
                PostalAddress = model.PostaAddress,
                WSPId = model.WSPId.ToString()
            };
            var result = await _userManager.CreateAsync(user, "@GosoftUser1");
            if (result.Succeeded)
            {
                var role = await _roleManager.FindByNameAsync("WSPAdmin");
                result = await _userManager.AddToRoleAsync(user, role.NormalizedName);

                var postModel = new PostUserModel()
                {
                    WSPUtilityId = model.WSPId,
                    FirstName = model.FirstName,
                    MiddleName = model.MiddleName,
                    LastName = model.LastName,
                    PostalAddress = model.PostaAddress,
                    Residence = model.Residence,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.EmailAddress,
                    IdentityId = user.Id
                };
                var baseUrl = _configuration["ApiBaseUrl"];
                var url = baseUrl + "admin/AddWspUser";
                var client = new RestClient(url);
                var request = new RestRequest(Method.POST);
                string json = JsonConvert.SerializeObject((PostUserModel)postModel);
                request.AddJsonBody(json);
                request.AddHeader("cache-control", "no-cache");
                request.AddParameter("Accept", "application/json");
                request.AddParameter("Content-Type", "application/json");
                var response = await client.ExecuteTaskAsync(request);
                var res = response.Content;
                await SendCredentials(user, defaultUserPassword);

                var postModel2 = new AuditLogUploadViewModel()
                {
                    CreatedBy = User.FindFirst("DisplayName")?.Value,
                    Description = $"Created User {user.DisplayName} with role {user.RoleName}"
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
                ViewBag.Id = model.WSPId;
                return RedirectToAction("Utilities", "Setting");
            }

            foreach (var error in result.Errors.Select(x => x.Description))
            {
                ModelState.AddModelError(string.Empty, error);
            }
            ViewBag.Id = model.WSPId;
            return RedirectToAction("Utilities", "Setting");
        }
        [Authorize(Roles = "WSPAdmin")]
        public IActionResult AddWSPUser()
        {
            return View(new RegisterViewModel());
        }
        [Authorize(Roles = "WSPAdmin")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> AddWSPUser(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            var wspId = Convert.ToInt32(User.FindFirst("WSPId")?.Value);
            var defaultUserPassword = RandomCodeGenerator.GeneratePassword();
            var user = new ApplicationUser
            {
                UserName = model.EmailAddress,
                Email = model.EmailAddress,
                PhoneNumber = model.PhoneNumber,
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                RoleName = "WSP Clark",
                Residence = model.Residence,
                PostalAddress = model.PostaAddress,
                WSPId = wspId.ToString()
            };
            var result = await _userManager.CreateAsync(user, defaultUserPassword);
            if (result.Succeeded)
            {
                var role = await _roleManager.FindByNameAsync("WSPUser");
                result = await _userManager.AddToRoleAsync(user, role.NormalizedName);
              
                var postModel = new PostUserModel()
                {
                    WSPUtilityId = wspId,
                    FirstName = model.FirstName,
                    MiddleName = model.MiddleName,
                    LastName = model.LastName,
                    PostalAddress = model.PostaAddress,
                    Residence = model.Residence,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.EmailAddress,
                    IdentityId = user.Id
                };
                var baseUrl = _configuration["ApiBaseUrl"];
                var url = baseUrl + $"admin/AddWspUser";
                var client = new RestClient(url);
                var request = new RestRequest(Method.POST);
                string json = JsonConvert.SerializeObject((PostUserModel)postModel);
                request.AddJsonBody(json);
                request.AddHeader("cache-control", "no-cache");
                request.AddParameter("Accept", "application/json");
                request.AddParameter("Content-Type", "application/json");
                var response = await client.ExecuteTaskAsync(request);
                await SendCredentials(user, defaultUserPassword);
                var res = response.Content;

                var postModel2 = new AuditLogUploadViewModel()
                {
                    CreatedBy = User.FindFirst("DisplayName")?.Value,
                    Description = $"Created User {user.DisplayName} with role {user.RoleName}"
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
                return View("AddWSPUser", new RegisterViewModel());
            }

            foreach (var error in result.Errors.Select(x => x.Description))
            {
                ModelState.AddModelError(string.Empty, error);
            }
            return View("AddWSPUser", new RegisterViewModel());
        }
        [Authorize(Roles = "WSPAdmin")]
        public IActionResult AddCustomer()
        {
            var wspId = Convert.ToInt32(User.FindFirst("WSPId")?.Value);
            ViewBag.Id = wspId;
            return View(new RegisterViewModel());
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> AddCustomer(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(new RegisterViewModel());
            }
            var wspId = Convert.ToInt32(User.FindFirst("WSPId")?.Value);
            var defaultUserPassword = RandomCodeGenerator.GeneratePassword();
            var user = new ApplicationUser
            {
                UserName = model.PhoneNumber,
                Email = model.EmailAddress,
                PhoneNumber = model.PhoneNumber,
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                RoleName = "Customer",
                Residence = model.Residence,
                PostalAddress = model.PostaAddress,
                WSPId = wspId.ToString()
            };
            var result = await _userManager.CreateAsync(user, defaultUserPassword);
            if (result.Succeeded)
            {
                var role = await _roleManager.FindByNameAsync("Customer");
                result = await _userManager.AddToRoleAsync(user, role.NormalizedName);
                var postModel = new PostCustomerAndMeterModel()
                {
                    WSPUtilityId = wspId,
                    FirstName = model.FirstName,
                    MiddleName = model.MiddleName,
                    LastName = model.LastName,
                    PostalAddress = model.PostaAddress,
                    Residence = model.Residence,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.EmailAddress,
                    IdentityId = user.Id,
                    IdNumber = model.IdNumber,
                    MeterCode = model.MeterCode,
                    IMENumber = model.IMENumber,
                    IMSI = model.IMSI,
                    SIMCardNumber = model.SIMCardNumber,
                    Longitude = model.Longitude,
                    Latitude = model.Latitude,
                    TariffId = model.TariffId,
                    BillingSettingId = 1,
                    ZoneId = model.ZoneId
                };
                var baseUrl = _configuration["ApiBaseUrl"];
                var url = baseUrl + $"WSP/AddCustomerAndMeter";
                var client = new RestClient(url);
                var request = new RestRequest(Method.POST);
                string json = JsonConvert.SerializeObject((PostCustomerAndMeterModel)postModel);
                request.AddJsonBody(json);
                request.AddHeader("cache-control", "no-cache");
                request.AddParameter("Accept", "application/json");
                request.AddParameter("Content-Type", "application/json");
                var response = await client.ExecuteTaskAsync(request);
                await SendCredentials(user, defaultUserPassword);
                var res = response.Content;

                var postModel2 = new AuditLogUploadViewModel()
                {
                    CreatedBy = User.FindFirst("DisplayName")?.Value,
                    Description = $"Created User {user.DisplayName} with role {user.RoleName}"
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
                ViewBag.Id = wspId;
                return View(new RegisterViewModel());
            }

            foreach (var error in result.Errors.Select(x => x.Description))
            {
                ModelState.AddModelError(string.Empty, error);
            }
            ViewBag.Id = wspId;
            return View(new RegisterViewModel());
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
        #region Methods
        private async Task SendCredentials(ApplicationUser user, string password)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var hostname = $"{this.Request.Scheme}://{this.Request.Host}";
            var title = "TAPPT Smart Water Platform";
            var callbackUrl = Url.Action("Login", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
            var callbackLink = $"Please confirm your account by clicking this link: <a href='{callbackUrl}' class='btn btn-primary'>Confirmation Link</a>";
            var pathToTemplate = Path.Combine(_env.ContentRootPath, "Mailer", "Templates", "ConfirmAccount.html");
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
            emailMessage.FromAddresses.Add(new EmailAddress { Name = title, Address = _emailConfiguration.SmtpUsername });
            emailMessage.Subject = "Account Activation";
            var emailService = _emailService as EmailService;
            var emailConfiguration = _emailConfiguration as EmailConfiguration;
            MailerActions.SendEmail(emailMessage, emailService, emailConfiguration);
            //BackgroundJob.Enqueue(() => MailerActions.SendEmail(emailMessage, emailService, emailConfiguration));
        }
        private IActionResult RedirectToUrl(string fallbackAction, string fallbackController, string redirectUrl)
        {
            if (Url.IsLocalUrl(redirectUrl))
            {
                return Redirect(redirectUrl);
            }
            else
            {
                return RedirectToAction(fallbackAction, fallbackController);
            }
        }
        #endregion
    }
}