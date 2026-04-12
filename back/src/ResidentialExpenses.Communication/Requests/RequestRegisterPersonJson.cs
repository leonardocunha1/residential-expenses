using System.ComponentModel.DataAnnotations;

namespace ResidentialExpenses.Communication.Requests;

public class RequestRegisterPersonJson
{
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public int Age { get; set; }
}
