using InwardPolicy_ViewComponent;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
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

        [HttpDelete]
        public async Task<IActionResult> DeleteUserMaster(string userId)
        {
            try
            {
                HttpClient client = new HttpClient()
                {
                    BaseAddress = new System.Uri("http://localhost:26317/")
                };
                using HttpResponseMessage httpResponseMessage = await client.DeleteAsync($"/Api/ApiUserMaster/DeleteCodesMaster/{userId}");

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var result = await httpResponseMessage.Content.ReadAsStringAsync();
                    bool status = JsonConvert.DeserializeObject<bool>(result);
                    TempData["Title"] = "success";
                    TempData["Message"] = "deleted";
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
    }
}
