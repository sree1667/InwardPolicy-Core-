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

        [HttpGet]
        [Route("FetchDropdownList")]
        public IActionResult FetchDropdownList()
        {
            try
            {
                CodesMasterManager objCodesMasterManager = new CodesMasterManager();
                DataSet ds = new DataSet();
                DataTable dtcopy = new DataTable();
                //Assured Type
                DataTable dt = objCodesMasterManager.BindDropDown("ASSURED TYPE");
                dtcopy = dt.Copy();
                dtcopy.TableName = "ASSURED TYPE";
                ds.Tables.Add(dtcopy);
                //Occupation
                dt.Clear();
                dt = objCodesMasterManager.BindDropDown("OCCUPATION");
                dtcopy = dt.Copy();
                dtcopy.TableName = "OCCUPATION";
                ds.Tables.Add(dtcopy);
                //PRODUCT CODE
                dt.Clear();
                dt = objCodesMasterManager.BindDropDown("PRODUCT CODE");
                dtcopy = dt.Copy();
                dtcopy.TableName = "PRODUCT CODE";
                ds.Tables.Add(dtcopy);
                //RISK CLASS
                dt.Clear();
                dt = objCodesMasterManager.BindDropDown("RISK CLASS");
                dtcopy = dt.Copy();
                dtcopy.TableName = "RISK CLASS";
                ds.Tables.Add(dtcopy);
                //CEDING SOURCE
                dt.Clear();
                dt = objCodesMasterManager.BindDropDown("CEDING SOURCE");
                dtcopy = dt.Copy();
                dtcopy.TableName = "CEDING SOURCE";
                ds.Tables.Add(dtcopy);
                //Currency
                dt.Clear();
                dt = objCodesMasterManager.BindDropDown("CURRENCY");
                dtcopy = dt.Copy();
                dtcopy.TableName = "CURRENCY";
                ds.Tables.Add(dtcopy);
                return Ok(JsonConvert.SerializeObject(ds));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        

        [HttpGet]
        [Route("FetchPolicyRiskDropdownList")]
        public IActionResult FetchPolicyRiskDropdownList()
        {
            try
            {
                CodesMasterManager objCodesMasterManager = new CodesMasterManager();
                DataSet ds = new DataSet();
                DataTable dtcopy = new DataTable();
                //Assured Type
                
                DataTable dt = objCodesMasterManager.BindDropDown("CURRENCY");
                dtcopy = dt.Copy();
                dtcopy.TableName = "CURRENCY";
                ds.Tables.Add(dtcopy);
                return Ok(JsonConvert.SerializeObject(ds));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        
        [HttpGet]
        [Route("GetCurrencyRate/{value?}")]
        public IActionResult GetCurrencyRate(string value)
        {
            try
            {
                string type = "CURRENCY";
                CodesMasterManager objCodesMasterManager = new CodesMasterManager();
                string rate = objCodesMasterManager.GetddlValue(value, type);
                return Ok(rate);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

       
    }
}
