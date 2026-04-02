namespace ResidentialExpenses.Communication.Responses;

public class ResponseErrorJson
{
    public List<string> ErrorMessages { get; set; }
    public bool? TokenIsExpired { get; set; }

    public ResponseErrorJson(string errorMessage)
    {
        ErrorMessages = [errorMessage];
    }

    public ResponseErrorJson(List<string> errorMessages)
    {
        ErrorMessages = errorMessages;
    }
}
