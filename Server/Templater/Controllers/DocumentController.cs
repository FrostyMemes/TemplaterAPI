using Microsoft.AspNetCore.Mvc;
using Templater.DTO.DocumentDTO;
using Templater.Services.Interfaces;

namespace Templater.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DocumentController: ControllerBase
{
    private readonly IObjectStorageService _objectStorageService;
    private readonly IDocumentService _documentService;
    
    public DocumentController(IObjectStorageService objectStorageService, IDocumentService documentService)
    {
        _objectStorageService = objectStorageService;
        _documentService = documentService;
    }

    [HttpGet("get-document/{id}")]
    public async Task<ActionResult> GetFileStream(string id)
    {
        var stream = await _objectStorageService.GetObjectStreamAsync(id);
        var fileName = await _documentService.GetFileNameAsync(id);
        return File(stream, "APPLICATION/octet-stream", fileName);
    }
    
    [HttpGet("get-document-link/{id}")]
    public async Task<ActionResult> GetFileLink(string id)
    {
        var fileName = await _documentService.GetFileNameAsync(id);
        var url = await _objectStorageService.GetObjectLinkAsync(id, fileName);
        return Ok(url);
    }
    
    
    [HttpGet("get-all")]
    public async Task<ActionResult<IEnumerable<AllDocumentsDto>>> GetAll()
    {
        var documents = await _documentService.GetAllDocumentsAsync();
        if (documents == null)
            return NotFound("Documents not found");
        return Ok(documents);
    }
    
    [HttpPost]
    public async Task<ActionResult> Post(IFormFile document)
    {
        var stream = new MemoryStream();
        await document.CopyToAsync(stream);
        var id = await _objectStorageService.SaveObjectAsync(stream, document.FileName);
        return Ok(new {id, document.FileName});
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        var document = await _documentService.DeleteDocumentAsync(Guid.Parse(id));
        if (document == null)
            return NotFound("Document isn't exist");
        
        return Ok(new {document.Id, document.FileName});
    }
}