using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InwardPolicy.BusinessLayer.Transaction
{
    public class FirePolicyRiskManager
    {
        public DataTable BindGrid(string PolUid)
        {
            Dictionary<string, Object> Dict = new Dictionary<string, Object>();
            Dict["PolUid"] = PolUid;
            string query = "SELECT RISK_ID, RISK_UID,RISK_DESC,RISK_LC_SI,RISK_FC_SI,RISK_LC_PREM,RISK_FC_PREM,(SELECT CM_DESC FROM CODES_MASTER WHERE CM_CODE=RISK_CLASS AND CM_TYPE='RISK CLASS') AS RISK_CLASS FROM FIRE_POLICY_RISK WHERE RISK_POL_UID=:PolUid ORDER BY RISK_ID ";
            return DBConnection.ExecuteQuerySelect(Dict, query).Tables[0];
        }
    }
}
