using Templater.DTO;

namespace Templater.Services.Interfaces;

public interface ITemplateService
{
    Task<IEnumerable<AllTemplateDto>> GetAllTemplatesAsync();
    Task<GetTemplateDto> GetTemplateAsync(Guid id);
    Task<Template> CreateTemplateAsync(CreateTemplateDto request);
    Task<Template> UpdateTemplateAsync(UpdateTemplateDto request);
    Task<Template> DeleteTemplateAsync(Guid id);
}