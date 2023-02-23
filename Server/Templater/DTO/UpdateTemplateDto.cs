namespace Templater.DTO;

public class UpdateTemplateDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Markdown { get; set; }
    public string Markup { get; set; }
}