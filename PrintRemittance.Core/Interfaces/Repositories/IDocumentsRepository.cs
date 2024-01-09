using PrintRemittance.Core.Entities;
using PrintRemittance.Core.Models;

namespace PrintRemittance.Core.Interfaces.Repositories;

public interface IDocumentsRepository
{
    Task<IEnumerable<DocumentsResultModel>> GetDocumentsAsync(GetDocumentsQueryParameter filter);
    Task<int> GetDocumentsCountAsync();
    Task<string> AddDocument(AddDocumentModel document);
    Task DeleteDocument(Guid documentId);
}
