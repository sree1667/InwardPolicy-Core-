using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLayer;
using BusinessEntity;
using System.Threading.Tasks;

namespace InwardPolicy.Core.Api.Controllers
{
    [Route("Api/[controller]")]
    [ApiController]
    public class ApiUserMasterController : Controller
    {
        [HttpPost]
        [Route("CheckLogin")]
        public IActionResult CheckLogin(UserMaster objUserMaster)
        {
            UserMasterManager objUserMasterManager = new UserMasterManager();
            bool loginStatus = objUserMasterManager.CheckLogin(objUserMaster.UserId, objUserMaster.Password);
            return Ok(loginStatus);
        }
    }
}
