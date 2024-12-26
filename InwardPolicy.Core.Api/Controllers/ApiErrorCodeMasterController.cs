using BusinessLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InwardPolicy.Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiErrorCodeMasterController : ControllerBase
    {
        [HttpPost]
        [Route("CheckLogin/{errCode?}")]
        public IActionResult GetErrorMessage(string errCode)
        {
            ErrorCodeMasterManager objErrorCodeMasterManager = new ErrorCodeMasterManager();
            return Ok(objErrorCodeMasterManager.GetMessage(errCode));
        }
    }
}
