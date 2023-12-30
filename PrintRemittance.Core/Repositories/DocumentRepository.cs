using Microsoft.EntityFrameworkCore;
using PrintRemittance.Core.Entities;
using PrintRemittance.Core.Interfaces.Repositories;
using PrintRemittance.Core.Models;

namespace PrintRemittance.Core.Repositories;

public class DocumentRepository : IDocumentsRepository
{
    private readonly PrintRemittanceDbContext _context;
    public DocumentRepository(PrintRemittanceDbContext context)
    {
        _context = context;
    }
    public async Task AddDocument(AddDocumentModel document)
    {
        await _context.AddAsync(new Document
        {
            CarName = document.CarName,
            CreatedDate = document.CreatedDate,
            Destination  = document.Destination,
            DriverName = document.DriverName,
            FactoryName = document.FactoryName,
            Id = new Guid(),
            PrintNumber = document.PrintNumber,
            Product = document.Product,
            RemittanceNumber = document.RemittanceNumber,
        });
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Document>> GetDocuments()
    {
        return await _context.Documents.AsNoTracking().ToListAsync();
    }
}
