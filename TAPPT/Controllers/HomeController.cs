using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TAPPT.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.Extensions.Options;
using TAPPT.Web.Helpers;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Rest;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors;

namespace TAPPT.Web.Controllers
{
    [Authorize]
    [EnableCors("CORS")]
    public class HomeController : Controller
    {
        private HttpContext httpContext;
        private readonly IConfiguration _configuration;
        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("WSPAdmin"))
            {
                var wspId = Convert.ToInt32(User.FindFirst("WSPId")?.Value);
                ViewBag.Id = wspId;
            }
            if (User.IsInRole("SuperAdmin"))
            {
                var wspId = Convert.ToInt32(User.FindFirst("WSPId")?.Value);
                ViewBag.Id = wspId;
            }
            return View();
        }

        private async Task<string> GetPowerBIAccessToken(PowerBISettings powerBISettings)
        {
            using (var client = new HttpClient())
            {
                var form = new Dictionary<string, string>();
                form["grant_type"] = "password";
                form["resource"] = powerBISettings.ResourceUrl;
                form["username"] = powerBISettings.UserName;
                form["password"] = powerBISettings.Password;
                form["client_id"] = powerBISettings.ApplicationId.ToString();
                form["client_secret"] = powerBISettings.ApplicationSecret;
                form["scope"] = "openid";

                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");

                using (var formContent = new FormUrlEncodedContent(form))
                using (var response = await client.PostAsync(powerBISettings.AuthorityUrl, formContent))
                {
                    var body = await response.Content.ReadAsStringAsync();
                    var jsonBody = JObject.Parse(body);
                    return jsonBody.SelectToken("access_token").Value<string>();
                }
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
