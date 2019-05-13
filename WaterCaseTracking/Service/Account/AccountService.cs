using System.Web;
using System.Web.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WaterCaseTracking.Service
{
    public class AccountService
    {
        public string GetValue(string key)
        {
            FormsIdentity Identity = (FormsIdentity)HttpContext.Current.User.Identity;
            if (Identity.IsAuthenticated)
            {
                FormsAuthenticationTicket ticket = Identity.Ticket;
                JObject jsonData = (JObject)JsonConvert.DeserializeObject(ticket.UserData.ToLower());

                if (jsonData != null && jsonData[key.ToLower()] != null)
                {
                    return jsonData[key.ToLower()].Value<string>();
                }
            }

            return "";
        }
    }
}