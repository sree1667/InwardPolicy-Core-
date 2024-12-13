using BusinessEntity;
using InwardPolicy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace InwardPolicy.Core.App.Areas.Transaction.Controllers
{
    [Area("Transaction")]
    public class FirePolicyRiskController : Controller
    {
        public async Task<IActionResult> FirePolicyRisk(string id1,string id2, string id3)
        {
            HttpClient client = new HttpClient()
            {
                BaseAddress = new System.Uri("http://localhost:26317/")
            };
            //ddl
            FirePolicyRiskModel objFirePolicyRiskModel = new FirePolicyRiskModel();
            objFirePolicyRiskModel.FirePolicyRisk = new FirePolicyRisk();
            using HttpResponseMessage httpResponseMessage = await client.GetAsync("Api/ApiCodesMaster/FetchPolicyRiskDropdownList");

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
                using HttpResponseMessage httpResponseMessage1 = await client.GetAsync($"Api/ApiFirePolicy/LoadControl/{id1}");
                if (httpResponseMessage1.IsSuccessStatusCode)
                {
                    var result = await httpResponseMessage1.Content.ReadAsStringAsync();
                    objFirePolicyRiskModel.FirePolicyRisk = JsonConvert.DeserializeObject<FirePolicyRisk>(result);
                }
            }
            else
            {
                objFirePolicyRiskModel.Mode = "I";
                //objFirePolicyModel.FirePolicy.PolFmDt = null;
                //objFirePolicyModel.FirePolicy.PolToDt = null;
                //objFirePolicyModel.FirePolicy.PolAssrDob = null;

            }

            if (id3 == "A")
            {
                objFirePolicyRiskModel.ApprStatus = "A";
            }
            else
            {
                objFirePolicyRiskModel.ApprStatus = "N";

            }
            return View(objFirePolicyRiskModel);
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

    }
}
