using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace IntelXL.HttpHandler
{
    public class HttpHandler : IHttpHandler
    {
        //private IDictionary<string, string> headers;
        /// <summary>
        /// Constructor method for HttpClientHandler
        /// </summary>
        //public HttpHandler(string apiKeyName, string keyValue)
        //{
        //    headers = new Dictionary<string, string>
        //    {
        //        { apiKeyName, keyValue },
        //        { "Ocp-Apim-Trace", "true" }
        //    };
        //}

        /// <summary>
        /// Asynchronous method to direct HTTP GET requests.
        /// </summary>
        /// <param name="uri">The API URI endpoint where the request will be directed.</param>
        /// <param name="user">The user claim to be sent with the request.</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> GetAsync(string uri, ClaimsPrincipal user = null)
        {
            using (HttpRequestMessage Message = new HttpRequestMessage())
            {
                Message.Method = HttpMethod.Get;
                Message.RequestUri = new Uri(uri);

                var returnValue = await SendAsync(Message, user);

                return returnValue;
            }
        }

        public async Task<T> GetAsync<T>(string uri, ClaimsPrincipal user = null)
        {
            HttpResponseMessage responseMessage = await GetAsync(uri, user);
            T response = JsonSerializer.Deserialize<T>(await responseMessage.Content.ReadAsStringAsync());
            return response;
        }

        /// <summary>
        /// Asynchronous method to direct HTTP POST requests.
        /// </summary>
        /// <param name="uri">The API URI endpoint where the request will be directed.</param>
        /// <param name="value">The HttpContent to be sent with the request body.</param>
        /// <param name="user">The user claim to be sent with the request.</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> PostAsync(string uri, HttpContent value, ClaimsPrincipal user = null)
        {
            using (HttpRequestMessage Message = new HttpRequestMessage())
            {
                Message.Method = HttpMethod.Post;
                Message.RequestUri = new Uri(uri);
                Message.Content = value;

                var returnValue = await SendAsync(Message, user);

                return returnValue;
            }
        }

        /// <summary>
        /// Asynchronous method to direct HTTP POST requests.
        /// </summary>
        /// <param name="uri">The API URI endpoint where the request will be directed.</param>
        /// <param name="value">The object content to be sent with the request body as JSON.</param>
        /// <param name="user">The user claim to be sent with the request.</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> PostAsJsonAsync<T>(string uri, T value, ClaimsPrincipal user = null)
        {
            using (HttpRequestMessage Message = new HttpRequestMessage())
            {
                Message.Method = HttpMethod.Post;
                Message.RequestUri = new Uri(uri);

                var json = JsonSerializer.Serialize(value);

                Message.Content = new StringContent(json, Encoding.UTF8, "application/json");

                var returnValue = await SendAsync(Message, user);

                return returnValue;
            }
        }

        /// <summary>
        /// Asynchronous method to direct HTTP PUT requests.
        /// </summary>
        /// <param name="uri">The API URI endpoint where the request will be directed.</param>
        /// <param name="value">The object content to be sent with the request body as JSON.</param>
        /// <param name="user">The user claim to be sent with the request.</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> PutAsJsonAsync<T>(string uri, T value, ClaimsPrincipal user = null)
        {
            using (HttpRequestMessage Message = new HttpRequestMessage())
            {
                Message.Method = HttpMethod.Put;
                Message.RequestUri = new Uri(uri);

                var json = JsonSerializer.Serialize(value);

                Message.Content = new StringContent(json, Encoding.UTF8, "application/json");

                var returnValue = await SendAsync(Message, user);

                return returnValue;
            }
        }

        public async Task<HttpResponseMessage> PutAsync(string uri, HttpContent value, ClaimsPrincipal user = null)
        {
            using (HttpRequestMessage Message = new HttpRequestMessage())
            {
                Message.Method = HttpMethod.Put;
                Message.RequestUri = new Uri(uri);
                Message.Content = value;

                var returnValue = await SendAsync(Message, user);

                return returnValue;
            }
        }

        /// <summary>
        /// Asynchronous method to direct HTTP DELETE requests.
        /// </summary>
        /// <param name="uri">The API URI endpoint where the request will be directed.</param>
        /// <param name="user">The user claim to be sent with the request.</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> DeleteAsync(string uri, ClaimsPrincipal user = null)
        {
            using (HttpRequestMessage message = new HttpRequestMessage())
            {
                message.Method = HttpMethod.Delete;
                message.RequestUri = new Uri(uri);

                var returnValue = await SendAsync(message, user);

                return returnValue;

            }
        }

        /// <summary>
        /// Asynchronous method to add headers to the request.
        /// </summary>
        /// <param name="message">The HttpRequestMessage to be modified.</param>
        /// <param name="dictionary">A dictionary containing string-string key-value pairs
        /// representing headers to be added to the request.</param>
        /// <returns></returns>
       /* public async Task<bool> AddHeadersAsync(HttpRequestMessage message, IDictionary<string, string> dictionary)
        {
            try
            {
                await Task.Run(() =>
                {
                    foreach (var item in dictionary)
                    {
                        if (message.Headers.Contains(item.Key)) message.Headers.Remove(item.Key);
                        message.Headers
                            .Add(item.Key, item.Value);
                    }
                });
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }*/

        /// <summary>
        /// This method is intended to be used as a validation mechanism for the 
        /// administrative headers. However, this can 
        /// be used as a simple tool to hash any string value. 
        /// </summary>
        /// <param name="adminHeaders">The string to hash</param>
        /// <returns>SHA256 hash string</returns>
        private string GetHash(string adminHeaders)
        {
            string hash = string.Empty;
            string headers = adminHeaders.ToString();
            SHA256Managed hashString = new SHA256Managed();
            byte[] hashBytes = hashString.ComputeHash(Encoding.UTF8.GetBytes(headers));
            foreach (byte b in hashBytes)
            {
                hash += string.Format("{0:x2}", b);
            }
            return hash;
        }

        private async Task<HttpResponseMessage> SendAsync(HttpRequestMessage message, ClaimsPrincipal user = null)
        {
            //await AddHeadersAsync(message, headers);

            // this line comes from identityModel and is used for setting various tokens
            //if (!string.IsNullOrWhiteSpace(_accessToken)) message.SetBearerToken(_accessToken);

            HttpResponseMessage responseMessage = new HttpResponseMessage();

            using (HttpClient client = new HttpClient())
            {
                responseMessage = await client.SendAsync(message);
            }

            return responseMessage;
        }


    }
}
