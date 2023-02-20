using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using Templater.Services.MarkdownTemplateService;

namespace Templater.Controllers;

[ApiController]
[Route("api")]
public class TemplaterController : ControllerBase
{
    private readonly ITemplateParser _templateParser;
    private readonly ApplicationDbContext _dbContext;

    public TemplaterController(ITemplateParser templateParser, ApplicationDbContext dbContext)
    {
        _templateParser = templateParser;
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<string> Get(string markdown)
    {
        var result = await _templateParser.ParseAsync(markdown);
        return result;
    }
    
    [HttpPost]
    public async Task<ActionResult> Post(TemplateModel templateModel)
    {
        return Ok(new
        {
            Id = '1'
        });
    }
}