using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ResidentialExpenses.Application.UseCases.Users.Register;
using Microsoft.Extensions.Configuration;
using ResidentialExpenses.Application.Services.AutoMapper;

namespace ResidentialExpenses.Application;


public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        AddAutoMapper(services);
        AddUseCases(services);
    }

    private static void AddAutoMapper(IServiceCollection services)
    {
        services.AddScoped<IMapper>(sp =>
        {
            var loggerFactory = sp.GetRequiredService<ILoggerFactory>();

            var config = new MapperConfiguration(
                cfg =>
                {
                    cfg.AddProfile(new AutoMapping());
                },
                loggerFactory
            );

            return config.CreateMapper();
        });
    }

    private static void AddUseCases(IServiceCollection services)
    {
        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
    }
}