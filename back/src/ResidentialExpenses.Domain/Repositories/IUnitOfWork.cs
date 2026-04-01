namespace ResidentialExpenses.Domain.Repositories;

public interface IUnitOfWork
{
    public Task Commit();
}