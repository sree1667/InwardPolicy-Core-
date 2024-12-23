using BusinessEntity;
using InwardPolicy.Admin.Models;
using InwardPolicyHelper;
using InwardPolicy_ViewComponent;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace InwardPolicy.Core.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CodesMasterController : Controller
    {


        public IActionResult CodesMaster()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CodesMasterBind()
        {
            try
            {
                HttpClient client = new HttpClient()
                {
                    BaseAddress = new System.Uri("http://localhost:26317/")
                };

                using HttpResponseMessage httpResponseMessage = await client.GetAsync($"Api/ApiCodesMaster/CodesMasterBind");

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
                        (u.CM_CODE != null && u.CM_CODE.Contains(searchValue, StringComparison.OrdinalIgnoreCase)) ||
                        (u.CM_TYPE != null && u.CM_TYPE.ToString().Contains(searchValue, StringComparison.OrdinalIgnoreCase))).ToList();

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

        [HttpDelete]
        public async Task<IActionResult> DeleteCodesMaster(string code, string type)
        {
            try
            {
                HttpClient client = new HttpClient()
                {
                    BaseAddress = new System.Uri("http://localhost:26317/")
                };
                using HttpResponseMessage httpResponseMessage = await client.DeleteAsync($"/Api/ApiCodesMaster/DeleteCodesMaster/{code}/{type}");

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var result = await httpResponseMessage.Content.ReadAsStringAsync();
                    bool status = JsonConvert.DeserializeObject<bool>(result);
                    //swal
                    var message = ECMHelper.GetErrorMessage(status ? "204" : "101").Result;
                    var title = status ? "Success" : "Error";
                    var icon = status ? "success" : "error";
                    return Json(new { message, title, icon });


                }
                else
                {
                    
                    return Ok(false);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        [HttpGet]
        public async Task<IActionResult> ShowModal(string? userId)
        {
            try
            {
                await Task.CompletedTask;
                return ViewComponent(typeof(CodesMasterViewComponent));
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCodesMasterDetails(string code,string type)
        {
            try
            {
                CodesMasterModel objCodesMasterModel = new CodesMasterModel();
                objCodesMasterModel.CodesMaster = new CodesMaster();
                HttpClient client = new HttpClient()
                {
                    BaseAddress = new System.Uri("http://localhost:26317/")
                };
                using HttpResponseMessage httpResponseMessage = await client.GetAsync($"/Api/ApiCodesMaster/GetCodesMasterDetails/{code}/{type}");
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var result = await httpResponseMessage.Content.ReadAsStringAsync();
                    objCodesMasterModel.CodesMaster = JsonConvert.DeserializeObject<CodesMaster>(result);
                    objCodesMasterModel.Mode = "U";
                    return ViewComponent(typeof(CodesMasterViewComponent), objCodesMasterModel);
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
        
        [HttpGet]
        public async Task<IActionResult> CheckCodesMaster(string code,string type)
        {
            try
            {
                
                HttpClient client = new HttpClient()
                {
                    BaseAddress = new System.Uri("http://localhost:26317/")
                };
                using HttpResponseMessage httpResponseMessage = await client.GetAsync($"/Api/ApiCodesMaster/CheckCodesMaster/{code}/{type}");
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var result = await httpResponseMessage.Content.ReadAsStringAsync();
                    bool status = JsonConvert.DeserializeObject<bool>(result);
                    var message = ECMHelper.GetErrorMessage("304").Result;
                    var title = "Error";
                    var icon = "error";

                    return Json(new { message, title, icon });
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

        [HttpPost]
        public async Task<IActionResult> CodesMaster(CodesMasterModel objCodesMasterModel)
        {
            try
            {
                HttpClient client = new HttpClient()
                {
                    BaseAddress = new System.Uri("http://localhost:26317/")
                };
                objCodesMasterModel.CodesMaster.UpOrCrBy = HttpContext.Session.GetString("UserId");
                using HttpResponseMessage httpResponseMessage = await client.PostAsJsonAsync($"/Api/ApiCodesMaster/CodesMasterInsert/{objCodesMasterModel.Mode}", objCodesMasterModel.CodesMaster);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var result = await httpResponseMessage.Content.ReadAsStringAsync();
                    bool status = JsonConvert.DeserializeObject<bool>(result);
                    if (status)
                    {
                        string message = await ECMHelper.GetErrorMessage(objCodesMasterModel.Mode == "U" ? "203" : "202");
                        TempData["Message"] = message;
                        TempData["Title"] = "Success";
                        TempData["Icon"] = "success";
                        return View("CodesMaster", objCodesMasterModel);
                    }
                    else
                    {
                        TempData["Message"] = await ECMHelper.GetErrorMessage(objCodesMasterModel.Mode == "U" ? "105" : "102");
                        TempData["Title"] = "Error";
                        TempData["Icon"] = "error";
                        return View("CodesMaster", objCodesMasterModel);
                    }
                }
                else
                {
                    return View("Login");
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }
}
