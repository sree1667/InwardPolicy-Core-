using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using InwardPolicy.Models;
using System.Threading.Tasks;

namespace InwardPolicy.Core.App.Controllers
{
    public class DashboardController : Controller
    {
        public async Task<IActionResult> Dashboard()
        {
            DashboardModel objdashboardModel = new();
            objdashboardModel = await LoadDashBoard();
            return View(objdashboardModel);
        }


        private async Task<DashboardModel> LoadDashBoard()
        {
            try
            {
                HttpClient client = new HttpClient()
                {
                    BaseAddress = new System.Uri("http://localhost:26317/")
                };
                using HttpResponseMessage httpResponseMessage = await client.GetAsync($"Api/ApiFirePolicy/LoadDashboardData");
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var result = await httpResponseMessage.Content.ReadAsStringAsync();
                    DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                    DashboardModel objdashboardModel = new()
                    {
                        TotalPolicy = dt.Rows[0]["TOTAL_POLICY"].ToString(),
                        TotalAmount = dt.Rows[0]["TOTAL_SI"].ToString(),
                        ApprovedPolicy = dt.Rows[0]["TOTAL_APPROVED"].ToString()
                    };
                    return objdashboardModel;

                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpGet]
        public async Task<IActionResult> LoadChart()
        {
            try
            {
                HttpClient client = new HttpClient()
                {
                    BaseAddress = new System.Uri("http://localhost:26317/")
                };
                using HttpResponseMessage httpResponseMessage = await client.GetAsync($"Api/ApiFirePolicy/GetPolicyCount");
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var result = await httpResponseMessage.Content.ReadAsStringAsync();
                    int[] policyCount = JsonConvert.DeserializeObject<int[]>(result);
                    
                    return Ok(policyCount);

                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}


