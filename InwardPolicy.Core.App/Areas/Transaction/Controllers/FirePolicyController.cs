using BusinessEntity;
using InwardPolicy.Transaction.Models;
using Microsoft.AspNetCore.Http;
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
    public class FirePolicyController : Controller
    {
        public async Task<IActionResult> FirePolicy(string id1)
        {
            
            HttpClient client = new HttpClient()
            {
                BaseAddress = new System.Uri("http://localhost:26317/")
            };
            //ddl
            FirePolicyModel objFirePolicyModel = new FirePolicyModel();
            if (!string.IsNullOrEmpty(id1))
            {
                using HttpResponseMessage httpResponseMessage1 = await client.GetAsync($"Api/ApiFirePolicy/LoadControl/{id1}");
                
                if (httpResponseMessage1.IsSuccessStatusCode)
                {
                    var result = await httpResponseMessage1.Content.ReadAsStringAsync();
                    objFirePolicyModel.FirePolicy = JsonConvert.DeserializeObject<FirePolicy>(result);


                }
            }
            using HttpResponseMessage httpResponseMessage = await client.GetAsync("Api/ApiCodesMaster/FetchDropdownList");
          
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var result = await httpResponseMessage.Content.ReadAsStringAsync();
                objFirePolicyModel = FillDropDowns(result, objFirePolicyModel);


            }


            return View(objFirePolicyModel);
        }

        private FirePolicyModel FillDropDowns(string result,FirePolicyModel objFirePolicyModel)
        {

            //objFirePolicyModel = new FirePolicyModel()
            //{
            //    PolCurrencyList = new List<SelectListItem>(),
            //    PolOccupationList = new List<SelectListItem>(),
            //    PolProductCodeList = new List<SelectListItem>(),
            //    PolAssuredTypeList = new List<SelectListItem>()
            //};
            objFirePolicyModel.PolCurrencyList = new List<SelectListItem>();
            objFirePolicyModel.PolOccupationList = new List<SelectListItem>();
            objFirePolicyModel.PolProductCodeList = new List<SelectListItem>();
            objFirePolicyModel.PolAssuredTypeList = new List<SelectListItem>();

            DataSet ds = JsonConvert.DeserializeObject<DataSet>(result);
            var tableMapping = new Dictionary<string, List<SelectListItem>>
            {
                { "Currency", objFirePolicyModel.PolCurrencyList },
                { "OCCUPATION", objFirePolicyModel.PolOccupationList },
                { "PRODUCT CODE", objFirePolicyModel.PolProductCodeList },
                { "ASSURED TYPE", objFirePolicyModel.PolAssuredTypeList }
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
            return objFirePolicyModel;
        }

        [HttpGet]
        public async Task<IActionResult> SetToDate(string? fromDate)
        {
            try
            {

                if (!DateTime.TryParse(fromDate, out DateTime parsedDate))
                {
                    return Ok(null);
                }
                DateTime today = DateTime.Today;
                DateTime FromDate = Convert.ToDateTime(fromDate);
                DateTime maxDate = today.AddYears(50);
                if (FromDate < today)
                {

                    return Ok(null);
                }
                if (FromDate > maxDate)
                {

                    return Ok(null);
                }
                DateTime toDate = FromDate.AddYears(1).AddDays(-1);
                return Ok(toDate.ToString("d"));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpGet]
        public async Task<IActionResult> ValidateDoB(string? dob)
        {
            try
            {
                if (!DateTime.TryParse(dob, out DateTime parsedDate))
                {
                    return Ok(null);
                }
                DateTime today = DateTime.Today;
                if (parsedDate > today)
                {
                    return Ok(null);
                }
                int age = today.Year - parsedDate.Year;
                if (parsedDate.Date > today.AddYears(-age)) age--;
                if (age < 18 || age > 120)
                {
                    return Ok(null);
                }
                return Ok(parsedDate.ToString("d"));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetCurrencyRate(string? value)
        {
            try
            {
                HttpClient client = new HttpClient()
                {
                    BaseAddress = new System.Uri("http://localhost:26317/")
                };
                using HttpResponseMessage httpResponseMessage = await client.GetAsync($"Api/ApiCodesMaster/GetCurrencyRate/{value}");
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var result = await httpResponseMessage.Content.ReadAsStringAsync();
                    string rate = JsonConvert.DeserializeObject<string>(result);
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
        [HttpPost]
        public async Task<IActionResult> FirePolicy(FirePolicyModel objFirePolicyModel)
        {

            try
            {
                HttpClient client = new HttpClient()
                {
                    BaseAddress = new System.Uri("http://localhost:26317")
                };
                objFirePolicyModel.FirePolicy.CrOrUpBy= HttpContext.Session.GetString("UserId");
                using HttpResponseMessage httpResponseMessage = await client.PostAsJsonAsync($"/Api/ApiFirePolicy/AddFirePolicy/{objFirePolicyModel.Mode}", objFirePolicyModel);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return View();
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}
