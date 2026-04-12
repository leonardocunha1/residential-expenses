using System.ComponentModel.DataAnnotations;

namespace ResidentialExpenses.Communication.Requests;

public class RequestUpdateUserJson
{
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string Email { get; set; } = string.Empty;
    public string OldPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}
