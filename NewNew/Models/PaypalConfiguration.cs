using PayPal.Api;
using System.Collections.Generic;

namespace NewNew.Models
{
    public class PaypalConfiguration
    {
        static PaypalConfiguration()
        {

        }
        public static Dictionary<string, string> GetConfig(string mode)
        {
            return new Dictionary<string, string>()
           {
               {"mode",mode }
           };
        }
        private static string GetAccessToken(string ClientId, string ClientSecret, string mode)
        {
            var accessToken = new OAuthTokenCredential(ClientId, ClientSecret, GetConfig(mode)).GetAccessToken();
            return accessToken;
        }
        public static APIContext GetAPIContext(string clientId, string clientSecret, string mode)
        {
            APIContext apiContext = new APIContext(GetAccessToken(clientId, clientSecret, mode));
            apiContext.Config = GetConfig(mode);
            return apiContext;
        }
    }
}
