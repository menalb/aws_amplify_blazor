using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ReceiptApp.Services
{
    public class ReceiptApi
    {        
        private readonly HttpClient _httpClient;
        private readonly TokenService _tokenService;

        public ReceiptApi(HttpClient httpClient, TokenService tokenService)
        {
            _httpClient = httpClient;
            _tokenService = tokenService;
        }

        public async Task<TResult> Get<TResult>(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = await _tokenService.BuildAuthHeader();

            var response = (await _httpClient.SendAsync(request));
            if (response.IsSuccessStatusCode)
            {
                return (await response.Content.ReadFromJsonAsync<TResult>());
            }

            throw new System.Exception(response.ReasonPhrase);
        }

        public async Task Put<TBody>(string url, TBody body)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, url);
            request.Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
            request.Headers.Authorization = await _tokenService.BuildAuthHeader();

            var response = (await _httpClient.SendAsync(request));

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }
}