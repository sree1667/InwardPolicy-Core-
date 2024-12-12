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
    }
}
