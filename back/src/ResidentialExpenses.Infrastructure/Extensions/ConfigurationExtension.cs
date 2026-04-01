using Microsoft.Extensions.Configuration;

namespace ResidentialExpenses.Infrastructure.Extensions;

public static class ConfigurationExtension
{
    public static bool IsUnitTestEnviroment(this IConfiguration configuration) => configuration.GetValue<bool>("InMemoryTest");
    public static string ConnectionString(this IConfiguration configuration)
    {
        return configuration.GetConnectionString("Connection")!;
    }
}
