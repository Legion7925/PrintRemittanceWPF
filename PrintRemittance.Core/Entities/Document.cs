namespace PrintRemittance.Core.Entities;

public class Document
{
    public Guid Id { get; set; }

    public string PrintNumber { get; set; } = string.Empty;

    public string FactoryName { get; set; } = string.Empty;

    public string CarName { get; set; }= string.Empty;

    public string Product { get; set; } = string.Empty;

    public string PlateNumber { get; set; } = string.Empty;  

    public string DriverName { get; set; } = string.Empty;

    public string Destination { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; }

}
