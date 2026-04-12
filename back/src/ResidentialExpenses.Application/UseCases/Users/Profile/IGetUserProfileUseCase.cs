using ResidentialExpenses.Communication.Responses;

namespace ResidentialExpenses.Application.UseCases.Users.Profile;

public interface IGetUserProfileUseCase
{
    Task<ResponseUserProfileJson> Execute();
}
