using PrintRemittance.Core.Entities;
using PrintRemittance.Core.Models;

namespace PrintRemittance.Core.Interfaces.Repositories;

public interface IDocumentsRepository
{
    IEnumerable<DocumentsResultModel> GetDocuments(GetDocumentsQueryParameter filter);
    int GetDocumentsCount(GetDocumentsQueryParameter filter);
    Task<string> AddDocument(AddDocumentModel document);
    Task DeleteDocument(Guid documentId);
}
