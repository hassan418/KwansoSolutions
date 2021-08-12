using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Kwanso.WebFrontEnd.Models;
using Kwanso.Model.Poco;
using System.Net.Http;
using Newtonsoft.Json;
using Kwanso.Model.ViewModel;
using System.Text;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

namespace Kwanso.WebFrontEnd.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpGet("/list-tasks")]
        public async Task<IActionResult> ListTasks()
        
        {
            List<Tasks> tasks = new List<Tasks>();
            string token = await GetTokenAsync();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

                using (var response = await httpClient.GetAsync("https://localhost:44354/list-tasks"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    tasks = JsonConvert.DeserializeObject<List<Tasks>>(apiResponse);
                }
            }
            return View(tasks);
        }

        [HttpGet("/create-task")]
        public ViewResult CreateTasks() => View();

        [HttpPost("/create-task")]
        public async Task<IActionResult> CreateTasks(Tasks task)
        {
            Tasks task1 = new Tasks();
            string token = await GetTokenAsync();
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(task), Encoding.UTF8, "application/json");
                httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
                using (var response = await httpClient.PostAsync("https://localhost:44354/create-task", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    task1 = JsonConvert.DeserializeObject<Tasks>(apiResponse);
                }
            }
            return Redirect("/list-tasks");
        }

        [HttpGet("/bulk-delete")]
        public async Task<IActionResult> DeleteTasks()
        {
            List<Tasks> tasks = new List<Tasks>();
            string token = await GetTokenAsync();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

                using (var response = await httpClient.GetAsync("https://localhost:44354/list-tasks"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    tasks = JsonConvert.DeserializeObject<List<Tasks>>(apiResponse);
                }
            }
            return View(tasks);
        }

        [HttpPost("/bulk-delete")]
        public async Task<IActionResult> BulkDeleteTask(List<int> Id)
        {
            List<Tasks> task = new List<Tasks>();
            string token = await GetTokenAsync();
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(Id), Encoding.UTF8, "application/json");
                httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
                using (var response = await httpClient.PostAsync("https://localhost:44354/bulk-delete", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    task = JsonConvert.DeserializeObject<List<Tasks>>(apiResponse);
                }
            }
            return View(task);
        }

        private async Task<string> GetTokenAsync()
        {
            string token = "";
            token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
            {
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(new UserVm { Email = "user@domain.com", Password = "password" }), Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync("https://localhost:44354/login", content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        token = JsonConvert.DeserializeObject<string>(apiResponse);
                    }
                }
                HttpContext.Session.SetString("token", token);
            }
            return token;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
