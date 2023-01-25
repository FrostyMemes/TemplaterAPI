using Microsoft.AspNetCore.Mvc;
using Templater.Builder;

namespace Templater.Controllers;

[ApiController]
[Route("[controller]")]

public class TemplaterController : ControllerBase
{
    private readonly ILogger<TemplaterController> _logger;
    private readonly TemplateParser _templateParser;

    public TemplaterController(ILogger<TemplaterController> logger)
    {
        _logger = logger;
        _templateParser = new TemplateParser();
    }
    
    [HttpGet(Name = "TemplateMarkdown")]
    public IActionResult Get(string markdown)
    {
        try
        {
            var template = _templateParser.Parse(markdown);
            return Ok(template);
        }
        catch (Exception e)
        {
            return StatusCode(500);
        }
        
        
    }
}