using Microsoft.AspNetCore.Mvc;
using System;
using BusinessLayer;
using BusinessEntity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Newtonsoft.Json;

namespace InwardPolicy.Core.Api.Controllers
{
    [Route("Api/[controller]")]
    [ApiController]
    public class ApiFirePolicyController : Controller
    {
        [HttpGet]
        [Route("LoadDashboardData")]
        public IActionResult LoadDashboardData()
        {
            try
            {
                FirePolicyManager objFirePolicyManager = new FirePolicyManager();
                DataRow dr = objFirePolicyManager.GetCounterValues();
                string json = JsonConvert.SerializeObject(dr);
                return Ok(json);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }

}
