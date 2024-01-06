namespace PrintRemittance.Core.Models;

public class AddDocumentModel
{
    public string FactoryName { get; set; } = string.Empty;

    public string CarName { get; set; } = string.Empty;

    public string Product { get; set; } = string.Empty;

    public string PlateNumber { get; set; } = string.Empty;

    public string DriverName { get; set; } = string.Empty;

    public string Destination { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; }
}
