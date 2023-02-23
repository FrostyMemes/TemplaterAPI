using Templater.DTO;

namespace Templater.Services.Interfaces;

public interface ITemplateService
{
    Task<IEnumerable<Template>> GetAllTemplatesAsync();
    Task<Template> GetTemplateAsync(string id);
    Task<Template> CreateTemplateAsync(CreateTemplateDto request);
    Task<Template> UpdateTemplateAsync(UpdateTemplateDto request);
    Task<Template> DeleteTemplateAsync(string id);
}