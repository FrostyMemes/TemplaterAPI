namespace Templater.Data.DataBaseModels;

public class Template
{
    public Guid Id { get; set; }
    
    public string Title { get; set; }
    
    public string Markdown { get; set; }
    
    public string Markup { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }
}