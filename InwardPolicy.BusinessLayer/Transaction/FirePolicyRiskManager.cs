using BusinessEntity;
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
            string query = "SELECT ROWNUM AS RISK_ID,(SELECT POL_APPR_STATUS FROM FIRE_POLICY WHERE POL_UID=RISK_POL_UID) AS POL_APPR_STATUS, " +
                                "RISK_UID,RISK_DESC,RISK_LC_SI,RISK_FC_SI,RISK_LC_PREM,RISK_FC_PREM," +
                                "(SELECT CM_DESC FROM CODES_MASTER WHERE CM_CODE=RISK_CLASS AND CM_TYPE='RISK CLASS') AS RISK_CLASS " +
                           "FROM FIRE_POLICY_RISK WHERE RISK_POL_UID=:PolUid ORDER BY RISK_ID ";
            return DBConnection.ExecuteQuerySelect(Dict, query).Tables[0];
        }

        public FirePolicyRisk GetRiskDetails(string riskUid)
        {
            Dictionary<string, Object> Dict = new Dictionary<string, Object>();
            Dict["riskUid"] = riskUid;
            string query = "SELECT * FROM FIRE_POLICY_RISK WHERE RISK_UID=:riskUid ";
            DataRow dr = DBConnection.ExecuteQuerySelect(Dict, query).Tables[0].Rows[0];
            FirePolicyRisk objFirePolicyRisk = new FirePolicyRisk
            {
                RiskUid = dr["RISK_UID"] != DBNull.Value ? dr["RISK_UID"].ToString() : null,
                RiskPolUid = dr["RISK_POL_UID"] != DBNull.Value ? dr["RISK_POL_UID"].ToString() : null,
                RiskClass = dr["RISK_CLASS"] != DBNull.Value ? dr["RISK_CLASS"].ToString() : null,
                RiskDesc = dr["RISK_DESC"] != DBNull.Value ? dr["RISK_DESC"].ToString() : null,
                RiskSICurr = dr["RISK_SI_CURR"] != DBNull.Value ? dr["RISK_SI_CURR"].ToString() : null,
                RiskPremCurr = dr["RISK_PREM_CURR"] != DBNull.Value ? dr["RISK_PREM_CURR"].ToString() : null,
                RiskFcSi = dr["RISK_FC_SI"] != DBNull.Value ? Convert.ToDouble(dr["RISK_FC_SI"]) : 0.0,  // Use default value 0.0 if null
                RiskLcSi = dr["RISK_LC_SI"] != DBNull.Value ? Convert.ToDouble(dr["RISK_LC_SI"]) : 0.0,  // Use default value 0.0 if null
                RiskPremRate = dr["RISK_PREM_RATE"] != DBNull.Value ? Convert.ToDouble(dr["RISK_PREM_RATE"]) : 0.0,  // Use default value 0.0 if null
                RiskFcPrem = dr["RISK_FC_PREM"] != DBNull.Value ? Convert.ToDouble(dr["RISK_FC_PREM"]) : 0.0,  // Use default value 0.0 if null
                RiskLcPrem = dr["RISK_LC_PREM"] != DBNull.Value ? Convert.ToDouble(dr["RISK_LC_PREM"]) : 0.0,  // Use default value 0.0 if null
                RiskCrBy = dr["RISK_CR_BY"] != DBNull.Value ? dr["RISK_CR_BY"].ToString() : null,
                RiskUpBy = dr["RISK_UP_BY"] != DBNull.Value ? dr["RISK_UP_BY"].ToString() : null
            };
            return objFirePolicyRisk;
        }

        public FirePolicyRisk GetRiskCurrency(string polUid)
        {
            string query = $"SELECT POL_PREM_CURRENCY,POL_SI_CURRENCY FROM FIRE_POLICY WHERE POL_UID={polUid}";
            DataRow dr= DBConnection.ExecuteDataset(query).Rows[0];
            FirePolicyRisk firePolicyRisk = new FirePolicyRisk();
            firePolicyRisk.RiskPremCurr = dr["POL_PREM_CURRENCY"].ToString();
            firePolicyRisk.RiskSICurr = dr["POL_SI_CURRENCY"].ToString();
            return firePolicyRisk;
        }

        public bool DeleteRisk(string riskUid,string polUid)
        {
            Dictionary<string, Object> Dict = new Dictionary<string, Object>();
            Dict["riskUid"] = riskUid;
            string query = "DELETE FROM FIRE_POLICY_RISK WHERE RISK_UID=:riskUid";
            int i = DBConnection.ExecuteQuery(Dict, query);
            UpdateFirePolicy(polUid);
            if (i == 1)
                return true;
            else
                return false;
        }

        public FirePolicyRisk AddRisk(string mode, FirePolicyRisk objFirePolicyRisk)
        {
            try
            {

                if (mode == "U")
                {
                    Dictionary<string, Object> Dict = new Dictionary<string, Object>();
                    Dict["RiskClass"] = objFirePolicyRisk.RiskClass;
                    Dict["RiskPremRate"] = objFirePolicyRisk.RiskPremRate;
                    Dict["RiskDesc"] = objFirePolicyRisk.RiskDesc;
                    Dict["RiskFcSi"] = objFirePolicyRisk.RiskFcSi;
                    Dict["RiskLcSi"] = objFirePolicyRisk.RiskLcSi;
                    Dict["RiskFcPrem"] = objFirePolicyRisk.RiskFcPrem;
                    Dict["RiskLcPrem"] = objFirePolicyRisk.RiskLcPrem;
                    Dict["RiskUpBy"] = objFirePolicyRisk.RiskUpBy;
                    Dict["RiskUid"] = Convert.ToInt32(objFirePolicyRisk.RiskUid);
                    string query = "UPDATE FIRE_POLICY_RISK SET RISK_CLASS = :RiskClass,RISK_PREM_RATE=:RiskPremRate, RISK_DESC = :RiskDesc,  " +
                                       "RISK_FC_SI = :RiskFcSi, RISK_LC_SI = :RiskLcSi, RISK_FC_PREM = :RiskFcPrem, RISK_LC_PREM = :RiskLcPrem, " +
                                       "RISK_UP_BY = :RiskUpBy, RISK_UP_DT = SYSDATE " +
                                   "WHERE RISK_UID = :RiskUid";
                    int i = DBConnection.ExecuteQuery(Dict, query);
                    UpdateFirePolicy(objFirePolicyRisk.RiskPolUid);
                    if (i == 1)
                        return objFirePolicyRisk;
                    else
                        return null;
                }
                else
                {
                    objFirePolicyRisk.RiskUid = GetRiskUid().ToString();
                    Dictionary<string, Object> Dict = new Dictionary<string, Object>();
                    Dict["RiskUid"] = Convert.ToInt32(objFirePolicyRisk.RiskUid);
                    Dict["RiskPolUid"] = Convert.ToInt32(objFirePolicyRisk.RiskPolUid);
                    Dict["RiskClass"] = objFirePolicyRisk.RiskClass;
                    Dict["RiskDesc"] = objFirePolicyRisk.RiskDesc;
                    Dict["RiskSICurr"] = objFirePolicyRisk.RiskSICurr;
                    Dict["RiskFcSi"] = objFirePolicyRisk.RiskFcSi;
                    Dict["RiskLcSi"] = objFirePolicyRisk.RiskLcSi;
                    Dict["RiskPremCurr"] = objFirePolicyRisk.RiskPremCurr;
                    Dict["RiskPremRate"] = objFirePolicyRisk.RiskPremRate;
                    Dict["RiskFcPrem"] = objFirePolicyRisk.RiskFcPrem;
                    Dict["RiskLcPrem"] = objFirePolicyRisk.RiskLcPrem;
                    Dict["RiskCrBy"] = objFirePolicyRisk.RiskCrBy;
                    
                    string query = "INSERT INTO FIRE_POLICY_RISK (RISK_UID, RISK_POL_UID, RISK_CLASS, RISK_DESC, RISK_SI_CURR, " +
                                        "RISK_FC_SI, RISK_LC_SI, RISK_PREM_CURR, RISK_PREM_RATE, RISK_FC_PREM, RISK_LC_PREM, RISK_CR_BY, " +
                                        "RISK_CR_DT) " +
                                    "VALUES (:RiskUid, :RiskPolUid, :RiskClass, :RiskDesc, :RiskSICurr, :RiskFcSi," +
                                        " :RiskLcSi, :RiskPremCurr, :RiskPremRate, :RiskFcPrem, :RiskLcPrem, :RiskCrBy, " +
                                        "SYSDATE)";
                    int i = DBConnection.ExecuteQuery(Dict, query);
                    //DBConnection.ExecuteProc(objFirePolicyRiskEntity.RiskPolUid);
                    UpdateFirePolicy(objFirePolicyRisk.RiskPolUid);
                    if (i == 1)
                        return objFirePolicyRisk;
                    else
                        return null;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public int GetRiskUid()
        {
            string query = "SELECT SEQ_RISK_UID.NEXTVAL FROM DUAL ";
            int uid = Convert.ToInt32(DBConnection.ExecuteScalar(query));
            return uid;
        }
        private void UpdateFirePolicy(string polUid)
        {
            Dictionary<string, Object> Dict = new Dictionary<string, Object>();
            Dict["RiskPolUid"] = polUid;
            string query = "UPDATE FIRE_POLICY SET " +
                "POL_FC_SI = ROUND((SELECT SUM(RISK_FC_SI) FROM FIRE_POLICY_RISK WHERE RISK_POL_UID = :RiskPolUid), 2), " +
                "POL_LC_SI = ROUND((SELECT SUM(RISK_LC_SI) FROM FIRE_POLICY_RISK WHERE RISK_POL_UID = :RiskPolUid), 2), " +
                "POL_GROSS_FC_PREM = ROUND((SELECT SUM(RISK_FC_PREM) FROM FIRE_POLICY_RISK WHERE RISK_POL_UID = :RiskPolUid), 2), " +
                "POL_GROSS_LC_PREM = ROUND((SELECT SUM(RISK_LC_PREM) FROM FIRE_POLICY_RISK WHERE RISK_POL_UID = :RiskPolUid), 2), " +
                "POL_VAT_FC_AMT = ROUND((SELECT SUM(RISK_FC_PREM) * 0.05 FROM FIRE_POLICY_RISK WHERE RISK_POL_UID = :RiskPolUid), 2), " +
                "POL_VAT_LC_AMT = ROUND((SELECT SUM(RISK_LC_PREM) * 0.05 FROM FIRE_POLICY_RISK WHERE RISK_POL_UID = :RiskPolUid), 2), " +
                "POL_NET_FC_PREM = ROUND((SELECT SUM(RISK_FC_PREM) * 1.05 FROM FIRE_POLICY_RISK WHERE RISK_POL_UID = :RiskPolUid), 2), " +
                "POL_NET_LC_PREM = ROUND((SELECT SUM(RISK_LC_PREM) * 1.05 FROM FIRE_POLICY_RISK WHERE RISK_POL_UID = :RiskPolUid), 2) " +
                "WHERE POL_UID = :RiskPolUid";
            DBConnection.ExecuteQuery(Dict, query);
        }
    }
}
