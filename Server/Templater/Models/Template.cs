namespace Templater.Models;

public class Template
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Markup field is required.")]
    public string Markup { get; set; }
}