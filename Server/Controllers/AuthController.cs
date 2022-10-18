using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BlazorJwtLogin.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration configuration;

    public AuthController(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    [HttpPost("Login")]
    public async Task<ActionResult<UserToken>> Login([FromBody] UserInfo userInfo)
    {
        //驗證方式純為Demo用
        if (userInfo.Email == "abc@gmail.com" && userInfo.Password == "abc123")
        {
            return BuildToken(userInfo);
        }

        return BadRequest("Login failed");
    }

    /// <summary>
    /// 建立Token
    /// </summary>
    /// <param name="userInfo"></param>
    /// <returns></returns>
    private UserToken BuildToken(UserInfo userInfo)
    {
        //記在jwt payload中的聲明，可依專案需求自訂Claim
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, userInfo.Email),
            new Claim(ClaimTypes.Role, "admin")
        };

        //取得對稱式加密 JWT Signature 的金鑰
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwt:Key"]));
        var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //設定token有效期限
        DateTime expireTime = DateTime.Now.AddMinutes(30);

        //產生token
        JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
            issuer: null,
            audience: null,
            claims: claims,
            expires: expireTime,
            signingCredentials: credential
        );
        string jwtToken = jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);

        //建立UserToken物件後回傳client
        UserToken userToken = new UserToken()
        {
            StatusCode = System.Net.HttpStatusCode.OK,
            token = jwtToken,
            ExpireTime = expireTime
        };

        return userToken;
    }
}

public class UserToken
{
    public string token { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public DateTime ExpireTime { get; set; }
}

public class UserInfo
{
    public string Email { get; set; }
    public string Password { get; set; }
}