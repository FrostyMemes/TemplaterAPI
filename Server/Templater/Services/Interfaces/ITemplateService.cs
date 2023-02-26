using Templater.DTO;

namespace Templater.Services.Interfaces;

public interface ITemplateService
{
    Task<IEnumerable<ResultTemplateDto>> GetAllTemplatesAsync();
    Task<ResultTemplateDto> GetTemplateAsync(Guid id);
    Task<Template> CreateTemplateAsync(CreateTemplateDto request);
    Task<Template> UpdateTemplateAsync(UpdateTemplateDto request);
    Task<Template> DeleteTemplateAsync(Guid id);
}