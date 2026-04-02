using Microsoft.EntityFrameworkCore;
using ResidentialExpenses.Domain.Entities;

namespace ResidentialExpenses.Infrastructure.DataAccess;

public class ResidentialExpensesDbContext : DbContext
{
    public ResidentialExpensesDbContext(DbContextOptions<ResidentialExpensesDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Person> People => Set<Person>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Transaction> Transactions => Set<Transaction>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // User
        builder.Entity<User>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).IsRequired().HasMaxLength(200);
            e.Property(x => x.Email).IsRequired().HasMaxLength(200);
            e.HasIndex(x => x.Email).IsUnique();
            e.Property(x => x.Password).IsRequired();
            e.Property(x => x.CreatedAt).HasDefaultValueSql("(now() at time zone 'utc')");

            e.HasMany(x => x.People)
             .WithMany(x => x.Users)
             .UsingEntity(j => j.ToTable("user_person"));
        });

        // Person
        builder.Entity<Person>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).IsRequired().HasMaxLength(200);
            e.Property(x => x.Age).IsRequired();
        });

        // Category
        builder.Entity<Category>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Description).IsRequired().HasMaxLength(400);
            e.Property(x => x.Purpose).IsRequired();
        });

        // Transaction
        builder.Entity<Transaction>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Description).IsRequired().HasMaxLength(400);
            e.Property(x => x.Value).IsRequired().HasColumnType("decimal(18,2)");
            e.Property(x => x.Type).IsRequired();

            e.HasOne(x => x.Category)
             .WithMany(x => x.Transactions)
             .HasForeignKey(x => x.CategoryId)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.Person)
             .WithMany(x => x.Transactions)
             .HasForeignKey(x => x.PersonId)
             .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
