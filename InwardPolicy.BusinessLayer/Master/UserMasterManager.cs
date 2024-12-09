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

        public bool CheckUserId(string userId)
        {
            try
            {
                Dictionary<string, Object> Dict = new Dictionary<string, object>();
                Dict["UserId"] = userId.ToUpper().Trim();
                string query = $"SELECT 1 FROM USER_MASTER WHERE USER_ID=:UserId";
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
        public bool InsertUserMaster(UserMaster objUserMaster,string mode)
        {
            try
            {
                
                if (mode == "I")
                {
                    Dictionary<string, Object> Dict = new Dictionary<string, object>();
                    
                    Dict["UserId"] = objUserMaster.UserId.ToUpper().Trim();
                    Dict["UserName"] = objUserMaster.UserName;
                    Dict["Password"] = objUserMaster.Password;
                    Dict["CrBy"] = objUserMaster.UpOrCrBy;
                    Dict["Active"] = objUserMaster.Active;
                  
                    string query = "INSERT INTO USER_MASTER (USER_ID, USER_NAME, USER_PASSWORD, USER_CR_BY, USER_CR_DT, USER_ACTIVE_YN) VALUES(:UserId,:UserName, :Password, :CrBy, SYSDATE,:Active) ";
                    int i = DBConnection.ExecuteQuery(Dict, query);
                    if (i == 1)
                        return true;
                    else
                        return false;
                }
                else if (mode == "U")
                {
                    Dictionary<string, Object> Dict = new Dictionary<string, object>();
                   
                    Dict["UserName"] = objUserMaster.UserName;
                    //Dict["Password"] = objUserMaster.Password;
                    Dict["UpBy"] = objUserMaster.UpOrCrBy;
                    Dict["Active"] = objUserMaster.Active;
                    Dict["UserId"] = objUserMaster.UserId.ToUpper().Trim();
                    string query = "UPDATE USER_MASTER SET USER_NAME = :UserName,USER_UP_BY = :UpBy, USER_UP_DT=SYSDATE, USER_ACTIVE_YN = :Active WHERE USER_ID = :UserId";
                    int i = DBConnection.ExecuteQuery(Dict, query);
                    if (i == 1)
                        return true;
                    else
                        return false;
                }
                else
                {
                    return false;
                }
                
            }
            catch (Exception ex)
            {

                throw ex;
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
        public DataTable GetUserDetails(string UserId)
        {
            try
            {
                Dictionary<string, Object> Dict = new Dictionary<string, object>();
                Dict["UserId"] = UserId;
                string query = "SELECT * FROM USER_MASTER WHERE USER_ID=:UserId";
                DataTable dt = DBConnection.ExecuteQuerySelect(Dict, query).Tables[0];
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
