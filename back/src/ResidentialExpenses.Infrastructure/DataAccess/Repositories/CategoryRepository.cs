using Microsoft.EntityFrameworkCore;
using ResidentialExpenses.Domain.Entities;
using ResidentialExpenses.Domain.Repositories.Category;

namespace ResidentialExpenses.Infrastructure.DataAccess.Repositories;

public class CategoryRepository : ICategoryReadOnlyRepository, ICategoryWriteOnlyRepository
{
    private readonly ResidentialExpensesDbContext _dbContext;

    public CategoryRepository(ResidentialExpensesDbContext dbContext) => _dbContext = dbContext;

    public async Task Add(Category category)
    {
        await _dbContext.Categories.AddAsync(category);
    }

    public async Task<List<Category>> GetAll()
    {
        return await _dbContext.Categories.AsNoTracking().ToListAsync();
    }

    public async Task<Category?> GetById(long id)
    {
        return await _dbContext.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
    }
}
