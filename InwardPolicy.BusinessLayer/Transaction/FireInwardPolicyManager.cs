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
    }
}
