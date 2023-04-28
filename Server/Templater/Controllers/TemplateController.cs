using Microsoft.AspNetCore.Mvc;
using Templater.DTO;
using Templater.Services.Interfaces;

namespace Templater.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TemplateController : ControllerBase
{
    private readonly ITemplateService _templateService;

    public TemplateController(ITemplateService templateService)
    {
        _templateService = templateService;
    }
    
    [HttpGet("[action]")]
    public async Task<ActionResult<IEnumerable<AllTemplateDto>>> GetAll()
    {
        var templates = await _templateService.GetAllTemplatesAsync();
        if (templates == null)
            return NotFound("Templates not found");
        return Ok(templates);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<GetTemplateDto>> Get(string id)
    {
        var template = await _templateService.GetTemplateAsync(Guid.Parse(id));
        if (template == null)
            return NotFound("Template not found");
        return Ok(template);
    }

    [HttpPost]
    public async Task<ActionResult> Post(TemplateModel templateModel)
    {
        var newTemplate = new CreateTemplateDto()
        {
            Title = templateModel.Title,
            Markdown = templateModel.Markdown,
            Markup = templateModel.Markup,
            UserId = 1
        };
        var template = await _templateService.CreateTemplateAsync(newTemplate);
        return Ok(new {template.Id, template.Title});
    }

    [HttpPut("{id}")]

    public async Task<ActionResult> Put(TemplateModel templateModel)
    {
        var updateTemplate = new UpdateTemplateDto()
        {
            Id = Guid.Parse(templateModel.Id),
            Title = templateModel.Title,
            Markdown = templateModel.Markdown,
            Markup = templateModel.Markup
        };
        
        var template = await _templateService.UpdateTemplateAsync(updateTemplate);
        if (template == null)
            return NotFound("Template isn't exist");

        return Ok(new {template.Id, template.Title, template.Markdown, template.Markup});
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        var template = await _templateService.DeleteTemplateAsync(Guid.Parse(id));
        if (template == null)
            return NotFound("Template isn't exist");

        return Ok(new {template.Id, template.Title, template.Markdown, template.Markup});
    }
}