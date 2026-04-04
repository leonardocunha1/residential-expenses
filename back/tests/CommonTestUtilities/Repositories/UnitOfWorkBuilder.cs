using Moq;
using ResidentialExpenses.Domain.Repositories;

namespace CommonTestUtilities.Repositories;

public class UnitOfWorkBuilder
{
    private readonly Mock<IUnitOfWork> _unitOfWork;

    public UnitOfWorkBuilder()
    {
        _unitOfWork = new Mock<IUnitOfWork>();
    }

    public Mock<IUnitOfWork> Mock => _unitOfWork;
    public IUnitOfWork Build() => _unitOfWork.Object;
}
