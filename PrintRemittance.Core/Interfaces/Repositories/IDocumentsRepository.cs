using PrintRemittance.Core.Entities;
using PrintRemittance.Core.Models;

namespace PrintRemittance.Core.Interfaces.Repositories;

public interface IDocumentsRepository
{
    Task<IEnumerable<DocumentsResultModel>> GetDocuments(GetDocumentsQueryParameter filter);
    Task AddDocument(AddDocumentModel document);
}
