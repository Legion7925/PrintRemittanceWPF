using Microsoft.EntityFrameworkCore;
using PrintRemittance.Core.Entities;
using PrintRemittance.Core.Exception;
using PrintRemittance.Core.Interfaces.Repositories;
using PrintRemittance.Core.Models;
using System.Reflection.Metadata.Ecma335;

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

    public async Task DeleteDocument(Guid documentId)
    {
        var document = GetDocumentById(documentId);

        if(document is null)
        {
            throw new AppException("سند مورد نظر یافت نشد");
        }

        _context.Remove(document);
        await _context.SaveChangesAsync();
    }

    private async Task<Document?> GetDocumentById(Guid documentId)
    {
        return await _context.Documents.FirstOrDefaultAsync(d=> d.Id == documentId);
    }

    public async Task<IEnumerable<DocumentsResultModel>> GetDocuments(GetDocumentsQueryParameter filter)
    {
        var documents =  _context.Documents.AsNoTracking().Where(r => r.CreatedDate.Date >= filter.StartDate.Date
            && r.CreatedDate.Date <= filter.EndDate.Date);

        if(!string.IsNullOrEmpty(filter.Destination))
        {
            documents = documents.Where(d => (d.Destination.ToLower()).Contains(filter.Destination.ToLower()));
        }

        return await documents.Select(d=> new DocumentsResultModel
        {
            CarName = d.CarName,
            CreatedDate= d.CreatedDate,
            Destination = d.Destination,
            DriverName = d.DriverName,
            FactoryName = d.FactoryName,
            Id = d.Id,
            PrintNumber = d.PrintNumber,
            Product = d.Product,
            RemittanceNumber = d.RemittanceNumber,
            
        }).ToListAsync();
    }
}
