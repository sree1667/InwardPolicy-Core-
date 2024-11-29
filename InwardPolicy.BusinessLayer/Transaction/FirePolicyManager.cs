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
        public DataTable GetCounterValues()
        {
            string query = "SELECT COUNT(*) AS TOTAL_POLICY,SUM(POL_LC_SI) AS TOTAL_SI," +
                 "SUM(CASE WHEN POL_APPR_STATUS = 'A' THEN 1 ELSE 0 END) AS TOTAL_APPROVED FROM FIRE_POLICY";
            return DBConnection.ExecuteDataset(query);
        }

        public int[] GetPolicyCount()
        {
            string query = "SELECT TO_CHAR(pol_iss_dt, 'YYYY-MM') AS month,COUNT(*) AS policies_issued " +
                             "FROM fire_policy " +
                             "WHERE pol_iss_dt >= ADD_MONTHS(TRUNC(SYSDATE, 'MM'), -6)" +
                             "GROUP BY TO_CHAR(pol_iss_dt, 'YYYY-MM') " +
                             "ORDER BY month desc";
            DataTable dt = DBConnection.ExecuteDataset(query);
            int rowCount = dt.Rows.Count;
            int[] policyCount = new int[7];
            for (int i = 0; i < rowCount; i++)
            {
                policyCount[6 - i] = Convert.ToInt32(dt.Rows[i]["policies_issued"]);
            }
            return policyCount;
        }
    }
}
