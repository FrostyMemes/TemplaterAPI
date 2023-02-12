namespace Templater.Services.Repositories.TemplateRepository;

public class TemplateRepository: ITemplateRepository
{
    public Task<IEnumerable<Template>> GetAllTemplates()
    {
        throw new NotImplementedException();
    }

    public Task<Template> GetDetails(int id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> InsertTemplate(Template template)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateTemplate(Template template)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteTemplate(int id)
    {
        throw new NotImplementedException();
    }
}