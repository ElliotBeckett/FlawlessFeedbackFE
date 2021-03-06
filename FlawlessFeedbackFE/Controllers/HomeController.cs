using FlawlessFeedbackFE.Helpers;
using FlawlessFeedbackFE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Net.Http;

namespace FlawlessFeedbackFE.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private HttpClient _client;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _client = httpClientFactory.CreateClient("FlawlessFeedbackHttpClient");
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult FAQ()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(UserInfo userInfo)
        {
            var response = _client.PostAsJsonAsync("Token/GenerateToken", userInfo).Result;
            var lastUrl = TempData["RedirectUrl"].ToString();

            /* TODO add feedback for login attempt */

            if (response.IsSuccessStatusCode)
            {
                string token = response.Content.ReadAsStringAsync().Result;

                // Send the userInfo to the API to get the userName
                var getUsername = _client.PostAsJsonAsync("Token/GetUserName", userInfo).Result;

                // Read the content of the returned username and turn it back into a string
                var userName = getUsername.Content.ReadAsStringAsync().Result;

                HttpContext.Session.SetString("Token", token);
                HttpContext.Session.SetString("UserName", userName.ToString());

#if DEBUG
                //var userName = HttpContext.Session.GetString("UserName");
#endif

                if (!String.IsNullOrWhiteSpace(lastUrl))
                {
                    return Redirect(lastUrl);
                }

                return RedirectToAction("Index");
            }

            return View();
        }

        public IActionResult Logout()
        {
            var token = HttpContext.Session.GetString("Token");

            if (token != null)
            {
                HttpContext.Session.SetString("Token", "");
                HttpContext.Session.Remove("Token");
                return RedirectToAction("Index");
            }

            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(UserInfo userInfo)
        {
            try
            {
                ApiRequest<UserInfo>.Post(_client, "User", userInfo);

                Login(userInfo);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}