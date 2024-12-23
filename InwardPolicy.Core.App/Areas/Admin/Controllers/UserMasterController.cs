﻿using BusinessEntity;
using InwardPolicy.Admin.Models;
using InwardPolicy_ViewComponent;
using InwardPolicyHelper;
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

        [HttpGet]
        public async Task<IActionResult> ShowModal(string? userId)
        {
            try
            {
                await Task.CompletedTask;
                return ViewComponent(typeof(UserMasterViewComponent));
            }
            catch (Exception ex)
            {

                throw;
            }
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

                    int pageSize = length != null ? Convert.ToInt32(length) : 5;
                    int skip = start != null ? Convert.ToInt32(start) : 0;

                    // Fetch data from the API
                    var result = await httpResponseMessage.Content.ReadAsStringAsync();
                    DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);

                    // Convert DataTable to List of objects
                    var data = Helper.ConvertDataTableToList(dt);

                    var searchValue = Request.Form["search[value]"].FirstOrDefault();
                    // Apply Search
                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        data = data.Where(u =>
                        (u.USER_NAME != null && u.USER_NAME.Contains(searchValue, StringComparison.OrdinalIgnoreCase)) ||
                        (u.USER_DESC != null && u.USER_DESC.Contains(searchValue, StringComparison.OrdinalIgnoreCase)) ||
                        (u.USER_ID != null && u.USER_ID.ToString().Contains(searchValue, StringComparison.OrdinalIgnoreCase))).ToList();

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
                        string message = await ECMHelper.GetErrorMessage(objUserMasterModel.Mode == "U" ? "203" : "202");
                        TempData["Message"] = message;
                        TempData["Title"] = "Success";
                        TempData["Icon"] = "success";
                        return View("UserMaster");
                    }
                    else
                    {
                        TempData["Message"] = await ECMHelper.GetErrorMessage(objUserMasterModel.Mode == "U" ? "105" : "102");
                        TempData["Title"] = "Error";
                        TempData["Icon"] = "error";
                        return View("UserMaster");
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

                    var message = ECMHelper.GetErrorMessage(status ? "204" : "101").Result;
                    var title = status ? "Success" : "Error";
                    var icon = status ? "success" : "error";

                    return Json(new { message, title, icon });
                }
                else
                {
                    return View("UserMaster");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> CheckUserId(string userId)
        {
            try
            {
                HttpClient client = new HttpClient()
                {
                    BaseAddress = new System.Uri("http://localhost:26317/")
                };
                using HttpResponseMessage httpResponseMessage = await client.PostAsJsonAsync($"/Api/ApiUserMaster/CheckUserId/{userId}", userId);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var result = await httpResponseMessage.Content.ReadAsStringAsync();
                    bool status = JsonConvert.DeserializeObject<bool>(result);

                    var message = ECMHelper.GetErrorMessage("306").Result;
                    var title =  "Error";
                    var icon =  "error";

                    return Json(new { message, title, icon });
                }
                else
                {
                    return Ok(false);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> GetUserMasterDetails(string userId)
        {
            try
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
                        objUserMasterModel.UserMaster = JsonConvert.DeserializeObject<UserMaster>(result);
                        objUserMasterModel.Mode = "U";
                        return ViewComponent(typeof(UserMasterViewComponent), objUserMasterModel);
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
            catch (Exception ex)
            {

                throw ex;
            }
        }

        
    }

}
