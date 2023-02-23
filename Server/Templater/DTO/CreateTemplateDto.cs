namespace Templater.DTO;

public class CreateTemplateDto
{
    
    public string Title { get; set; }
    public string Markdown { get; set; }
    public string Markup { get; set; }
    public int UserId { get; set; }
}