using Microsoft.AspNetCore.Mvc;
using ResidentialExpenses.API.Filters;

namespace ResidentialExpenses.API.Attributes;

public class AuthenticatedUserAttribute : TypeFilterAttribute
{
    public AuthenticatedUserAttribute() : base(typeof(AuthenticatedUserFilter))
    {
    }
}
