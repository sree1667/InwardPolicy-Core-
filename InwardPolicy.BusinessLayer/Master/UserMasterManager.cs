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
    public class UserMasterManager
    {
        public bool CheckLogin(string UserId, string Password)
        {

            try
            {
                Dictionary<string, Object> userCheckDict = new Dictionary<string, object>();
                userCheckDict["puserId"] = UserId.Trim();
                string userDetailsQuery = $"SELECT USER_ID,USER_NAME,USER_PASSWORD,USER_ACTIVE_YN FROM USER_MASTER WHERE USER_ID=:puserId";
                DataTable dt = DBConnection.ExecuteQuerySelect(userCheckDict, userDetailsQuery).Tables[0];
                bool result;

                if (dt.Rows.Count == 1)
                {
                    if (Password == dt.Rows[0]["USER_PASSWORD"].ToString() && dt.Rows[0]["USER_ACTIVE_YN"].ToString() == "Y")
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
                else
                {
                    result = false;
                }
                return result;
            }
            catch
            {
                throw;

            }
        }
        public DataTable UserMasterBind()
        {
            try
            {
                string query = "SELECT USER_ID,USER_NAME," +
                    "CASE WHEN USER_ACTIVE_YN='Y' THEN 'Yes' WHEN USER_ACTIVE_YN='N' THEN 'No' END AS USER_ACTIVE_YN " +
                    "FROM USER_MASTER ORDER BY USER_ID";
                return DBConnection.ExecuteDataset(query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool InsertUserMaster(UserMaster objUserMaster)
        {
            try
            {
                Dictionary<string, Object> Dict = new Dictionary<string, object>();
                Dict["UserId"] = objUserMaster.UserId.ToUpper().Trim();
                Dict["UserName"] = objUserMaster.UserName;
                Dict["Password"] = objUserMaster.Password;
                Dict["CrBy"] = objUserMaster.CrBy;
                Dict["Active"] = objUserMaster.Active;
                string query = "INSERT INTO USER_MASTER (USER_ID, USER_NAME, USER_PASSWORD, USER_CR_BY, USER_CR_DT, USER_ACTIVE_YN) VALUES(:UserId,:UserName, :Password, :CrBy, SYSDATE, :Active) ";
                int i = DBConnection.ExecuteQuery(Dict, query);
                if (i == 1)
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool DeleteUserMaster(string userId)
        {
            try
            {
                Dictionary<string, Object> Dict = new Dictionary<string, object>();
                Dict["UserId"] = userId.ToUpper().Trim();
                string query = "DELETE FROM USER_MASTER WHERE USER_ID=:UserId";
                int i = DBConnection.ExecuteQuery(Dict, query);
                if (i == 1)
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
