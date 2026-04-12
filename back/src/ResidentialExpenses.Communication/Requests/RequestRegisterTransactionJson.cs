using System.ComponentModel.DataAnnotations;
using ResidentialExpenses.Communication.Enums;

namespace ResidentialExpenses.Communication.Requests;

public class RequestRegisterTransactionJson
{
    [Required]
    public string Description { get; set; } = string.Empty;
    [Required]
    public decimal Value { get; set; }
    [Required]
    public TransactionTypeJson Type { get; set; }
    [Required]
    public long CategoryId { get; set; }
    [Required]
    public long PersonId { get; set; }
}
