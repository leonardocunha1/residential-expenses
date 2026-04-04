using Moq;
using ResidentialExpenses.Domain.Entities;
using ResidentialExpenses.Domain.Repositories.Transaction;

namespace CommonTestUtilities.Repositories;

public class TransactionReadOnlyRepositoryBuilder
{
    private readonly Mock<ITransactionReadOnlyRepository> _repository;

    public TransactionReadOnlyRepositoryBuilder()
    {
        _repository = new Mock<ITransactionReadOnlyRepository>();
    }

    public TransactionReadOnlyRepositoryBuilder GetAllByPersonId(long personId, List<Transaction> transactions)
    {
        _repository.Setup(r => r.GetAllByPersonId(personId)).ReturnsAsync(transactions);
        return this;
    }

    public TransactionReadOnlyRepositoryBuilder GetAllByPersonIds(List<long> personIds, List<Transaction> transactions)
    {
        _repository.Setup(r => r.GetAllByPersonIds(personIds)).ReturnsAsync(transactions);
        return this;
    }

    public ITransactionReadOnlyRepository Build() => _repository.Object;
}
