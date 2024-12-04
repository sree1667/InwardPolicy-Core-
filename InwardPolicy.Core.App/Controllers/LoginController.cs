using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using InwardPolicy.Models;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;

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

            try
            {
                HttpClient client = new HttpClient()
                {
                    BaseAddress = new System.Uri("http://localhost:26317/")
                };
                using HttpResponseMessage httpResponseMessage = await client.PostAsJsonAsync($"/Api/ApiUserMaster/CheckLogin/{objUserMasterModel.UserId}/{objUserMasterModel.Password}", objUserMasterModel);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var result = await httpResponseMessage.Content.ReadAsStringAsync();
                    bool loginStatus = JsonConvert.DeserializeObject<bool>(result);
                    if (loginStatus)
                    {
                        HttpContext.Session.SetString("UserId", objUserMasterModel.UserId);
                        return RedirectToAction("Dashboard", "Dashboard");
                    }
                    else
                    {
                        return View("Login");
                    }
                }
                else
                {
                    return View("Login");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }
    }
}
