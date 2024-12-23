using InwardPolicy.BusinessEntity;
using InwardPolicy.Models;
using InwardPolicy_ViewComponent;
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

namespace InwardPolicy.Core.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ErrorCodeMasterController : Controller
    {
        public IActionResult ErrorCodeMaster()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ErrorCodeMasterBind()
        {
            try
            {
                HttpClient client = new HttpClient()
                {
                    BaseAddress = new System.Uri("http://localhost:26317/")
                };

                using HttpResponseMessage httpResponseMessage = await client.GetAsync($"Api/ApiErrorCodeMaster/ErrorCodeMasterBind");

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
                        (u.ERR_CODE != null && u.ERR_CODE.Contains(searchValue, StringComparison.OrdinalIgnoreCase)) ||
                        (u.ERR_TYPE != null && u.ERR_TYPE.ToString().Contains(searchValue, StringComparison.OrdinalIgnoreCase))).ToList();

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
        [HttpGet]
        public async Task<IActionResult> GetErrorCodeMasterDetails(string code)
        {
            try
            {
                ErrorCodeMasterModel objErrorCodeMasterModel = new ErrorCodeMasterModel();
                objErrorCodeMasterModel.ErrorCodeMaster = new ErrorCodeMaster();
                HttpClient client = new HttpClient()
                {
                    BaseAddress = new System.Uri("http://localhost:26317/")
                };
                using HttpResponseMessage httpResponseMessage = await client.GetAsync("Api/ApiCodesMaster/FetchErrorDropdownList");
                //ddl fetch
               //ErrorCodeMasterModel objErrorCodeMasterModel = new ErrorCodeMasterModel();
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var result = await httpResponseMessage.Content.ReadAsStringAsync();
                    objErrorCodeMasterModel = FillDropDowns(result, objErrorCodeMasterModel);

                }
                using HttpResponseMessage ErrDetails = await client.GetAsync($"/Api/ApiErrorCodeMaster/GetErrorCodeMasterDetails/{code}");
                if (ErrDetails.IsSuccessStatusCode)
                {
                    var result = await ErrDetails.Content.ReadAsStringAsync();
                    objErrorCodeMasterModel.ErrorCodeMaster = JsonConvert.DeserializeObject<ErrorCodeMaster>(result);
                    objErrorCodeMasterModel.Mode = "U";
                    return ViewComponent(typeof(ErrorCodeMasterViewComponent), objErrorCodeMasterModel);
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
        public async Task<IActionResult> CheckErrorCodeMaster(string code)
        {
            try
            {

                HttpClient client = new HttpClient()
                {
                    BaseAddress = new System.Uri("http://localhost:26317/")
                };
                using HttpResponseMessage httpResponseMessage = await client.GetAsync($"/Api/ApiErrorCodeMaster/CheckErrorCodeMaster/{code}");
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var result = await httpResponseMessage.Content.ReadAsStringAsync();
                    bool status = JsonConvert.DeserializeObject<bool>(result);
                    return Ok(status);
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
        [HttpDelete]
        public async Task<IActionResult> DeleteErrorCodeMaster(string code)
        {
            try
            {
                HttpClient client = new HttpClient()
                {
                    BaseAddress = new System.Uri("http://localhost:26317/")
                };
                using HttpResponseMessage httpResponseMessage = await client.DeleteAsync($"/Api/ApiErrorCodeMaster/DeleteErrorCodeMaster/{code}");
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var result = await httpResponseMessage.Content.ReadAsStringAsync();
                    bool status = JsonConvert.DeserializeObject<bool>(result);
                    return Ok(status);
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
        [HttpPost]
        public async Task<IActionResult> ErrorCodeMaster(ErrorCodeMasterModel objErrorCodeMasterModel)
        {
            try
            {
                HttpClient client = new HttpClient()
                {
                    BaseAddress = new System.Uri("http://localhost:26317/")
                };
                objErrorCodeMasterModel.ErrorCodeMaster.UpOrCrBy = HttpContext.Session.GetString("UserId");
                using HttpResponseMessage httpResponseMessage = await client.PostAsJsonAsync($"/Api/ApiErrorCodeMaster/ErrorCodeMasterInsert/{objErrorCodeMasterModel.Mode}", objErrorCodeMasterModel.ErrorCodeMaster);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var result = await httpResponseMessage.Content.ReadAsStringAsync();
                    bool status = JsonConvert.DeserializeObject<bool>(result);
                    if (status)
                    {
                        TempData["SwalTitle"] = "Success!";
                        TempData["SwalMessage"] = "Inserted Sucessfully.";
                        TempData["SwalIcon"] = "success";
                        return View("ErrorCodeMaster", objErrorCodeMasterModel);
                    }
                    else
                    {
                        return View("ErrorCodeMaster", objErrorCodeMasterModel);
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
        [HttpGet]
        public async Task<IActionResult> ShowModal(string? userId)
        {
            try
            {
                HttpClient client = new HttpClient()
                {
                    BaseAddress = new System.Uri("http://localhost:26317/")
                };
                using HttpResponseMessage httpResponseMessage = await client.GetAsync("Api/ApiCodesMaster/FetchErrorDropdownList");
                //ddl fetch
                ErrorCodeMasterModel objErrorCodeMasterModel = new ErrorCodeMasterModel();
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var result = await httpResponseMessage.Content.ReadAsStringAsync();
                    objErrorCodeMasterModel = FillDropDowns(result, objErrorCodeMasterModel);

                }
                await Task.CompletedTask;
                return ViewComponent(typeof(ErrorCodeMasterViewComponent), objErrorCodeMasterModel);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private ErrorCodeMasterModel FillDropDowns(string result, ErrorCodeMasterModel objErrorCodeMasterModel)
        {
            objErrorCodeMasterModel.ErrTypeList = new List<SelectListItem>();


            DataSet ds = JsonConvert.DeserializeObject<DataSet>(result);
            var tableMapping = new Dictionary<string, List<SelectListItem>>
            {
                { "ERROR TYPE", objErrorCodeMasterModel.ErrTypeList }

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
            return objErrorCodeMasterModel;
        }
    }
}
