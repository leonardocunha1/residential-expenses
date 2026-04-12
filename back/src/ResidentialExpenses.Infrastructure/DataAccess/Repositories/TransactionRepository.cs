using Microsoft.EntityFrameworkCore;
using ResidentialExpenses.Domain.Entities;
using ResidentialExpenses.Domain.Repositories.Transaction;

namespace ResidentialExpenses.Infrastructure.DataAccess.Repositories;

public class TransactionRepository : ITransactionReadOnlyRepository, ITransactionWriteOnlyRepository
{
    private readonly ResidentialExpensesDbContext _dbContext;

    public TransactionRepository(ResidentialExpensesDbContext dbContext) => _dbContext = dbContext;

    public async Task Add(Transaction transaction)
    {
        await _dbContext.Transactions.AddAsync(transaction);
    }

    public async Task<List<Transaction>> GetAllByPersonId(long personId)
    {
        return await _dbContext.Transactions
            .AsNoTracking()
            .Where(t => t.PersonId == personId)
            .ToListAsync();
    }

    public async Task<List<Transaction>> GetAllByPersonIds(List<long> personIds)
    {
        return await _dbContext.Transactions
            .AsNoTracking()
            .Where(t => personIds.Contains(t.PersonId))
            .ToListAsync();
    }
}
