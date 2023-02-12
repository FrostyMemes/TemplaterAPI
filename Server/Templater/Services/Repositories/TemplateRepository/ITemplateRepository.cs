namespace Templater.Services.Repositories.TemplateRepository;

public interface ITemplateRepository
{
    Task<IEnumerable<Template>> GetAllTemplates();
    Task<Template> GetDetails(int id);
    Task<bool> InsertTemplate(Template template);
    Task<bool> UpdateTemplate(Template template);
    Task<bool> DeleteTemplate(int id);
}