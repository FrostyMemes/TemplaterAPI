namespace Templater.Services.Repositories.TemplateRepository;

public interface ITemplateRepository
{
    Task<IEnumerable<Template>> GetAllTemplatesAsync();
    Task<Template> GetTemplateAsync(int id);
    Task<bool> InsertTemplateAsync(TemplateModel template);
    Task<bool> UpdateTemplateAsync(TemplateModel template);
    Task<bool> DeleteTemplateAsync(int id);
}