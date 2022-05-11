using System;
using System.Security.Claims;

namespace Utilities.Auths
{
    public interface IJwtHandler
    {
        string GenerateAccessToken(Guid userId, string username, object permission);

        string GenerateRefreshToken();

        ClaimsPrincipal GetPrincipalFromToken(string token);
    }
}