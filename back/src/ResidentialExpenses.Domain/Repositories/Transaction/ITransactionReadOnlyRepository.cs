namespace ResidentialExpenses.Domain.Repositories.Transaction;

public interface ITransactionReadOnlyRepository
{
    Task<List<Entities.Transaction>> GetAllByPersonId(long personId);
    Task<List<Entities.Transaction>> GetAllByPersonIds(List<long> personIds);
}
