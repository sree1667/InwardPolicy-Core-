using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class FirePolicyManager
    {
        public DataRow GetCounterValues()
        {
            string query = "SELECT COUNT(*) AS TOTAL_POLICY,SUM(POL_LC_SI) AS TOTAL_SI," +
                 "SUM(CASE WHEN POL_APPR_STATUS = 'A' THEN 1 ELSE 0 END) AS TOTAL_APPROVED FROM FIRE_POLICY";
            return DBConnection.ExecuteDataset(query).Rows[0];
        }
    }
}
