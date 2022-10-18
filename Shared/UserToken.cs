using System.Net;

namespace BlazorJwtLogin.Shared;

public class UserToken
{
    public string token { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public DateTime ExpireTime { get; set; }
}