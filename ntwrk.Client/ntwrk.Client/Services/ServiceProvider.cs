
/* Unmerged change from project 'ntwrk.Client (net8.0-ios)'
Before:
using ntwrk.Client.Services.Authenticate;
using Newtonsoft.Json;
After:
using Newtonsoft.Json;
using ntwrk.Client.Helpers;
using ntwrk.Client.Services.Authenticate;
*/

/* Unmerged change from project 'ntwrk.Client (net8.0-maccatalyst)'
Before:
using ntwrk.Client.Services.Authenticate;
using Newtonsoft.Json;
After:
using Newtonsoft.Json;
using ntwrk.Client.Helpers;
using ntwrk.Client.Services.Authenticate;
*/

/* Unmerged change from project 'ntwrk.Client (net8.0-windows10.0.19041.0)'
Before:
using ntwrk.Client.Services.Authenticate;
using Newtonsoft.Json;
After:
using Newtonsoft.Json;
using ntwrk.Client.Helpers;
using ntwrk.Client.Services.Authenticate;
*/

/* Unmerged change from project 'ntwrk.Client (net8.0-ios)'
Before:
using System.Threading.Tasks;
using ntwrk.Client.Helpers;
After:
using System.Threading.Tasks;
*/

/* Unmerged change from project 'ntwrk.Client (net8.0-maccatalyst)'
Before:
using System.Threading.Tasks;
using ntwrk.Client.Helpers;
After:
using System.Threading.Tasks;
*/

/* Unmerged change from project 'ntwrk.Client (net8.0-windows10.0.19041.0)'
Before:
using System.Threading.Tasks;
using ntwrk.Client.Helpers;
After:
using System.Threading.Tasks;
*/
namespace ntwrk.Client.Services
{
    public class ServiceProvider
    {
        public string _accessToken = "";
        private DevHttpsConnectionHelper _devSslHelper;
        public ServiceProvider()
        {
            _devSslHelper = new DevHttpsConnectionHelper(sslPort: 7136);
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest request)
        {
            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = HttpMethod.Post;
            httpRequestMessage.RequestUri = new Uri(_devSslHelper.DevServerRootUrl + "/Authenticate/Authenticate");

            if (request != null)
            {
                string jsonContent = JsonConvert.SerializeObject(request);
                var httpContent = new StringContent(jsonContent, encoding: Encoding.UTF8, "application/json"); ;
                httpRequestMessage.Content = httpContent;
            }

            try
            {
                var response = await _devSslHelper.HttpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<AuthenticateResponse>(responseContent);
                result.StatusCode = (int)response.StatusCode;

                if (result.StatusCode == 200)
                {
                    _accessToken = result.Token;
                }
                return result;
            }
            catch (Exception ex)
            {
                var result = new AuthenticateResponse
                {
                    StatusCode = 500,
                    StatusMessage = ex.Message
                };
                return result;
            }
        }
        public async Task<RegisterResponse> Register(RegisterRequest request)
        {
            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = HttpMethod.Post;
            httpRequestMessage.RequestUri = new Uri(_devSslHelper.DevServerRootUrl + "/Register/Register"); // Replace with your actual API endpoint

            if (request != null)
            {
                string jsonContent = JsonConvert.SerializeObject(request);
                var httpContent = new StringContent(jsonContent, encoding: Encoding.UTF8, "application/json");
                httpRequestMessage.Content = httpContent;
            }

            try
            {
                var response = await _devSslHelper.HttpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<RegisterResponse>(responseContent);
                result.StatusCode = (int)response.StatusCode;

                return result;
            }
            catch (Exception ex)
            {
                var result = new RegisterResponse
                {
                    StatusCode = 500,
                    StatusMessage = ex.Message
                };
                return result;
            }
        }

        public async Task<TResponse> CallWebApi<TRequest, TResponse>(
            string apiUrl, HttpMethod httpMethod, TRequest request) where TResponse : BaseResponse
        {
            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = HttpMethod.Post;
            httpRequestMessage.RequestUri = new Uri(_devSslHelper.DevServerRootUrl + apiUrl);
            httpRequestMessage.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _accessToken);

            if (request != null)
            {
                string jsonContent = JsonConvert.SerializeObject(request);
                var httpContent = new StringContent(jsonContent, encoding: Encoding.UTF8, "application/json");
                httpRequestMessage.Content = httpContent;
            }

            try
            {
                var response = await _devSslHelper.HttpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<TResponse>(responseContent);
                result.StatusCode = (int)response.StatusCode;

                return result;
            }
            catch (Exception ex)
            {
                var result = Activator.CreateInstance<TResponse>();
                result.StatusCode = 500;
                result.StatusMessage = ex.Message;
                return result;
            }
        }
    }
}
