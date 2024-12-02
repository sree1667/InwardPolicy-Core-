using BusinessEntity;
using InwardPolicy.Admin.Models;
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
        public IActionResult UserMaster()
        {
            UserMasterModel model = new UserMasterModel();
            model.UserMaster = new UserMaster();
            return View(model);
        }

        //[HttpPost]
        //public async Task<IActionResult> UserMasterBind()
        //{

        //    HttpClient client = new HttpClient()
        //    {
        //        BaseAddress = new System.Uri("http://localhost:26317/")
        //    };
        //    using HttpResponseMessage httpResponseMessage = await client.GetAsync($"Api/ApiUserMaster/UserMasterBind");
        //    if (httpResponseMessage.IsSuccessStatusCode)
        //    {
        //        var result = await httpResponseMessage.Content.ReadAsStringAsync();
        //        //DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
        //        return Ok(result);
        //    }
        //    else
        //    {
        //        return null;
        //    }

        //}

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

                    int pageSize = length != null ? Convert.ToInt32(length) : 10;
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
                            u.USER_NAME.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                            u.USER_ID.ToString().Contains(searchValue)).ToList();
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
