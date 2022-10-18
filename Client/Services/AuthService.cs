using System.Text;
using System.Text.Json;
using Blazored.LocalStorage;
using BlazorJwtLogin.Client.Auths;
using BlazorJwtLogin.Shared;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorJwtLogin.Client.Services;

public class AuthService : IAuthService
{
    private readonly ILocalStorageService localStorageService;
    private readonly HttpClient httpClient;
    private readonly AuthenticationStateProvider authenticationStateProvider;

    public AuthService(ILocalStorageService localStorageService, HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider)
    {
        this.localStorageService = localStorageService;
        this.httpClient = httpClient;
        this.authenticationStateProvider = authenticationStateProvider;
    }

    public async Task<bool> LoginAsync(UserInfo userInfo)
    {
        bool result = false;

        var json = JsonSerializer.Serialize(userInfo);
        // var json = JsonConvert.SerializeObject(userInfo);
        HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync("/api/Auth/Login", httpContent);

        if (response.IsSuccessStatusCode)
        {
            var resContent = await response.Content.ReadAsStringAsync();
            // UserToken userToken = JsonConvert.DeserializeObject<UserToken>(resContent);
            UserToken userToken = JsonSerializer.Deserialize<UserToken>(resContent);

            await localStorageService.SetItemAsync<string>("authToken", userToken.token);

            ((JwtAuthenticationStateProvider)authenticationStateProvider).NotifyUserAuthentication(userToken.token);

            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", userToken.token);

            result = true;
        }

        return result;
    }

    public async Task LogoutAsync()
    {
        await localStorageService.RemoveItemAsync("authToken");
        ((JwtAuthenticationStateProvider)authenticationStateProvider).NotifyUserLogOut();
        httpClient.DefaultRequestHeaders.Authorization = null;
    }
}