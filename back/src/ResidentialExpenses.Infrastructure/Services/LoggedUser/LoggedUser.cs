using Microsoft.EntityFrameworkCore;
using ResidentialExpenses.Domain.Entities;
using ResidentialExpenses.Domain.Security.Tokens;
using ResidentialExpenses.Domain.Services.LoggedUser;
using ResidentialExpenses.Infrastructure.DataAccess;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ResidentialExpenses.Infrastructure.Services.LoggedUser;

public class LoggedUser : ILoggedUser
{
    private readonly ResidentialExpensesDbContext _dbContext;
    private readonly ITokenProvider _tokenProvider;

    public LoggedUser(ResidentialExpensesDbContext dbContext, ITokenProvider tokenProvider)
    {
        _dbContext = dbContext;
        _tokenProvider = tokenProvider;
    }

    public async Task<User> Get()
    {
        string token = _tokenProvider.TokenOnRequest();
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = tokenHandler.ReadJwtToken(token);
        var userId = long.Parse(jwtSecurityToken.Claims.First(claim => claim.Type == ClaimTypes.Sid).Value);
        return await _dbContext
            .Users
            .AsNoTracking()
            .FirstAsync(user => user.Id == userId);
    }
}