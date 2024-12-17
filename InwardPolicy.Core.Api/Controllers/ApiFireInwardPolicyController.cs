using BusinessEntity;
using BusinessLayer;
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
    public class ApiFireInwardPolicyController : ControllerBase
    {
        [HttpGet]
        [Route("LoadInwardControl/{poluid?}")]
        public IActionResult LoadInwardControl(string poluid)
        {
            try
            {
                FireInwardPolicyManager objFirePolicyManager = new FireInwardPolicyManager();
                FireInwardPolicy fireInwardPolicy  = objFirePolicyManager.FetchInwardDetails(poluid);
                string json = JsonConvert.SerializeObject(fireInwardPolicy);
                return Ok(json);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpGet]
        [Route("FireInwardPolicyBind/{poluid?}")]
        public IActionResult FireInwardPolicyBind(string poluid)
        {
            try
            {
                FireInwardPolicyManager objFirePolicyManager = new FireInwardPolicyManager();
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
        [Route("AddFireInwardPolicy/{mode?}")]
        public IActionResult AddFireInwardPolicy(string mode,FireInwardPolicy fireInwardPolicy)
        {
            try
            {
                FireInwardPolicyManager objFirePolicyManager = new FireInwardPolicyManager();
                fireInwardPolicy = objFirePolicyManager.AddInward(fireInwardPolicy, mode);
                string json = JsonConvert.SerializeObject(fireInwardPolicy);
                return Ok(json);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
