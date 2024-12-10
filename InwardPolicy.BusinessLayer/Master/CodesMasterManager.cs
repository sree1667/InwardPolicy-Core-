using BusinessEntity;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class CodesMasterManager
    {
        public DataTable CodesMasterBind()
        {
            try
            {
                string query = "SELECT CM_TYPE,CM_CODE,CM_DESC,CM_VALUE," +
                    "CASE WHEN CM_ACTIVE_YN='Y' THEN 'Yes' WHEN CM_ACTIVE_YN='N' THEN 'No' END AS CM_ACTIVE_YN " +
                    "FROM CODES_MASTER ORDER BY CM_TYPE,CM_CODE";
                return DBConnection.ExecuteDataset(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public bool DeleteUserMaster(string code, string type)
        {
            try
            {
                Dictionary<string, Object> Dict = new Dictionary<string, object>();
                Dict["Code"] = code.ToUpper().Trim();
                Dict["Type"] = type.ToUpper().Trim();
                string query = "DELETE FROM CODES_MASTER WHERE CM_CODE=:Code AND CM_TYPE=:Type";
                int i = DBConnection.ExecuteQuery(Dict , query);
                if (i == 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetCMDetails(string code, string type)
        {
            try
            {
                Dictionary<string, Object> Dict = new Dictionary<string, Object>();
                Dict["Code"] = code.ToUpper().Trim();
                Dict["Type"] = type.ToUpper().Trim();
                string query = "SELECT * FROM CODES_MASTER WHERE CM_CODE=:Code AND CM_TYPE=:Type";
                DataTable dt = DBConnection.ExecuteQuerySelect(Dict, query).Tables[0];
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool InsertUserMaster(CodesMaster objCodesMaster, string mode)
        {
            if (mode == "U" && !string.IsNullOrEmpty(mode))
            {
                Dictionary<string, object> Dict = new Dictionary<string, object>();
                
                Dict["Description"] = objCodesMaster.Description;
                Dict["Value"] = objCodesMaster.Value;
                Dict["UpBy"] = objCodesMaster.UpBy;
                Dict["Active"] = objCodesMaster.Active;
                Dict["Code"] = objCodesMaster.Code.ToUpper().Trim();
                Dict["Type"] = objCodesMaster.Type.ToUpper().Trim();
                string query = "UPDATE CODES_MASTER SET CM_DESC = :Description, CM_VALUE = :Value, CM_UP_BY = :UpBy,CM_UP_DT=SYSDATE, CM_ACTIVE_YN = :Active WHERE CM_CODE = :Code AND CM_TYPE = :Type";
                int i = DBConnection.ExecuteQuery(Dict, query);
                if (i == 1)
                    return true;
                else
                    return false;
            }
            else 
            {
                Dictionary<string, Object> Dict = new Dictionary<string, object>();
                Dict["Code"] = objCodesMaster.Code.ToUpper().Trim();
                Dict["Type"] = objCodesMaster.Type.ToUpper().Trim();
                Dict["Description"] = objCodesMaster.Description;
                Dict["Value"] = objCodesMaster.Value;
                Dict["CrBy"] = objCodesMaster.CrBy;
                Dict["Active"] = objCodesMaster.Active;
                string query = "INSERT INTO CODES_MASTER (CM_CODE, CM_TYPE, CM_DESC, CM_VALUE, CM_CR_BY, CM_CR_DT, CM_ACTIVE_YN) VALUES(:Code,:Type, :Description, NVL(:Value, 0), :CrBy, SYSDATE, :Active) ";
                int i = DBConnection.ExecuteQuery(Dict, query);
                if (i == 1)
                    return true;
                else
                    return false;
            }
            
        }

        public bool CheckCodesMaster(string code, string type)
        {
            Dictionary<string, Object> Dict = new Dictionary<string, object>();
            
            Dict["Code"] = code.ToUpper().Trim();
            Dict["Type"] = type.ToUpper().Trim();
            string query = $"SELECT 1 FROM CODES_MASTER WHERE CM_CODE=:Code AND CM_TYPE=:Type";
            DataTable dt = DBConnection.ExecuteQuerySelect(Dict, query).Tables[0];
            int count = dt.Rows.Count;
            if (count != 1)
                return false;
            else
                return true;
        }
    }
}
