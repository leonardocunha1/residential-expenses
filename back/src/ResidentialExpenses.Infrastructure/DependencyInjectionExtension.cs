using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ResidentialExpenses.Domain.Repositories;
using ResidentialExpenses.Domain.Repositories.Category;
using ResidentialExpenses.Domain.Repositories.Person;
using ResidentialExpenses.Domain.Repositories.Transaction;
using ResidentialExpenses.Domain.Repositories.User;
using ResidentialExpenses.Domain.Security.Cryptography;
using ResidentialExpenses.Domain.Security.Tokens;
using ResidentialExpenses.Domain.Services.LoggedUser;
using ResidentialExpenses.Infrastructure.DataAccess;
using ResidentialExpenses.Infrastructure.DataAccess.Repositories;
using ResidentialExpenses.Infrastructure.Extensions;
using ResidentialExpenses.Infrastructure.Security.Tokens;
using ResidentialExpenses.Infrastructure.Services.LoggedUser;

namespace ResidentialExpenses.Infrastructure;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IPasswordEncripter, Security.Cryptography.BCrypt>();
        services.AddScoped<ILoggedUser, LoggedUser>();

        AddRepositories(services);
        AddToken(services, configuration);

        if (configuration.IsUnitTestEnviroment())
            return;

        AddDbContext(services, configuration);
    }

    public static void ApplyMigrations(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ResidentialExpensesDbContext>();
        context.Database.Migrate();
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserReadOnlyRepository, UserRepository>();
        services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
        services.AddScoped<IUserUpdateOnlyRepository, UserRepository>();
        services.AddScoped<IPersonReadOnlyRepository, PersonRepository>();
        services.AddScoped<IPersonWriteOnlyRepository, PersonRepository>();
        services.AddScoped<IPersonUpdateOnlyRepository, PersonRepository>();
        services.AddScoped<ICategoryReadOnlyRepository, CategoryRepository>();
        services.AddScoped<ICategoryWriteOnlyRepository, CategoryRepository>();
        services.AddScoped<ITransactionReadOnlyRepository, TransactionRepository>();
        services.AddScoped<ITransactionWriteOnlyRepository, TransactionRepository>();
    }

    private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.ConnectionString();
        services.AddDbContext<ResidentialExpensesDbContext>(config => config.UseNpgsql(connectionString));
    }

    private static void AddToken(IServiceCollection services, IConfiguration configuration)
    {
        var expirationTimeMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpiresMinutes");
        var signingKey = configuration.GetValue<string>("Settings:Jwt:SigningKey");

        services.AddScoped<IAccessTokenGenerator>(config => new JwtTokenGenerator(expirationTimeMinutes, signingKey!));
        services.AddScoped<IAccessTokenValidator>(config => new JwtTokenValidator(signingKey!));
    }
}
