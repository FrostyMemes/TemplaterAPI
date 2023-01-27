using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using Templater.Services.MarkdownTemplateService;

namespace Templater.Controllers;

[ApiController]
[Route("[controller]")]

public class TemplaterController : ControllerBase
{
    private readonly ITemplateParser _templateParser;

    public TemplaterController(ITemplateParser templateParser)
    {
        _templateParser = templateParser;
    }
    
    [HttpGet(Name = "TemplateMarkdown")]
    public async Task<string> Get(string markdown)
    {
        var result = await _templateParser.Parse(markdown);
        return result;
    }
}