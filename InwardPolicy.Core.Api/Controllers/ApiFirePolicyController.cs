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
    public class ApiFirePolicyController : ControllerBase
    {
        [HttpGet]
        [Route("LoadDashboardData")]
        public IActionResult LoadDashboardData()
        {
            try
            {
                FirePolicyManager objFirePolicyManager = new FirePolicyManager();
                DataTable dt = objFirePolicyManager.GetCounterValues();
                string json = JsonConvert.SerializeObject(dt);
                return Ok(json);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet]
        [Route("GetPolicyCount")]
        public int[] GetPolicyCount()
        {
            try
            {
                FirePolicyManager objFirePolicyManager = new FirePolicyManager();
                int[] policyCount = objFirePolicyManager.GetPolicyCount();
                return policyCount;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }

}
