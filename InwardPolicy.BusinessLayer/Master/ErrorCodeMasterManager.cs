using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class ErrorCodeMasterManager
    {
        public string GetMessage(string str)
        {
            string query = $"SELECT ERR_DESC FROM ERROR_CODE_MASTER WHERE ERR_CODE='{str}'";
            string result = DBConnection.ExecuteScalar(query).ToString().Trim();
            return result;
        }
    }
}
