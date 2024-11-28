using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace InwardPolicy.Core.App.Controllers
{
    public class DashboardController : Controller
    {
        //public IActionResult Dashboard()
        //{
        //    LoadDashBoard();
        //    return View();
        //}

        private async Task<IActionResult> Dashboard()
        {
            HttpClient client = new HttpClient()
            {
                BaseAddress = new System.Uri("http://localhost:26317/")
            };
            using HttpResponseMessage httpResponseMessage = await client.GetAsync($"api/ApiTest/ApiTest");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var result = await httpResponseMessage.Content.ReadAsStringAsync();
                DataRow loginStatus = JsonConvert.DeserializeObject<DataRow>(result);
            }
            else
            {
                return View("Dashboard");
            }
        }
    }
}
