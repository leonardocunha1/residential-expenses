using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ResidentialExpenses.Application.UseCases.Users.Delete;
using ResidentialExpenses.Application.UseCases.Users.Login;
using ResidentialExpenses.Application.UseCases.Users.Register;
using ResidentialExpenses.Application.UseCases.Users.Update;
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
        services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
        services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
        services.AddScoped<IDeleteUserUseCase, DeleteUserUseCase>();
    }
}