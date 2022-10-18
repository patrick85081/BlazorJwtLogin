using BlazorJwtLogin.Shared;

namespace BlazorJwtLogin.Client.Services;

public interface IAuthService
{
    Task<bool> LoginAsync(UserInfo userInfo);
    Task LogoutAsync();
}