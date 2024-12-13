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

        private string GetPolicyNumber(FirePolicy objFirePolicy)
        {

            
            Dictionary<string, object> Dict = new Dictionary<string, object>();
            Dict["PolProdCode"] = objFirePolicy.PolProdCode;
            string query = " SELECT DFUN_POLICY_NUMBER(:PolProdCode) POLICY_NUMBER FROM dual";
            DataTable dt = DBConnection.ExecuteQuerySelect(Dict, query).Tables[0];
            string policyNumber = dt.Rows[0]["POLICY_NUMBER"].ToString();
            return policyNumber;
        }
        public int GetPolUid()
        {
            string query = "SELECT SEQ_POL_UID.NEXTVAL FROM DUAL ";
            int uid = Convert.ToInt32(DBConnection.ExecuteScalar(query));
            return uid;
        }

        public string AddFirePolicy(FirePolicy objFirePolicy,string mode)
        {
            if (mode == "U")
            {
                Dictionary<string, object> Dict = new Dictionary<string, object>();
                Dict["PolFmDt"] = objFirePolicy.PolFmDt;
                Dict["PolToDt"] = objFirePolicy.PolToDt;
                Dict["PolAssrName"] = objFirePolicy.PolAssrName;
                Dict["PolAssrAddress"] = objFirePolicy.PolAssrAddress;
                Dict["PolAssrMobile"] = objFirePolicy.PolAssrMobile;
                Dict["PolAssrEmail"] = objFirePolicy.PolAssrEmail;
                Dict["PolAssrDob"] = objFirePolicy.PolAssrDob;
                Dict["PolAssrOccupation"] = objFirePolicy.PolAssrOccupation;
                Dict["PolAssrType"] = objFirePolicy.PolAssrType;
                Dict["PolAssrCivilId"] = objFirePolicy.PolAssrCivilId;
                Dict["UpBy"] = objFirePolicy.CrOrUpBy;
                Dict["Poluid"] = objFirePolicy.Poluid;
                string query = "UPDATE FIRE_POLICY SET  POL_FM_DT = :PolFmDt, POL_TO_DT = :PolToDt,  " +
                    "POL_ASSR_NAME = :PolAssrName, POL_ASSR_ADDRESS = :PolAssrAddress, POL_ASSR_MOBILE = :PolAssrMobile, POL_ASSR_EMAIL = :PolAssrEmail, " +
                    "POL_ASSR_DOB = :PolAssrDob, POL_ASSR_OCCUPATION = :PolAssrOccupation, POL_ASSR_TYPE = :PolAssrType, POL_ASSR_CIVIL_ID = :PolAssrCivilId, " +
                   " POL_UP_BY = :UpBy, POL_UP_DT = SYSDATE WHERE POL_UID = :Poluid";
                int i = DBConnection.ExecuteQuery(Dict, query);
                if (i == 1)
                    return objFirePolicy.Poluid.ToString();
                else
                    return null;
            }
            else
            {
                string policyNumber = GetPolicyNumber(objFirePolicy);
                Dictionary<string, object> Dict = new Dictionary<string, object>();
                Dict["PolNo"] = policyNumber;
                string uid = GetPolUid().ToString();
                Dict["Poluid"] = Convert.ToInt32(uid);
                Dict["PolFmDt"] = objFirePolicy.PolFmDt;
                Dict["PolToDt"] = objFirePolicy.PolToDt;
                Dict["PolProdCode"] = objFirePolicy.PolProdCode;
                Dict["PolAssrName"] = objFirePolicy.PolAssrName;
                Dict["PolAssrAddress"] = objFirePolicy.PolAssrAddress;
                Dict["PolAssrMobile"] = objFirePolicy.PolAssrMobile;
                Dict["PolAssrEmail"] = objFirePolicy.PolAssrEmail;
                Dict["PolAssrDob"] = objFirePolicy.PolAssrDob;
                Dict["PolAssrOccupation"] = objFirePolicy.PolAssrOccupation;
                Dict["PolAssrType"] = objFirePolicy.PolAssrType;
                Dict["PolAssrCivilId"] = objFirePolicy.PolAssrCivilId;
                Dict["PolSICurrency"] = objFirePolicy.PolSICurrency;
                Dict["PolSICurrencyRate"] = objFirePolicy.PolSICurrencyRate;
                Dict["PolPremCurrency"] = objFirePolicy.PolPremCurrency;
                Dict["PolPremCurrencyRate"] = objFirePolicy.PolPremCurrencyRate;
                Dict["CrBy"] = objFirePolicy.CrOrUpBy;
                string query = "INSERT INTO FIRE_POLICY (POL_NO, POL_UID, POL_ISS_DT, POL_FM_DT, POL_TO_DT, POL_PROD_CODE, POL_ASSR_NAME, POL_ASSR_ADDRESS, POL_ASSR_MOBILE, POL_ASSR_EMAIL," +
                    " POL_ASSR_DOB, POL_ASSR_OCCUPATION, POL_ASSR_TYPE, POL_ASSR_CIVIL_ID, POL_SI_CURRENCY, POL_SI_CURR_RATE, POL_PREM_CURRENCY, POL_PREM_CURR_RATE," +
                    "  POL_APPR_STATUS,POL_CR_BY,POL_CR_DT) VALUES(:PolNo,:Poluid,SYSDATE,:PolFmDt,:PolToDt,:PolProdCode,:PolAssrName,:PolAssrAddress," +
                    ":PolAssrMobile,:PolAssrEmail,:PolAssrDob,:PolAssrOccupation,:PolAssrType,:PolAssrCivilId,:PolSICurrency,:PolSICurrencyRate,:PolPremCurrency,:PolPremCurrencyRate," +
                    "'N',:CrBy,SYSDATE)";
                int i = DBConnection.ExecuteQuery(Dict, query);
                if (i == 1)
                    return uid;
                else
                    return null;
            }
        }
        public DataTable BindGrid()
        {
            string query = "SELECT * FROM FIRE_POLICY ORDER BY POL_UID";
            return DBConnection.ExecuteDataset(query);
        }
        public FirePolicy FetchPolicyDetails(string polUid)
        {
            Dictionary<string, object> Dict = new Dictionary<string, object>();
            Dict["poluid"] = polUid;
            string query = $"SELECT * FROM FIRE_POLICY WHERE POL_UID=:poluid";
            string query2 = $"SELECT COUNT(INW_POL_UID) FROM FIRE_INW_POLICY WHERE INW_POL_UID={polUid}";
            int InwCount = Convert.ToInt32(DBConnection.ExecuteScalar(query2));
            DataRow dr = DBConnection.ExecuteQuerySelect(Dict, query).Tables[0].Rows[0];
            FirePolicy objfirePolicy = new FirePolicy();
            objfirePolicy.PolNo = dr["POL_NO"].ToString();
            //Binding DateTime fields
            string fmdt = Convert.ToDateTime(dr["POL_FM_DT"]).ToString("yyyy-MM-dd");
            objfirePolicy.PolFmDt = fmdt != null ? Convert.ToDateTime(fmdt) : DateTime.MinValue;
            objfirePolicy.PolToDt = dr["POL_TO_DT"] != DBNull.Value ? Convert.ToDateTime(dr["POL_TO_DT"]) : DateTime.MinValue;
            objfirePolicy.PolIssDt = Convert.ToDateTime(dr["POL_ISS_DT"]);
            objfirePolicy.PolAssrDob = dr["POL_ASSR_DOB"] != DBNull.Value ? Convert.ToDateTime(dr["POL_ASSR_DOB"]) : DateTime.MinValue;
            // Binding string fields
            objfirePolicy.PolProdCode = dr["POL_PROD_CODE"] != DBNull.Value ? dr["POL_PROD_CODE"].ToString() : string.Empty;
            objfirePolicy.PolAssrName = dr["POL_ASSR_NAME"] != DBNull.Value ? dr["POL_ASSR_NAME"].ToString() : string.Empty;
            objfirePolicy.PolAssrAddress = dr["POL_ASSR_ADDRESS"] != DBNull.Value ? dr["POL_ASSR_ADDRESS"].ToString() : string.Empty;
            objfirePolicy.PolAssrMobile = dr["POL_ASSR_MOBILE"] != DBNull.Value ? dr["POL_ASSR_MOBILE"].ToString() : string.Empty;
            objfirePolicy.PolAssrEmail = dr["POL_ASSR_EMAIL"] != DBNull.Value ? dr["POL_ASSR_EMAIL"].ToString() : string.Empty;
            objfirePolicy.PolAssrOccupation = dr["POL_ASSR_OCCUPATION"] != DBNull.Value ? dr["POL_ASSR_OCCUPATION"].ToString() : string.Empty;
            objfirePolicy.PolAssrType = dr["POL_ASSR_TYPE"] != DBNull.Value ? dr["POL_ASSR_TYPE"].ToString() : string.Empty;
            objfirePolicy.PolAssrCivilId = dr["POL_ASSR_CIVIL_ID"] != DBNull.Value ? dr["POL_ASSR_CIVIL_ID"].ToString() : string.Empty;
            // Binding currency fields
            objfirePolicy.PolSICurrency = dr["POL_SI_CURRENCY"] != DBNull.Value ? dr["POL_SI_CURRENCY"].ToString() : string.Empty;
            objfirePolicy.PolPremCurrency = dr["POL_PREM_CURRENCY"] != DBNull.Value ? dr["POL_PREM_CURRENCY"].ToString() : string.Empty;
            objfirePolicy.PolSICurrencyRate = dr["POL_SI_CURR_RATE"] != DBNull.Value ? Convert.ToDouble(dr["POL_SI_CURR_RATE"]) : 0.00D;
            objfirePolicy.PolPremCurrencyRate = dr["POL_PREM_CURR_RATE"] != DBNull.Value ? Convert.ToDouble(dr["POL_PREM_CURR_RATE"]) : 0.00D;
            // Binding monetary fields with conditional checks for DBNull and formatting decimal
            objfirePolicy.PolFcSi = dr["POL_FC_SI"] != DBNull.Value ? Convert.ToDouble(dr["POL_FC_SI"]) : 0.00;
            objfirePolicy.PolLcSi = dr["POL_LC_SI"] != DBNull.Value ? Convert.ToDouble(dr["POL_LC_SI"]) : 0.00;
            objfirePolicy.PolGrossFcPrem = dr["POL_GROSS_FC_PREM"] != DBNull.Value ? Convert.ToDouble(dr["POL_GROSS_FC_PREM"]) : 0.00;
            objfirePolicy.PolGrossLcPrem = dr["POL_GROSS_LC_PREM"] != DBNull.Value ? Convert.ToDouble(dr["POL_GROSS_LC_PREM"]) : 0.00;
            objfirePolicy.PolNetFcPrem = dr["POL_NET_FC_PREM"] != DBNull.Value ? Convert.ToDouble(dr["POL_NET_FC_PREM"]) : 0.00;
            objfirePolicy.PolNetLcPrem = dr["POL_NET_LC_PREM"] != DBNull.Value ? Convert.ToDouble(dr["POL_NET_LC_PREM"]) : 0.00;
            objfirePolicy.PolVatFcAmt = dr["POL_VAT_FC_AMT"] != DBNull.Value ? Convert.ToDouble(dr["POL_VAT_FC_AMT"]) : 0.00;
            objfirePolicy.PolVatLcAmt = dr["POL_VAT_LC_AMT"] != DBNull.Value ? Convert.ToDouble(dr["POL_VAT_LC_AMT"]) : 0.00;
            objfirePolicy.InwCount = InwCount;
            return objfirePolicy;
        }


    }
}
