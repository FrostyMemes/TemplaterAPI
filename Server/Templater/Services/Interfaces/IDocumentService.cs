using Templater.DTO.DocumentDTO;
using Templater.DTO.TemplateDTO;

namespace Templater.Services.Interfaces;

public interface IDocumentService
{
    Task<IEnumerable<AllDocumentsDto>> GetAllDocumentsAsync();
    Task<GetDocumentDto> GetDocumentAsync(Guid id);
    Task<Document> DeleteDocumentAsync(Guid id);
}