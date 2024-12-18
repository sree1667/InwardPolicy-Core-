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
        public async Task<IActionResult> FirePolicy(string id1,string id2)
        {
            
            HttpClient client = new HttpClient()
            {
                BaseAddress = new System.Uri("http://localhost:26317/")
            };
            //ddl
            FirePolicyModel objFirePolicyModel = new FirePolicyModel();
            objFirePolicyModel.FirePolicy = new FirePolicy();
            objFirePolicyModel.FireInwardPolicy = new FireInwardPolicy();
            using HttpResponseMessage httpResponseMessage = await client.GetAsync("Api/ApiCodesMaster/FetchDropdownList");
            //ddl fetch
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var result = await httpResponseMessage.Content.ReadAsStringAsync();
                objFirePolicyModel = FillDropDowns(result, objFirePolicyModel);


            }
            //control data
            if (!string.IsNullOrEmpty(id1))
            {
                objFirePolicyModel.Mode = "U";
                objFirePolicyModel.FirePolicy.Poluid = Convert.ToInt32(id1);
                using HttpResponseMessage httpResponseMessage1 = await client.GetAsync($"Api/ApiFirePolicy/LoadControl/{id1}");
                //load policy control
                if (httpResponseMessage1.IsSuccessStatusCode)
                {
                    var result = await httpResponseMessage1.Content.ReadAsStringAsync();
                    objFirePolicyModel.FirePolicy = JsonConvert.DeserializeObject<FirePolicy>(result);
                    
                    if (objFirePolicyModel.FireInwardPolicy != null && objFirePolicyModel.FirePolicy != null)
                    {
                        objFirePolicyModel.FireInwardPolicy.InwPremCurr = objFirePolicyModel.FirePolicy.PolPremCurrency;
                        objFirePolicyModel.FireInwardPolicy.InwSiCurr = objFirePolicyModel.FirePolicy.PolSICurrency;
                    }
                    //check if there is any inward
                    if (objFirePolicyModel.FirePolicy.InwCount == 1)
                    {
                        objFirePolicyModel.InwardMode = "U";
                        using HttpResponseMessage httpResponseMessageInward = await client.GetAsync($"Api/ApiFireInwardPolicy/LoadInwardControl/{id1}");
                        //INWARD LOAD
                        if (httpResponseMessageInward.IsSuccessStatusCode)
                        {
                            var inwardDetails = await httpResponseMessageInward.Content.ReadAsStringAsync();
                            objFirePolicyModel.FireInwardPolicy = new FireInwardPolicy();
                            objFirePolicyModel.FireInwardPolicy = JsonConvert.DeserializeObject<FireInwardPolicy>(inwardDetails);
                        }
                    }
                    else
                    {
                        objFirePolicyModel.InwardMode = "I";
                    }
                }
                objFirePolicyModel.FirePolicy.Poluid = Convert.ToInt32(id1);
            }
            else
            {
                objFirePolicyModel.Mode = "I";
            }
                
            if (id2=="A")
            {
                objFirePolicyModel.ApprStatus = "A";
            }
            else
            {
                objFirePolicyModel.ApprStatus = "N";

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
            objFirePolicyModel.CedingSourceList = new List<SelectListItem>();
            objFirePolicyModel.RiskClassList = new List<SelectListItem>();

            DataSet ds = JsonConvert.DeserializeObject<DataSet>(result);
            var tableMapping = new Dictionary<string, List<SelectListItem>>
            {
                { "Currency", objFirePolicyModel.PolCurrencyList },
                { "OCCUPATION", objFirePolicyModel.PolOccupationList },
                { "PRODUCT CODE", objFirePolicyModel.PolProductCodeList },
                { "RISK CLASS", objFirePolicyModel.RiskClassList },
                { "CEDINGSOURCE", objFirePolicyModel.CedingSourceList },
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
        public async Task<IActionResult> DeleteFirePolicyRisk(string? riskUid,string polUid)
        {
            try
            {
                HttpClient client = new HttpClient()
                {
                    BaseAddress = new System.Uri("http://localhost:26317/")
                };
                using HttpResponseMessage httpResponseMessage = await client.DeleteAsync($"Api/ApiFirePolicyRisk/DeleteFirePolicyRisk/{riskUid}/{polUid}");
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var result = await httpResponseMessage.Content.ReadAsStringAsync();
                    bool status = JsonConvert.DeserializeObject<bool>(result);
                    return Ok(status);

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
        public async Task<IActionResult> FirePolicy(FirePolicyModel objFirePolicyModel,string id1)
        {

            try
            {
                
                HttpClient client = new HttpClient()
                {
                    BaseAddress = new System.Uri("http://localhost:26317")
                };
                if (!string.IsNullOrEmpty(id1))
                {
                    objFirePolicyModel.FirePolicy.Poluid = Convert.ToInt32(id1);
                }
                objFirePolicyModel.FirePolicy.CrOrUpBy= HttpContext.Session.GetString("UserId");
                using HttpResponseMessage httpResponseMessage = await client.PostAsJsonAsync($"/Api/ApiFirePolicy/AddFirePolicy/{objFirePolicyModel.Mode}", objFirePolicyModel.FirePolicy);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    TempData["SwalTitle"] = "Success!";
                    TempData["SwalMessage"] = "Your operation was completed successfully.";
                    TempData["SwalIcon"] = "success";
                    var result = await httpResponseMessage.Content.ReadAsStringAsync();
                    string uid = JsonConvert.DeserializeObject<string>(result);
                    return  RedirectToAction("FirePolicy", new { id1 = $"{uid}", id2 = "N" });
                    
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
        
        [HttpPost]
        public async Task<IActionResult> FireInwardPolicy(FirePolicyModel objFirePolicyModel)
        {

            try
            {
                
                    objFirePolicyModel.FireInwardPolicy.InwPolUid = objFirePolicyModel.FirePolicy.Poluid.ToString();
                
                
                HttpClient client = new HttpClient()
                {
                    BaseAddress = new System.Uri("http://localhost:26317")
                };
                //objFirePolicyModel.FirePolicy.CrOrUpBy= HttpContext.Session.GetString("UserId");
                //objFirePolicyModel.FirePolicy.CrOrUpBy = HttpContext.Session.GetString("UserId");
                using HttpResponseMessage httpResponseMessage = await client.PostAsJsonAsync($"/Api/ApiFireInwardPolicy/AddFireInwardPolicy/{objFirePolicyModel.InwardMode}", objFirePolicyModel.FireInwardPolicy);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    
                    var result = await httpResponseMessage.Content.ReadAsStringAsync();
                    objFirePolicyModel.FireInwardPolicy = JsonConvert.DeserializeObject<FireInwardPolicy>(result);
                    if (!string.IsNullOrEmpty(result))
                    {
                        TempData["SwalTitle"] = "Success!";
                        TempData["SwalMessage"] = "Your operation was completed successfully.";
                        TempData["SwalIcon"] = "success";
                    }

                    return  RedirectToAction("FirePolicy", new { id1 = $"{objFirePolicyModel.FirePolicy.Poluid}", id2 = "N" });
                    
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

        [HttpPost]
        public async Task<IActionResult> FirePolicyRiskBind(string poluid)
        {
            try
            {
                HttpClient client = new HttpClient()
                {
                    BaseAddress = new System.Uri("http://localhost:26317/")
                };

                using HttpResponseMessage httpResponseMessage = await client.GetAsync($"Api/ApiFirePolicyRisk/FirePolicyRiskBind/{poluid}");

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var orderColumnIndex = Request.Form["order[0][column]"].FirstOrDefault(); // Get column index
                    var orderDirection = Request.Form["order[0][dir]"].FirstOrDefault();

                    var draw = Request.Form["draw"].FirstOrDefault();
                    var start = Request.Form["start"].FirstOrDefault();
                    var length = Request.Form["length"].FirstOrDefault();
                    var searchValue = Request.Form["search[value]"].FirstOrDefault();

                    int pageSize = length != null ? Convert.ToInt32(length) : 5;
                    int skip = start != null ? Convert.ToInt32(start) : 0;

                    // Fetch data from the API
                    var result = await httpResponseMessage.Content.ReadAsStringAsync();
                    DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);

                    // Convert DataTable to List of objects
                    var data = Helper.ConvertDataTableToList(dt);

                    // Apply Search
                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        data = data.Where(u =>
                        (u.USER_NAME != null && u.USER_NAME.Contains(searchValue, StringComparison.OrdinalIgnoreCase))).ToList();

                    }



                    // Count of filtered records
                    var recordsFiltered = data.Count;

                    // Apply Pagination
                    var pagedData = data.Skip(skip).Take(pageSize).ToList();

                    // Prepare the response
                    var dataTableResponse = new
                    {
                        draw = draw,
                        recordsTotal = dt.Rows.Count, // Total records in the DataTable (unfiltered)
                        recordsFiltered = recordsFiltered, // Total records after filtering
                        data = pagedData // Paginated and filtered data
                    };

                    return Ok(dataTableResponse);

                }
                else
                {
                    return StatusCode((int)httpResponseMessage.StatusCode, "Error retrieving data");
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        [HttpPost]
        public async Task<IActionResult> FireInwPolicyBind(string poluid)
        {
            try
            {
                HttpClient client = new HttpClient()
                {
                    BaseAddress = new System.Uri("http://localhost:26317/")
                };

                using HttpResponseMessage httpResponseMessage = await client.GetAsync($"Api/ApiFireInwardPolicy/FireInwardPolicyBind/{poluid}");

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var orderColumnIndex = Request.Form["order[0][column]"].FirstOrDefault(); // Get column index
                    var orderDirection = Request.Form["order[0][dir]"].FirstOrDefault();

                    var draw = Request.Form["draw"].FirstOrDefault();
                    var start = Request.Form["start"].FirstOrDefault();
                    var length = Request.Form["length"].FirstOrDefault();
                    var searchValue = Request.Form["search[value]"].FirstOrDefault();

                    int pageSize = length != null ? Convert.ToInt32(length) : 5;
                    int skip = start != null ? Convert.ToInt32(start) : 0;

                    // Fetch data from the API
                    var result = await httpResponseMessage.Content.ReadAsStringAsync();
                    DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);

                    // Convert DataTable to List of objects
                    var data = Helper.ConvertDataTableToList(dt);

                    // Apply Search
                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        data = data.Where(u =>
                        (u.USER_NAME != null && u.USER_NAME.Contains(searchValue, StringComparison.OrdinalIgnoreCase))).ToList();

                    }



                    // Count of filtered records
                    var recordsFiltered = data.Count;

                    // Apply Pagination
                    var pagedData = data.Skip(skip).Take(pageSize).ToList();

                    // Prepare the response
                    var dataTableResponse = new
                    {
                        draw = draw,
                        recordsTotal = dt.Rows.Count, // Total records in the DataTable (unfiltered)
                        recordsFiltered = recordsFiltered, // Total records after filtering
                        data = pagedData // Paginated and filtered data
                    };

                    return Ok(dataTableResponse);

                }
                else
                {
                    return StatusCode((int)httpResponseMessage.StatusCode, "Error retrieving data");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
