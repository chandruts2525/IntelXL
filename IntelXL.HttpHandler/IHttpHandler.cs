using System.Security.Claims;

namespace IntelXL.HttpHandler
{

    /// <summary>
    /// The interface for HTTP client Handler used for controllers.
    /// Designed to create http request messages for communication with WebApi.
    /// </summary>
    public interface IHttpHandler
    {
        /// <summary>
        /// Asynchronous method to direct HTTP GET requests.
        /// </summary>
        /// <param name="uri">The URI where the request will be directed.</param>
        /// <param name="user">The user claim to be sent with the request.</param>
        /// <returns></returns>
        Task<HttpResponseMessage> GetAsync(string uri, ClaimsPrincipal user = null);

        Task<T> GetAsync<T>(string uri, ClaimsPrincipal user = null);

        /// <summary>
        /// Asynchronous method to direct HTTP POST requests.
        /// </summary>
        /// <param name="uri">The URI where the request will be directed.</param>
        /// <param name="value">The HttpContent to be sent with the request body.</param>
        /// <param name="user">The user claim to be sent with the request.</param>
        /// <returns></returns>
        Task<HttpResponseMessage> PostAsync(string uri, HttpContent value, ClaimsPrincipal user = null);

        /// <summary>
        /// Asynchronous method to direct HTTP POST requests.
        /// </summary>
        /// <param name="uri">The URI where the request will be directed.</param>
        /// <param name="value">The object content to be sent with the request body as JSON.</param>
        /// <param name="user">The user claim to be sent with the request.</param>
        /// <returns></returns>
        Task<HttpResponseMessage> PostAsJsonAsync<T>(string uri, T value, ClaimsPrincipal user = null);

        /// <summary>
        /// Asynchronous method to direct HTTP PUT requests.
        /// </summary>
        /// <param name="uri">The URI where the request will be directed.</param>
        /// <param name="value">The object content to be sent with the request body as JSON.</param>
        /// <param name="user">The user claim to be sent with the request.</param>
        /// <returns></returns>
        Task<HttpResponseMessage> PutAsJsonAsync<T>(string uri, T value, ClaimsPrincipal user = null);

        /// <summary>
        /// Asynchronous method to direct HTTP PUT requests.
        /// </summary>
        /// <param name="uri">The URI where the request will be directed.</param>
        /// <param name="value">The object content to be sent with the request body as JSON.</param>
        /// <param name="user">The user claim to be sent with the request.</param>
        /// <returns></returns>
        Task<HttpResponseMessage> PutAsync(string uri, HttpContent value, ClaimsPrincipal user = null);

        /// <summary>
        /// Asynchronous method to direct HTTP DELETE requests.
        /// </summary>
        /// <param name="uri">The URI where the request will be directed.</param>
        /// <param name="user">The user claim to be sent with the request.</param>
        /// <returns></returns>
        Task<HttpResponseMessage> DeleteAsync(string uri, ClaimsPrincipal user = null);

        /// <summary>
        /// Asynchronous method to add headers to the request.
        /// </summary>
        /// <param name="message">The HttpRequestMessage to be modified.</param>
        /// <param name="dictionary">A dictionary containing string-string key-value pairs
        /// representing headers to be added to the request.</param>
        /// <returns></returns>
       /* Task<bool> AddHeadersAsync(HttpRequestMessage message, IDictionary<string, string> dictionary);*/
    }
}
