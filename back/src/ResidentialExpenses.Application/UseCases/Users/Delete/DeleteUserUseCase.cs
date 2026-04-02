using ResidentialExpenses.Domain.Repositories;
using ResidentialExpenses.Domain.Repositories.User;
using ResidentialExpenses.Domain.Services.LoggedUser;

namespace ResidentialExpenses.Application.UseCases.Users.Delete;

public class DeleteUserUseCase : IDeleteUserUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUserWriteOnlyRepository _writeOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUserUseCase(
        ILoggedUser loggedUser,
        IUserWriteOnlyRepository writeOnlyRepository,
        IUnitOfWork unitOfWork)
    {
        _loggedUser = loggedUser;
        _writeOnlyRepository = writeOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute()
    {
        var user = await _loggedUser.Get();

        await _writeOnlyRepository.Delete(user);

        await _unitOfWork.Commit();
    }
}
