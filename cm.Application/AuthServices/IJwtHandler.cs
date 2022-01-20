using System;
using System.Security.Claims;

namespace cm.Application.AuthServices
{
    public interface IJwtHandler
    {
        string GenerateAccessToken(Guid userId, string username);

        string GenerateRefreshToken();

        ClaimsPrincipal GetPrincipalFromToken(string token);
    }
}