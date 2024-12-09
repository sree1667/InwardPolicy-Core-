using Microsoft.AspNetCore.Mvc;
using BusinessLayer;
using BusinessEntity;
using System.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InwardPolicy.Core.Api.Controllers
{
    [Route("Api/[controller]")]
    [ApiController]
    public class ApiCodesMasterController : Controller
    {
        [HttpGet]
        [Route("CodesMasterBind")]
        public IActionResult CodesMasterBind()
        {
            try
            {
                CodesMasterManager objCodesMasterManager = new CodesMasterManager();
                DataTable dt = objCodesMasterManager.CodesMasterBind();
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
