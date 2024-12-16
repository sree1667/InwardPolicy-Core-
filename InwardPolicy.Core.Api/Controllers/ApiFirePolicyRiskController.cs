using BusinessEntity;
using InwardPolicy.BusinessLayer.Transaction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace InwardPolicy.Core.Api.Controllers
{
    [Route("Api/[controller]")]
    [ApiController]
    public class ApiFirePolicyRiskController : ControllerBase
    {
        [HttpGet]
        [Route("FirePolicyRiskBind/{poluid?}")]
        public IActionResult FirePolicyRiskBind(string poluid)
        {
            try
            {
                FirePolicyRiskManager objFirePolicyManager = new FirePolicyRiskManager();
                DataTable dt = objFirePolicyManager.BindGrid(poluid);
                string json = JsonConvert.SerializeObject(dt);
                return Ok(json);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpGet]
        [Route("FetchRiskDetails/{riskUid?}")]
        public IActionResult FetchRiskDetails(string riskUid)
        {
            try
            {
                FirePolicyRiskManager objFirePolicyManager = new FirePolicyRiskManager();
                FirePolicyRisk objFirePolicyRisk= objFirePolicyManager.GetRiskDetails(riskUid);
                string json = JsonConvert.SerializeObject(objFirePolicyRisk);
                return Ok(json);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpPost]
        [Route("AddRisk/{mode?}")]
        public IActionResult AddRisk (string mode,FirePolicyRisk objFirePolicyRisk)
        {
            try
            {
                FirePolicyRiskManager objFirePolicyManager = new FirePolicyRiskManager();
                objFirePolicyRisk = objFirePolicyManager.AddRisk(mode, objFirePolicyRisk);
                string json = JsonConvert.SerializeObject(objFirePolicyRisk);
                return Ok(json);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        
        
    }
}
