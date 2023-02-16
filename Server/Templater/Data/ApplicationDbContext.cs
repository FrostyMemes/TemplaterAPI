namespace Templater.Data;

public class ApplicationDbContext:DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):
        base(options)
    {
        
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Template> Templates { get; set; }
}