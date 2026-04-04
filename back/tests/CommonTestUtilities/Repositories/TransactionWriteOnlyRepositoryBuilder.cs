using Moq;
using ResidentialExpenses.Domain.Entities;
using ResidentialExpenses.Domain.Repositories.Transaction;

namespace CommonTestUtilities.Repositories;

public class TransactionWriteOnlyRepositoryBuilder
{
    private readonly Mock<ITransactionWriteOnlyRepository> _repository;

    public TransactionWriteOnlyRepositoryBuilder()
    {
        _repository = new Mock<ITransactionWriteOnlyRepository>();
    }

    public Mock<ITransactionWriteOnlyRepository> Mock => _repository;
    public ITransactionWriteOnlyRepository Build() => _repository.Object;
}
