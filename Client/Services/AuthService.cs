using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using MinimalBlazorAdmin.Shared.Models;

namespace MinimalBlazorAdmin.Client.Services
{
    public class AuthService: IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ILocalStorageService _localStorageService;

        public AuthService(HttpClient httpClient,
            AuthenticationStateProvider authenticationStateProvider,
            ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
            _localStorageService = localStorageService;
        }

        public async Task<RegisterResult> RegisterAsync(RegisterDto registerDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/accounts", registerDto);
            var registerResult = await response.Content.ReadFromJsonAsync<RegisterResult>();
            return registerResult;
        }
        
        public async Task<LoginResult> LoginAsync(LoginDto loginDto)
        {
            // var loginJson = JsonSerializer.Serialize(loginDto);
            // var response = await _httpClient.PostAsync("auth/login",
            //     new StringContent(loginJson, Encoding.UTF8, "application/json"));

            var response = await _httpClient.PostAsJsonAsync("api/login", loginDto);
            
            // var loginResult = JsonSerializer.Deserialize<LoginResult>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true});

            var loginResult = await response.Content.ReadFromJsonAsync<LoginResult>();
            
            if (!response.IsSuccessStatusCode)
            {
                return loginResult;
            }

            await _localStorageService.SetItemAsync("authToken", loginResult.Token);
            ((CustomAuthProvider)_authenticationStateProvider).MarkUserAuthenticated(loginResult.Token);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", loginResult.Token);

            return loginResult;
        }

        public async Task LogoutAsync()
        {
            await _localStorageService.RemoveItemAsync("authToken");
            ((CustomAuthProvider)_authenticationStateProvider).MarkUserLoggedOut();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }

    public interface IAuthService
    {
        public Task<RegisterResult> RegisterAsync(RegisterDto registerDto);
        public Task<LoginResult> LoginAsync(LoginDto loginDto);
        public Task LogoutAsync();
    }
}