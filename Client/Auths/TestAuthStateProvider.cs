using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorJwtLogin.Client.Auths;

public class TestAuthStateProvider : AuthenticationStateProvider
{
    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var anonymous = new ClaimsIdentity();

        return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(anonymous)));
    }
}