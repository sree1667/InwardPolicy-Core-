using BusinessEntity;
using InwardPolicy.Models;
using InwardPolicyHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace InwardPolicy.Core.App.Areas.Transaction.Controllers
{
    [Area("Transaction")]
    public class FirePolicyRiskController : Controller
    {
        public async Task<IActionResult> FirePolicyRisk(string id1, string id2)
        {
            HttpClient client = new HttpClient()
            {
                BaseAddress = new System.Uri("http://localhost:26317/")
            };
            
            bool approvalstatus = false;
            //check approvr status
            using HttpResponseMessage apprstatus = await client.GetAsync($"Api/ApiFirepolicy/CheckApprovalStatus/{id1}");
            if (apprstatus.IsSuccessStatusCode)
            {
                var result = await apprstatus.Content.ReadAsStringAsync();
                approvalstatus = JsonConvert.DeserializeObject<bool>(result);
            }
            //ddl
            FirePolicyRiskModel objFirePolicyRiskModel = new FirePolicyRiskModel();
            objFirePolicyRiskModel.FirePolicyRisk = new FirePolicyRisk();

            //curency load
            using HttpResponseMessage currencyMessage = await client.GetAsync($"Api/ApiFirePolicyRisk/FetchPolRiskDetails/{id1}");
            if (currencyMessage.IsSuccessStatusCode)
            {
                var result = await currencyMessage.Content.ReadAsStringAsync();
                objFirePolicyRiskModel.FirePolicyRisk = JsonConvert.DeserializeObject<FirePolicyRisk>(result);
            }




            using HttpResponseMessage httpResponseMessage = await client.GetAsync($"Api/ApiCodesMaster/FetchPolicyRiskDropdownList/{id1}");

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var result = await httpResponseMessage.Content.ReadAsStringAsync();
                objFirePolicyRiskModel = FillDropDowns(result, objFirePolicyRiskModel);


            }
            //control data
            if (!string.IsNullOrEmpty(id2))
            {
                objFirePolicyRiskModel.Mode = "U";
                objFirePolicyRiskModel.FirePolicyRisk.RiskPolUid = id1;
                using HttpResponseMessage httpResponseMessage1 = await client.GetAsync($"Api/ApiFirePolicyRisk/FetchRiskDetails/{id2}");
                if (httpResponseMessage1.IsSuccessStatusCode)
                {
                    var result = await httpResponseMessage1.Content.ReadAsStringAsync();
                    objFirePolicyRiskModel.FirePolicyRisk = JsonConvert.DeserializeObject<FirePolicyRisk>(result);
                }
            }
            else
            {
                objFirePolicyRiskModel.Mode = "I";
            }

            if (approvalstatus)
            {
                objFirePolicyRiskModel.ApprStatus = "A";
            }
            else
            {
                objFirePolicyRiskModel.ApprStatus = "N";

            }
            return View(objFirePolicyRiskModel);
        }
        [HttpPost]
        public async Task<IActionResult> FirePolicyRisk(FirePolicyRiskModel objFirePolicyRiskModel, string id1, string id2)
        {
            if (!string.IsNullOrEmpty(id2))
            {
                objFirePolicyRiskModel.Mode = "U";
                objFirePolicyRiskModel.FirePolicyRisk.RiskUid = id2;
                
            }
            objFirePolicyRiskModel.FirePolicyRisk.RiskPolUid = id1;
            HttpClient client = new HttpClient()
            {
                BaseAddress = new System.Uri("http://localhost:26317/")
            };
            using HttpResponseMessage httpResponseMessage = await client.PostAsJsonAsync($"Api/ApiFirePolicyRisk/AddRisk/{objFirePolicyRiskModel.Mode}", objFirePolicyRiskModel.FirePolicyRisk);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var result = await httpResponseMessage.Content.ReadAsStringAsync();
                objFirePolicyRiskModel.FirePolicyRisk = JsonConvert.DeserializeObject<FirePolicyRisk>(result);
                if (!string.IsNullOrEmpty(objFirePolicyRiskModel.FirePolicyRisk.RiskUid))
                {
                    string message = await ECMHelper.GetErrorMessage(objFirePolicyRiskModel.Mode == "U" ? "203" : "202");
                    TempData["Message"] = message;
                    TempData["Title"] = "Success";
                    TempData["Icon"] = "success";
                    
                }
                else
                {
                    TempData["Message"] = await ECMHelper.GetErrorMessage(objFirePolicyRiskModel.Mode == "U" ? "105" : "102");
                    TempData["Title"] = "Error";
                    TempData["Icon"] = "error";
                    
                }
                return RedirectToAction("FirePolicyRisk", new { id2 = objFirePolicyRiskModel.FirePolicyRisk.RiskUid });

            }
            else
            {
                return Ok();
            }
        }
        private FirePolicyRiskModel FillDropDowns(string result, FirePolicyRiskModel objFirePolicyRiskModel)
        {
            objFirePolicyRiskModel.RiskClass = new List<SelectListItem>();
            objFirePolicyRiskModel.RiskCurrency = new List<SelectListItem>();
            //objFirePolicyRiskModel.RiskPremCurrency = new List<SelectListItem>();


            DataSet ds = JsonConvert.DeserializeObject<DataSet>(result);
            var tableMapping = new Dictionary<string, List<SelectListItem>>
            {
                //{ "Currency", objFirePolicyRiskModel.RiskClass },
                { "Currency", objFirePolicyRiskModel.RiskCurrency },
                { "RISK CLASS", objFirePolicyRiskModel.RiskClass },


            };
            foreach (var mapping in tableMapping)
            {
                string tableName = mapping.Key;
                List<SelectListItem> targetList = mapping.Value;
                targetList.Add(new SelectListItem { Text = "---SELECT---", Value = "0" });
                if (ds.Tables.Contains(tableName))
                {
                    DataTable dt = ds.Tables[tableName];
                    foreach (DataRow dataRow in dt.Rows)
                    {
                        targetList.Add(new SelectListItem
                        {
                            Text = dataRow["TEXT"]?.ToString() ?? string.Empty,
                            Value = dataRow["CODE"]?.ToString() ?? string.Empty
                        });
                    }
                }
            }
            return objFirePolicyRiskModel;

        }
        [HttpGet]
        public async Task<IActionResult> GetCmValue(string? code)
        {
            try
            {
                HttpClient client = new HttpClient()
                {
                    BaseAddress = new System.Uri("http://localhost:26317/")
                };
                using HttpResponseMessage httpResponseMessage = await client.GetAsync($"Api/ApiCodesMaster/GetRiskPremium/{code}");
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var result = await httpResponseMessage.Content.ReadAsStringAsync();
                    int rate = JsonConvert.DeserializeObject<int>(result);
                    return Ok(rate);
                }
                else
                {
                    return Ok();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpGet]
        public async Task<IActionResult> FillAll(string? poluid)
        {
            try
            {
                HttpClient client = new HttpClient()
                {
                    BaseAddress = new System.Uri("http://localhost:26317/")
                };
                using HttpResponseMessage httpResponseMessage = await client.GetAsync($"Api/ApiFirePolicy/FillAll/{poluid}");
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var result = await httpResponseMessage.Content.ReadAsStringAsync();
                    string[] rate = JsonConvert.DeserializeObject<string[]>(result);
                    return Ok(rate);
                }
                else
                {
                    return Ok();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}
