using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using Templater.Services.MarkdownTemplateService;

namespace Templater.Controllers;

[ApiController]
[Route("api/templater")]
public class TemplaterController : ControllerBase
{
    private readonly ITemplateParser _templateParser;

    public TemplaterController(ITemplateParser templateParser)
    {
        _templateParser = templateParser;
    }

    [HttpGet]
    public async Task<string> Get(string markdown)
    {
        var result = await _templateParser.ParseAsync(markdown);
        return result;
    }
}