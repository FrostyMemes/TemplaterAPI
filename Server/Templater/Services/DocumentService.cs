using Templater.DTO.DocumentDTO;
using Templater.Services.Interfaces;

namespace Templater.Services;

public class DocumentService: IDocumentService
{
    private readonly ApplicationDbContext _dbContext;

    public DocumentService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IEnumerable<AllDocumentsDto>> GetAllDocumentsAsync()
    {
        return await _dbContext.Documents
            .Select(document => new AllDocumentsDto()
            {
                Id = document.Id.ToString(),
                FileName = document.FileName,
            })
            .ToListAsync();
    }

    public async Task<GetDocumentDto> GetDocumentAsync(Guid id)
    {
        #pragma warning disable CS8603
        return await _dbContext.Documents
            .Where(document => document.Id == id)
            .Select(document => new GetDocumentDto()
            {
                Id = document.Id.ToString(),
                FileName = document.FileName,
            })
            .FirstOrDefaultAsync();
    }

    public async Task<Document> DeleteDocumentAsync(Guid id)
    {
        var document = await _dbContext.Documents.FindAsync(id);
        if (document != null)
        {
            _dbContext.Documents.Remove(document);
            await _dbContext.SaveChangesAsync();
        }
        return document;
    }
    
    public async Task<string?> GetFileNameAsync(string id)
    {
        return await _dbContext.Documents
            .Where(document => document.Id.Equals(Guid.Parse(id)))
            .Select(document => document.FileName)
            .FirstOrDefaultAsync();
    }
}