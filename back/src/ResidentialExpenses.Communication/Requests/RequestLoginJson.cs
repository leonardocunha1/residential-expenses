using System.ComponentModel.DataAnnotations;

namespace ResidentialExpenses.Communication.Requests;

public class RequestLoginJson
{
    [Required]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
}
