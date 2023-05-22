using System.ComponentModel.DataAnnotations.Schema;

namespace Templater.Data.DataBaseModels;

[Table("users")]
[Index(nameof(Email),IsUnique = true)]
public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string HashedPassword  { get; set; }
    
    public ICollection<Template> Templates { get; set; }
    public ICollection<Document> Documents { get; set; }
}