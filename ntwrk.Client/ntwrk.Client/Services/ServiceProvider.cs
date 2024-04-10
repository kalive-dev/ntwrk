using Newtonsoft.Json;
using ntwrk.Client.Services.Authenticate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ntwrk.Client.Services
{
    public class ServiceProvider
    {
        private static ServiceProvider _instance;
        private string _serverRootUrl = "https://10.0.2.2:7233";
        public string _accessToken = "";

        private ServiceProvider() {
            
        }

        public static ServiceProvider GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ServiceProvider();
            }
            return _instance;
        }

        public async Task <AuthenticateResponse> Authenticate(AuthenticateRequest request)
        {
            var devSslHelper = new DevHttpsConnectionHelper(sslPort: 7136);
            using (HttpClient client = devSslHelper.HttpClient)
            {
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Method = HttpMethod.Post;
                httpRequestMessage.RequestUri = new Uri(devSslHelper.DevServerRootUrl + "/Authenticate/Authenticate");
                if (request != null)
                {
                    string jsonContent = JsonConvert.SerializeObject(request);
                    var httpContent = new StringContent(jsonContent, encoding: Encoding.UTF8,"applitcation/json");
                    httpRequestMessage.Content = httpContent;
                }

                try
                {
                    var response = await client.SendAsync(httpRequestMessage);
                    var responseContent = await response.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<AuthenticateResponse>(responseContent);
                    result.StatusCode = (int)response.StatusCode;
                    //result.Message = result.StatusMessage;

                    if(result.StatusCode == 200)
                    {
                        _accessToken = result.Token;
                    }
                    return result;
                }
                catch(Exception exception)
                {
                    var result = new AuthenticateResponse()
                    {
                        StatusCode = 500,
                        StatusMessage = exception.Message
                    };
                    return result;
                }
            }
        }
    }
}
