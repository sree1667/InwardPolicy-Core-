using BusinessLayer;
using InwardPolicy.BusinessEntity;
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
    [Route("api/[controller]")]
    [ApiController]
    public class ApiErrorCodeMasterController : ControllerBase
    {
        [HttpGet]
        [Route("GetErrorMessage/{errCode?}")]
        public IActionResult GetErrorMessage(string errCode)
        {
            ErrorCodeMasterManager objErrorCodeMasterManager = new ErrorCodeMasterManager();
            var json = JsonConvert.SerializeObject(objErrorCodeMasterManager.GetMessage(errCode));
            return Ok(json);
        }

        [HttpGet]
        [Route("ErrorCodeMasterBind")]
        public IActionResult ErrorCodeMasterBind()
        {
            try
            {
                ErrorCodeMasterManager objErrorCodeMasterManager = new ErrorCodeMasterManager();
                DataTable dt = objErrorCodeMasterManager.ErrorCodeMasterBind();
                string json = JsonConvert.SerializeObject(dt);
                return Ok(json);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpPost]
        [Route("ErrorCodeMasterInsert/{mode?}")]
        public IActionResult ErrorCodeMasterInsert(ErrorCodeMaster objErrorCodeMaster, string mode)
        {
            ErrorCodeMasterManager objErrorCodeMasterManager = new ErrorCodeMasterManager();
            bool InsertStatus = ErrorCodeMasterManager.InsertErrorCodeMaster(objErrorCodeMaster, mode);
            return Ok(InsertStatus);
        }
        [HttpGet]
        [Route("GetErrorCodeMasterDetails/{code?}")]
        public IActionResult GetErrorCodeMasterDetails(string code)
        {
            try
            {
                ErrorCodeMasterManager objErrorCodeMasterManager = new ErrorCodeMasterManager();
                DataTable dt = objErrorCodeMasterManager.GetErrDetails(code);
                ErrorCodeMaster objErrorCodeMaster = new ErrorCodeMaster();
                objErrorCodeMaster.Code = dt.Rows[0]["ERR_CODE"].ToString();
                objErrorCodeMaster.Type = dt.Rows[0]["ERR_TYPE"].ToString();
                objErrorCodeMaster.Desc = dt.Rows[0]["ERR_DESC"].ToString();
               
                return Ok(objErrorCodeMaster);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        [Route("CheckErrorCodeMaster/{code?}")]
        public IActionResult CheckErrorCodeMaster(string code)
        {
            try
            {
                ErrorCodeMasterManager objErrorCodeMasterManager = new ErrorCodeMasterManager();
                bool status = objErrorCodeMasterManager.CheckErrorCodeMaster(code);

                return Ok(status);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpDelete]
        [Route("DeleteErrorCodeMaster/{code?}")]
        public IActionResult DeleteErrorCodeMaster(string code)
        {
            try
            {
                ErrorCodeMasterManager objErrorCodeMasterManager = new ErrorCodeMasterManager();
                bool status = objErrorCodeMasterManager.DeleteUserMaster(code);
                string json = JsonConvert.SerializeObject(status);
                return Ok(json);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
