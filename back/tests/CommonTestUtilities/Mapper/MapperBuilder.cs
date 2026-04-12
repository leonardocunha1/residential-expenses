using AutoMapper;
using Microsoft.Extensions.Logging;
using ResidentialExpenses.Application.Services.AutoMapper;

namespace CommonTestUtilities.Mapper;

public class MapperBuilder
{
    public static IMapper Build()
    {
        var loggerFactory = LoggerFactory.Create(builder => { });

        var configuration = new MapperConfiguration(
            cfg => cfg.AddProfile<AutoMapping>(),
            loggerFactory);

        return configuration.CreateMapper();
    }
}
