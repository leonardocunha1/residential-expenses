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
    }

    private void DomainToResponse()
    {
        CreateMap<User, ResponseUserProfileJson>();
    }
}
