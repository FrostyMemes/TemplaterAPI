namespace Templater.Models;


[Index(nameof(Email),IsUnique = true)]
public class User
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Nickname field is required.")]
    public string Username { get; set; }
    
    [Required(ErrorMessage = "Email field is required.")]
    [EmailAddress]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password field is required.")]
    public string Password { get; set; }

    public ICollection<Template> Templates { get; set; }
}