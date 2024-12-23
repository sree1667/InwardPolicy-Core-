using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace InwardPolicyHelper
{
    public class ECMHelper
    {
        public static async Task<string> GetErrorMessage(string code)
        {
            string message = string.Empty;
            HttpClient client = new HttpClient()
            {
                BaseAddress = new System.Uri("http://localhost:26317/")
            };

            using HttpResponseMessage httpResponseMessage = await client.GetAsync($"/Api/ApiErrorCodeMaster/GetErrorMessage/{code}");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var result = await httpResponseMessage.Content.ReadAsStringAsync();
                message = JsonConvert.DeserializeObject<string>(result);
                return message;
            }
            else
            {
                return message;
            }
        }
    }
}
