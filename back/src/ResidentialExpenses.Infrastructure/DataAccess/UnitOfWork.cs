using ResidentialExpenses.Domain.Repositories;

namespace ResidentialExpenses.Infrastructure.DataAccess;

public class UnitOfWork : IUnitOfWork
{
    private readonly ResidentialExpensesDbContext _dbContext;

    public UnitOfWork(ResidentialExpensesDbContext dbContext) => _dbContext = dbContext;

    public async Task Commit() => await _dbContext.SaveChangesAsync();
}