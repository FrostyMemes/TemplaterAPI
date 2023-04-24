using Minio;
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

    public async Task<IEnumerable<AllTemplateDto>> GetAllTemplatesAsync()
    {
        return await _dbContext.Templates
            .Select(template => new AllTemplateDto()
            {
                Id = template.Id.ToString(),
                Title = template.Title,
            })
            .ToListAsync();
    }

    public async Task<GetTemplateDto> GetTemplateAsync(Guid id)
    {
        #pragma warning disable CS8603
        return await _dbContext.Templates
            .Where(template => template.Id == id)
            .Select(template => new GetTemplateDto()
            {
                Id = template.Id.ToString(),
                Title = template.Title,
                Markdown = template.Markdown,
                Markup = template.Markup
            })
            .FirstOrDefaultAsync();
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
        // ReSharper disable once HeapView.BoxingAllocation
        var template = await _dbContext.Templates.FindAsync(updateTemplate.Id);
        if (template != null)
        {
            template.Title = updateTemplate.Title;
            template.Markdown = updateTemplate.Markdown;
            template.Markup = updateTemplate.Markup;
            await _dbContext.SaveChangesAsync();
        }
        return template;
    }

    public async Task<Template> DeleteTemplateAsync(Guid id)
    {
        // ReSharper disable once HeapView.BoxingAllocation
        var template = await _dbContext.Templates.FindAsync(id);
        if (template != null)
        {
            _dbContext.Templates.Remove(template);
            await _dbContext.SaveChangesAsync();
        }
        return template;
    }
}
