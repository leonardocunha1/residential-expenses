using ResidentialExpenses.Communication.Enums;

namespace ResidentialExpenses.Communication.Requests;

public class RequestRegisterCategoryJson
{
    public string Description { get; set; } = string.Empty;
    public CategoryPurposeJson Purpose { get; set; }
}
