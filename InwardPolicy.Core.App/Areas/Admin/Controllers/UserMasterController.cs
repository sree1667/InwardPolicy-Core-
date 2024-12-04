using BusinessEntity;
using InwardPolicy.Admin.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace InwardPolicy.Core.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserMasterController : Controller
    {
        public IActionResult UserMaster(string mode)
        {
            UserMasterModel model = new UserMasterModel();
            model.UserMaster = new UserMaster();
            model.Mode = "I";
            return View(model);
        }

        

        [HttpPost]
        public async Task<IActionResult> UserMasterBind()
        {
            try
            {
                HttpClient client = new HttpClient()
                {
                    BaseAddress = new System.Uri("http://localhost:26317/")
                };

                using HttpResponseMessage httpResponseMessage = await client.GetAsync($"Api/ApiUserMaster/UserMasterBind");

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
                    var data = ConvertDataTableToList(dt);

                    // Apply Search
                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        data = data.Where(u =>
                        (u.USER_NAME != null && u.USER_NAME.Contains(searchValue, StringComparison.OrdinalIgnoreCase)) ||
                        (u.USER_ID != null && u.USER_ID.ToString().Contains(searchValue))).ToList();

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
        public async Task<IActionResult> UserMaster(UserMasterModel objUserMasterModel)
        {
            try
            {
                HttpClient client = new HttpClient()
                {
                    BaseAddress = new System.Uri("http://localhost:26317/")
                };
                objUserMasterModel.UserMaster.UpOrCrBy= HttpContext.Session.GetString("UserId");
                using HttpResponseMessage httpResponseMessage = await client.PostAsJsonAsync($"/Api/ApiUserMaster/UserMasterInsert/{objUserMasterModel.Mode}", objUserMasterModel.UserMaster);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var result = await httpResponseMessage.Content.ReadAsStringAsync();
                    bool status = JsonConvert.DeserializeObject<bool>(result);
                    if (status)
                    {
                        if(objUserMasterModel.Mode == "U")
                        {
                            objUserMasterModel.Mode = "U";
                            return View("UserMaster", objUserMasterModel);
                        }
                        else
                        {
                            return RedirectToAction("UserMaster", "UserMaster", objUserMasterModel.Mode);
                        }
                        
                    }
                    else
                    {
                        return View("UserMaster", objUserMasterModel);
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
        
        [HttpDelete]
        public async Task<IActionResult> DeleteUserMaster(string userId)
        {
            try
            {
                HttpClient client = new HttpClient()
                {
                    BaseAddress = new System.Uri("http://localhost:26317/")
                };
                using HttpResponseMessage httpResponseMessage = await client.DeleteAsync($"/Api/ApiUserMaster/DeleteUserMaster/{userId}");

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var result = await httpResponseMessage.Content.ReadAsStringAsync();
                    bool status = JsonConvert.DeserializeObject<bool>(result);
                    TempData["Title"] = "success";
                    TempData["Message"] ="deleted" ;
                    TempData["Icon"] = "success";
                    return View("UserMaster");
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
        public async Task<IActionResult> GetUserMasterDetails(string userId)
        {
            try
            {
                UserMasterModel objUserMasterModel = new UserMasterModel();
                objUserMasterModel.UserMaster = new UserMaster();
                HttpClient client = new HttpClient()
                {
                    BaseAddress = new System.Uri("http://localhost:26317/")
                };
                using HttpResponseMessage httpResponseMessage = await client.GetAsync($"/Api/ApiUserMaster/GetUserMasterDetails/{userId}");
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var result = await httpResponseMessage.Content.ReadAsStringAsync();
                    DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                    //if (dt.Rows.Count > 0)
                    //{

                    //objUserMasterModel.UserMaster.UserId = dt.Rows[0]["USER_ID"] != DBNull.Value ? dt.Rows[0]["USER_ID"].ToString() : string.Empty;
                    //objUserMasterModel.UserMaster.UserName = dt.Rows[0]["USER_NAME"] != DBNull.Value ? dt.Rows[0]["USER_NAME"].ToString() : string.Empty;
                    //objUserMasterModel.UserMaster.Password = dt.Rows[0]["USER_PASSWORD"] != DBNull.Value ? dt.Rows[0]["USER_PASSWORD"].ToString() : string.Empty;
                    //if (dt.Rows[0]["USER_ACTIVE_YN"].ToString() == "Y")
                    //{
                    //    objUserMasterModel.UserMaster.IsActiveYN = true;
                    //}
                    //else
                    //{
                    //    objUserMasterModel.UserMaster.IsActiveYN = false;
                    //}
                    //}

                    //return View("UserMaster", objUserMasterModel);
                    var json = JsonConvert.SerializeObject(dt);
                    return Ok(json);
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

        public static List<dynamic> ConvertDataTableToList(DataTable pDataTable)
        {

            var data = new List<dynamic>();
            if (pDataTable != null)
            {
                foreach (DataRow item in pDataTable.Rows)
                {
                    IDictionary<string, object> dn = new ExpandoObject();

                    foreach (var column in pDataTable.Columns.Cast<DataColumn>())
                    {
                        dn[column.ColumnName] = item[column];
                    }
                    data.Add(dn);
                }
            }

            return data;
        }
    }

}
