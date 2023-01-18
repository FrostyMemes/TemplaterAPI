using Microsoft.AspNetCore.Mvc;

namespace Templater.Controllers;

[ApiController]
[Route("[controller]")]

public class TemplaterController : ControllerBase
{
    private readonly ILogger<TemplaterController> _logger;

    public TemplaterController(ILogger<TemplaterController> logger)
    {
        _logger = logger;
    }
    
    [HttpGet(Name = "ParseMarkdown")]
    public void Get()
    {
        
    }
}