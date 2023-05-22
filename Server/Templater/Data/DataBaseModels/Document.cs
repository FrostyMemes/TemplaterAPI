using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Templater.Data.DataBaseModels;

[Table("documents")]
public class Document
{
    public Guid Id { get; set; }
    public string FileName { get; set; }
    
    public int UserId { get; set; }

    public User User { get; set; }
}