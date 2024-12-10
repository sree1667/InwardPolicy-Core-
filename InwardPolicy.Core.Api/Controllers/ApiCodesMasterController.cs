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

        [HttpDelete]
        [Route("DeleteCodesMaster/{code?}/{type?}")]
        public IActionResult DeleteUserMaster(string code,string type)
        {
            try
            {
                CodesMasterManager objCodesMasterManager = new CodesMasterManager();
                bool status = objCodesMasterManager.DeleteUserMaster(code, type);
                string json = JsonConvert.SerializeObject(status);
                return Ok(json);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        [Route("GetCodesMasterDetails/{code?}/{type?}")]
        public IActionResult GetCodesMasterDetails(string code,string type)
        {
            try
            {
                CodesMasterManager objCodesMasterManager = new CodesMasterManager();
                DataTable dt = objCodesMasterManager.GetCMDetails(code,type);
                CodesMaster objCodesMaster = new CodesMaster();
                objCodesMaster.Code = dt.Rows[0]["CM_CODE"].ToString();
                objCodesMaster.Type = dt.Rows[0]["CM_TYPE"].ToString();
                objCodesMaster.Description = dt.Rows[0]["CM_DESC"].ToString();
                objCodesMaster.Value = Convert.ToDouble(dt.Rows[0]["CM_VALUE"]);
                objCodesMaster.Active = dt.Rows[0]["CM_ACTIVE_YN"].ToString();
                return Ok(objCodesMaster);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        [Route("CheckCodesMaster/{code?}/{type?}")]
        public IActionResult CheckCodesMaster(string code,string type)
        {
            try
            {
                CodesMasterManager objCodesMasterManager = new CodesMasterManager();
                bool status = objCodesMasterManager.CheckCodesMaster(code,type);
                
                return Ok(status);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        [Route("CodesMasterInsert/{mode?}")]
        public IActionResult UserMasterInsert(CodesMaster objCodesMaster, string mode)
        {
            CodesMasterManager objCodesMasterManager = new CodesMasterManager();
            bool InsertStatus = objCodesMasterManager.InsertUserMaster(objCodesMaster, mode);
            return Ok(InsertStatus);
        }
    }
}
