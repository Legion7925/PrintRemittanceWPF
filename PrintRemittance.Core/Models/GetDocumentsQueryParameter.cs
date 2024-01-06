namespace PrintRemittance.Core.Models;

public class GetDocumentsQueryParameter : QueryParametersBase
{
    public DateTime StartDate { get; set; } = DateTime.Now.AddDays(-1);

    public DateTime EndDate { get; set; } = DateTime.Now.AddDays(1);

    public string? Destination { get; set; }

}
