namespace Templater.Models;

public class Template
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Title field is required.")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Markdown field is required.")]
    public string Markdown { get; set; }
    
    [Required(ErrorMessage = "Markup field is required.")]
    public string Markup { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }
}