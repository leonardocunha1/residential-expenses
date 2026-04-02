using Microsoft.IdentityModel.Tokens;
using ResidentialExpenses.Domain.Security.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ResidentialExpenses.Infrastructure.Security.Tokens;

internal class JwtTokenValidator : IAccessTokenValidator
{
    private readonly string _signingKey;

    public JwtTokenValidator(string signingKey)
    {
        _signingKey = signingKey;
    }

    public long ValidateAndGetUserIdentifier(string token)
    {
        var validationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            IssuerSigningKey = SecurityKey(),
            ClockSkew = TimeSpan.Zero,
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var principal = tokenHandler.ValidateToken(token, validationParameters, out _);

        var userIdClaim = principal.Claims.First(c => c.Type == ClaimTypes.Sid);

        return long.Parse(userIdClaim.Value);
    }

    private SymmetricSecurityKey SecurityKey()
    {
        var key = Encoding.UTF8.GetBytes(_signingKey);
        return new SymmetricSecurityKey(key);
    }
}
