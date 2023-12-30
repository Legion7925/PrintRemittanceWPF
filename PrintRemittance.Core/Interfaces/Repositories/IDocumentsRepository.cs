using PrintRemittance.Core.Entities;
using PrintRemittance.Core.Models;

namespace PrintRemittance.Core.Interfaces.Repositories;

public interface IDocumentsRepository
{
    Task<IEnumerable<Document>> GetDocuments();
    Task AddDocument(AddDocumentModel document);
}
