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
    }
}
