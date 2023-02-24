using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using Templater.Services.MarkdownTemplateService;

namespace Markdown.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MarkdownController : ControllerBase
{
    private readonly IMarkdownParser _markdownParser;

    public MarkdownController(IMarkdownParser markdownParser)
    {
        _markdownParser = markdownParser;
    }

    [HttpGet]
    public async Task<string> Get(string markdown)
    {
        var result = await _markdownParser.ParseAsync(markdown);
        return result;
    }
}