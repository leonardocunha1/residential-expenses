namespace ResidentialExpenses.Communication.Responses;

public class ResponseApiJson<T>
{
    public bool Success { get; set; } = true;
    public T? Data { get; set; }
    public List<string> Errors { get; set; } = [];
    public ResponseMetadataJson Metadata { get; set; } = new();
}
