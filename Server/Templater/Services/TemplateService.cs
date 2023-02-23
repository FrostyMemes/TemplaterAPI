using Templater.DTO;
using Templater.Services.Interfaces;

namespace Templater.Services;

public class TemplateService : ITemplateService
{
    private readonly ApplicationDbContext _dbContext;

    public TemplateService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Template>> GetAllTemplatesAsync()
    {
        return await _dbContext.Templates.ToListAsync();
    }

    public async Task<Template> GetTemplateAsync(string id)
    {
        return await _dbContext.Templates.FindAsync(id);
    }

    public async Task<Template> CreateTemplateAsync(CreateTemplateDto newTemplate)
    {
        var template = new Template()
        {
            Title = newTemplate.Title,
            Markdown = newTemplate.Markdown,
            Markup = newTemplate.Markup,
            UserId = newTemplate.UserId
        };
        await _dbContext.Templates.AddAsync(template);
        await _dbContext.SaveChangesAsync();
        return template;
    }

    public async Task<Template> UpdateTemplateAsync(UpdateTemplateDto updateTemplate)
    {
        var template = await _dbContext.Templates.FindAsync(updateTemplate.Id.ToString());
        if (template != null)
        {
            template.Title = updateTemplate.Title;
            template.Markdown = updateTemplate.Markdown;
            template.Markup = updateTemplate.Markup;
            await _dbContext.SaveChangesAsync();
        }
        return template;
    }

    public async Task<Template> DeleteTemplateAsync(string id)
    {
        var template = await _dbContext.Templates.FindAsync(id);
        if (template != null)
        {
            _dbContext.Templates.Remove(template);
            await _dbContext.SaveChangesAsync();
        }
        return template;
   
    }
}
