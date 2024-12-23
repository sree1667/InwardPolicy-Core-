using DataAccessLayer;
using InwardPolicy.BusinessEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class ErrorCodeMasterManager
    {
        public string GetMessage(string str)
        {
            string query = $"SELECT ERR_DESC FROM ERROR_CODE_MASTER WHERE ERR_CODE='{str}'";
            string result = DBConnection.ExecuteScalar(query).ToString().Trim();
            return result;
        }

        public DataTable ErrorCodeMasterBind()
        {
            try
            {
                string query = "SELECT * FROM ERROR_CODE_MASTER ORDER BY ERR_TYPE,ERR_CODE";
                return DBConnection.ExecuteDataset(query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool InsertErrorCodeMaster(ErrorCodeMaster objErrorCodeMaster, string mode)
        {
            if (mode == "U")
            {
                Dictionary<string, Object> Dict = new Dictionary<string, object>();
               // Dict["Type"] = objErrorCodeMaster.Type;
                Dict["Description"] = objErrorCodeMaster.Desc;
                Dict["UpBy"] = objErrorCodeMaster.UpOrCrBy;
                Dict["Code"] = objErrorCodeMaster.Code.ToUpper().Trim();
                string query = "UPDATE ERROR_CODE_MASTER SET ERR_DESC = :Description," +
                                "ERR_UP_BY = :UpBy, ERR_UP_DT=SYSDATE " +
                                "WHERE ERR_CODE = :Code";
                int i = DBConnection.ExecuteQuery(Dict, query);
                if (i == 1)
                    return true;
                else
                    return false;
            }
            else
            {
                Dictionary<string, Object> Dict = new Dictionary<string, object>();
                Dict["Code"] = objErrorCodeMaster.Code.ToUpper().Trim();
                Dict["Type"] = objErrorCodeMaster.Type;
                Dict["Description"] = objErrorCodeMaster.Desc;
                Dict["CrBy"] = objErrorCodeMaster.UpOrCrBy;
                string query = "INSERT INTO ERROR_CODE_MASTER (ERR_CODE, ERR_TYPE, ERR_DESC, ERR_CR_BY, ERR_CR_DT) " +
                                "VALUES(:Code,:Type, :Description, :CrBy, SYSDATE) ";
                int i = DBConnection.ExecuteQuery(Dict, query);
                if (i == 1)
                    return true;
                else
                    return false;
            }
            
        }

        public DataTable GetErrDetails(string code)
        {
            try
            {
                Dictionary<string, Object> Dict = new Dictionary<string, object>();
                Dict["Code"] = code.ToUpper().Trim();
                string query = "SELECT * FROM ERROR_CODE_MASTER WHERE ERR_CODE=:Code";
                DataTable dt = DBConnection.ExecuteQuerySelect(Dict, query).Tables[0];
                return dt;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool CheckErrorCodeMaster(string code)
        {
            try
            {
                Dictionary<string, Object> Dict = new Dictionary<string, object>();
                Dict["Code"] = code.ToUpper().Trim();
                string query = $"SELECT 1 FROM ERROR_CODE_MASTER WHERE ERR_CODE=:Code";
                DataTable dt = DBConnection.ExecuteQuerySelect(Dict, query).Tables[0];
                int count = dt.Rows.Count;
                if (count != 1)
                    return false;
                else
                    return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteUserMaster(string code)
        {
            Dictionary<string, Object> Dict = new Dictionary<string, object>();
            Dict["Code"] = code.ToUpper().Trim();
            string query = "DELETE FROM ERROR_CODE_MASTER WHERE ERR_CODE=:Code";
            int i = DBConnection.ExecuteQuery(Dict, query);
            if (i == 1)
                return true;
            else
                return false;
        }
    }
}
