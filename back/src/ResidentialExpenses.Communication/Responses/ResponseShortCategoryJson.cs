using ResidentialExpenses.Communication.Enums;

namespace ResidentialExpenses.Communication.Responses;

public class ResponseShortCategoryJson
{
    public long Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public CategoryPurposeJson Purpose { get; set; }
}
