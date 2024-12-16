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
    public class FireInwardPolicyManager
    {
        public DataTable BindGrid(string poluid)
        {
            string query = $"SELECT ROWNUM SERIAL_NUM,INW_UID,INW_ORG_POL_NO,INW_CEDING_SOURCE," +
                $"(SELECT CM_DESC FROM CODES_MASTER WHERE CM_CODE=INW_RISK_CLASS AND CM_TYPE='RISK CLASS') AS INW_RISK_CLASS FROM FIRE_INW_POLICY WHERE INW_POL_UID={poluid}";
            DataTable dt = DBConnection.ExecuteDataset(query);
            return dt;

        }

        public FireInwardPolicy FetchInwardDetails(string uid)
        {
            Dictionary<string, object> Dict = new Dictionary<string, object>();
            Dict["InwPolUid"] = uid;
            string query = "SELECT * FROM FIRE_INW_POLICY  WHERE INW_POL_UID=:InwPolUid";
            DataTable dt = DBConnection.ExecuteQuerySelect(Dict, query).Tables[0];
            DataRow dr = dt.Rows[0];
            FireInwardPolicy objFireInwardPolicy = new FireInwardPolicy();
            // dt into obj
            objFireInwardPolicy.InwPremCurr = dr["INW_SI_CURR"] != DBNull.Value ? dr["INW_SI_CURR"].ToString() : null;
            objFireInwardPolicy.InwSiCurr = dr["INW_PREM_CURR"] != DBNull.Value ? dr["INW_PREM_CURR"].ToString() : null;
            objFireInwardPolicy.InwOrgPolNo = dr["INW_ORG_POL_NO"] != DBNull.Value ? dr["INW_ORG_POL_NO"].ToString() : null;
            objFireInwardPolicy.InwCedingSource = dr["INW_CEDING_SOURCE"] != DBNull.Value ? dr["INW_CEDING_SOURCE"].ToString() : null;
            objFireInwardPolicy.InwRiskClass = dr["INW_RISK_CLASS"] != DBNull.Value ? dr["INW_RISK_CLASS"].ToString() : null;
            objFireInwardPolicy.InwOrgSiFc = dr["INW_ORG_SI_FC"] != DBNull.Value ? Convert.ToDouble(dr["INW_ORG_SI_FC"]) : 0;
            objFireInwardPolicy.InwOrgSiLc = dr["INW_ORG_SI_LC"] != DBNull.Value ? Convert.ToDouble(dr["INW_ORG_SI_LC"]) : 0;
            objFireInwardPolicy.InwOrgPremFc = dr["INW_ORG_PREM_FC"] != DBNull.Value ? Convert.ToDouble(dr["INW_ORG_PREM_FC"]) : 0;
            objFireInwardPolicy.InwOrgPremLc = dr["INW_ORG_PREM_LC"] != DBNull.Value ? Convert.ToDouble(dr["INW_ORG_PREM_LC"]) : 0;
            objFireInwardPolicy.InwOrgInsName = dr["INW_ORG_INS_NAME"] != DBNull.Value ? dr["INW_ORG_INS_NAME"].ToString() : null;
            objFireInwardPolicy.InwSharePerc = dr["INW_SHARE_PERC"] != DBNull.Value ? Convert.ToDouble(dr["INW_SHARE_PERC"]) : 0;
            objFireInwardPolicy.InwSiShareFc = dr["INW_SI_SHARE_FC"] != DBNull.Value ? Convert.ToDouble(dr["INW_SI_SHARE_FC"]) : 0;
            objFireInwardPolicy.InwSiShareLc = dr["INW_SI_SHARE_LC"] != DBNull.Value ? Convert.ToDouble(dr["INW_SI_SHARE_LC"]) : 0;
            objFireInwardPolicy.InwPremShareFc = dr["INW_PREM_SHARE_FC"] != DBNull.Value ? Convert.ToDouble(dr["INW_PREM_SHARE_FC"]) : 0;
            objFireInwardPolicy.InwPremShareLc = dr["INW_PREM_SHARE_LC"] != DBNull.Value ? Convert.ToDouble(dr["INW_PREM_SHARE_LC"]) : 0;
            objFireInwardPolicy.InwCommPerc = dr["INW_COMM_PERC"] != DBNull.Value ? Convert.ToDouble(dr["INW_COMM_PERC"]) : 0;
            objFireInwardPolicy.InwCommLcAmt = dr["INW_COMM_LC_AMT"] != DBNull.Value ? Convert.ToDouble(dr["INW_COMM_LC_AMT"]) : 0;
            objFireInwardPolicy.InwCommFcAmt = dr["INW_COMM_FC_AMT"] != DBNull.Value ? Convert.ToDouble(dr["INW_COMM_FC_AMT"]) : 0;
            objFireInwardPolicy.InwRiskId = dr["INW_RISK_ID"] != DBNull.Value ? dr["INW_RISK_ID"] : null;

            // For DateTime fields, handle nullable values
            objFireInwardPolicy.InwCrDt = dr["INW_CR_DT"] != DBNull.Value ? Convert.ToDateTime(dr["INW_CR_DT"]) : (DateTime?)null;
            objFireInwardPolicy.InwUpDt = dr["INW_UP_DT"] != DBNull.Value ? Convert.ToDateTime(dr["INW_UP_DT"]) : DateTime.MinValue;

            //return objFireInwardPolicy;
            return objFireInwardPolicy;

        }
    }
}
