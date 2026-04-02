using Microsoft.EntityFrameworkCore;
using ResidentialExpenses.Domain.Entities;
using ResidentialExpenses.Domain.Repositories.Person;

namespace ResidentialExpenses.Infrastructure.DataAccess.Repositories;

public class PersonRepository : IPersonReadOnlyRepository, IPersonWriteOnlyRepository, IPersonUpdateOnlyRepository
{
    private readonly ResidentialExpensesDbContext _dbContext;

    public PersonRepository(ResidentialExpensesDbContext dbContext) => _dbContext = dbContext;

    public async Task Add(Person person)
    {
        await _dbContext.People.AddAsync(person);
    }

    public async Task Delete(Person person)
    {
        var personToRemove = await _dbContext.People.FindAsync(person.Id);
        _dbContext.People.Remove(personToRemove!);
    }

    public async Task<List<Person>> GetAllByUserId(long userId)
    {
        return await _dbContext.People
            .AsNoTracking()
            .Where(p => p.Users.Any(u => u.Id == userId))
            .ToListAsync();
    }

    public async Task<Person?> GetByIdAndUserId(long id, long userId)
    {
        return await _dbContext.People
            .FirstOrDefaultAsync(p => p.Id == id && p.Users.Any(u => u.Id == userId));
    }

    public void Update(Person person)
    {
        _dbContext.People.Update(person);
    }
}
