using System.ComponentModel.DataAnnotations;
using ResidentialExpenses.Communication.Enums;

namespace ResidentialExpenses.Communication.Requests;

public class RequestRegisterCategoryJson
{
    [Required]
    public string Description { get; set; } = string.Empty;
    [Required]
    public CategoryPurposeJson Purpose { get; set; }
}
