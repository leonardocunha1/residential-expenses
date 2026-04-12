using ResidentialExpenses.Communication.Responses;
using ResidentialExpenses.Domain.Services.LoggedUser;

namespace ResidentialExpenses.Application.UseCases.Users.Profile;

public class GetUserProfileUseCase : IGetUserProfileUseCase
{
    private readonly ILoggedUser _loggedUser;

    public GetUserProfileUseCase(ILoggedUser loggedUser)
    {
        _loggedUser = loggedUser;
    }

    public async Task<ResponseUserProfileJson> Execute()
    {
        var user = await _loggedUser.Get();

        return new ResponseUserProfileJson
        {
            Name = user.Name,
            Email = user.Email
        };
    }
}
