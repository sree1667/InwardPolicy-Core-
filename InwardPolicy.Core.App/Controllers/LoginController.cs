using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using InwardPolicy.Models;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace InwardPolicy.Core.App.Controllers
{
    public class LoginController : Controller
    {
        //[Route("/")]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CheckLogin(UsermasterModel objUserMasterModel)
        {
            
            HttpClient client = new HttpClient()
            {
                BaseAddress = new System.Uri("http://localhost:26317/")
            };
            using HttpResponseMessage httpResponseMessage = await client.PostAsJsonAsync($"/Api/ApiUserMaster/CheckLogin", objUserMasterModel.UserMaster);
            //using HttpResponseMessage httpResponseMessage = await client.GetAsync($"api/ApiTest/ApiTest");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var result = await httpResponseMessage.Content.ReadAsStringAsync();
                bool loginStatus = JsonConvert.DeserializeObject<bool>(result);
                if (loginStatus)
                {
                    return RedirectToAction("Dashboard", "Dashboard");
                }
                else
                {
                    return View("Login");
                }
                //bool Login = httpResponseMessage.Content.ReadAsb ReadAsBoolAsync().Result;
            }
            else
            {
                return View("Login");
            }
            
        }
    }
}
