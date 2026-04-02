using ResidentialExpenses.Communication.Requests;
using ResidentialExpenses.Domain.Entities;
using AutoMapper;
using ResidentialExpenses.Communication.Responses;

namespace ResidentialExpenses.Application.Services.AutoMapper;


public class AutoMapping : Profile
{
    public AutoMapping()
    {
        RequestToDomain();
        DomainToResponse();
    }

    private void RequestToDomain()
    {
        CreateMap<RequestRegisterUserJson, User>()
            .ForMember(dest => dest.Password, opt => opt.Ignore());

        CreateMap<RequestRegisterPersonJson, Person>();
        CreateMap<RequestUpdatePersonJson, Person>();
        CreateMap<RequestRegisterCategoryJson, Category>();
        CreateMap<RequestRegisterTransactionJson, Transaction>();
    }

    private void DomainToResponse()
    {
        CreateMap<User, ResponseUserProfileJson>();
        CreateMap<Person, ResponseRegisteredPersonJson>();
        CreateMap<Person, ResponseShortPersonJson>();
        CreateMap<Category, ResponseRegisteredCategoryJson>();
        CreateMap<Category, ResponseShortCategoryJson>();
        CreateMap<Transaction, ResponseRegisteredTransactionJson>();
        CreateMap<Transaction, ResponseShortTransactionJson>();
    }
}
