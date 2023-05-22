using Microsoft.AspNetCore.Mvc;
using Templater.Services.Interfaces;
using System.IO;
using Microsoft.AspNetCore.StaticFiles;

namespace Templater.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DocumentController: ControllerBase
{
    private readonly IObjectStorageService _objectStorageService;
    
    public DocumentController(IObjectStorageService objectStorageService)
    {
        _objectStorageService = objectStorageService;
    }

    [HttpGet("get-file/{id}")]
    public async Task<ActionResult> GetFileStream(string id)
    {
        var stream = await _objectStorageService.GetObjectStreamAsync(id);
        var fileName = await _objectStorageService.GetFileNameAsync(id);
        return File(stream, "APPLICATION/octet-stream", fileName);
    }
    
    [HttpGet("get-file-link/{id}")]
    public async Task<ActionResult> GetFileLink(string id)
    {
        var fileName = await _objectStorageService.GetFileNameAsync(id);
        var url = await _objectStorageService.GetObjectLinkAsync(id, fileName);
        return Ok(url);
    }
    
    [HttpPost]
    public async Task<ActionResult> Post(IFormFile document)
    {
        var stream = new MemoryStream();
        await document.CopyToAsync(stream);
        var id = await _objectStorageService.SaveObjectAsync(stream, document.FileName);
        return Ok(new {id, document.FileName});
    }
}