namespace Templater.Services.Repositories.TemplateRepository;

public class TemplateRepository: ITemplateRepository
{
    public Task<IEnumerable<Template>> GetAllTemplatesAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Template> GetTemplateAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> InsertTemplateAsync(TemplateModel template)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateTemplateAsync(TemplateModel template)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteTemplateAsync(int id)
    {
        throw new NotImplementedException();
    }
}