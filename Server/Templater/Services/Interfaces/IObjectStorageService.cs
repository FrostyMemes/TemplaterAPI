namespace Templater.Services.Interfaces;

public interface IObjectStorageService
{
    Task<Stream> GetObjectStreamAsync(string id);
    Task<string> GetObjectLinkAsync(string id, string fileName);
    Task<Guid> SaveObjectAsync(MemoryStream stream, string fileName);
    Task DeleteObjectAsync(string id);
}