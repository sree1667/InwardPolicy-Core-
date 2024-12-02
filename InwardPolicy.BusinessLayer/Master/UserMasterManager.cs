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
    }
}
