using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using ReceiptApp.Configuration;

namespace ReceiptApp.Services
{
    public class TokenService
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly AuthenticationConfig _config;
        public TokenService(IJSRuntime jsRuntime, AuthenticationConfig config)
        {
            _jsRuntime = jsRuntime;
            _config = config;
        }

        public async Task<string> GetIdToken()
        {
            var url = $"{_config.Authority}:{_config.ClientId}";
            var userDataKey = $"oidc.user:{url}";

            var userDataString = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", userDataKey);
            var userData = System.Text.Json.JsonSerializer.Deserialize<UserData>(userDataString);

            return userData.id_token;
        }

        public async Task<AuthenticationHeaderValue> BuildAuthHeader()
        {
            var token = await GetIdToken();
            return new AuthenticationHeaderValue("Bearer", token);
        }
    }

    class UserData
    {
        public string id_token { get; set; }
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public string token_type { get; set; }
        public string scope { get; set; }
        public UserProfile profile { get; set; }
        public int expires_at { get; set; }
    }

    public class UserProfile
    {
        public string username { get; set; }
        public string sub { get; set; }
    }
}