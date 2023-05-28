using Minio;
using Templater.Services.Interfaces;

namespace Templater.Services;

public class MinioObjectStorageService: IObjectStorageService
{
    private readonly MinioClient _client;
    private readonly ApplicationDbContext _dbContext;

    public MinioObjectStorageService(MinioClient client, ApplicationDbContext dbContext)
    {
        _client = client;
        _dbContext = dbContext;
    }

    public async Task<Stream> GetObjectStreamAsync(string id)
    {
        var fileStream = new MemoryStream();
        var fileObject = new GetObjectArgs().WithBucket("docx-templater-storage")
            .WithObject(id).WithCallbackStream(stream => stream.CopyToAsync(fileStream));
        await _client.GetObjectAsync(fileObject);
        fileStream.Seek(0, SeekOrigin.Begin);
        return fileStream;
    }
    
    public async Task<string> GetObjectLinkAsync(string id, string fileName)
    {
        var args = new PresignedGetObjectArgs()
            .WithBucket("docx-templater-storage")
            .WithObject(id)
            .WithExpiry(60 * 60 * 24)
            .WithHeaders(new Dictionary<string, string>()
            {
                {"Content-Disposition", $"filename=\"{fileName}\""}
            });
        
        string? url = await _client.PresignedGetObjectAsync(args);
        return url;
    }

    public async Task<Guid> SaveObjectAsync(MemoryStream stream, string fileName)
    {
        var newDocument = new Document() { FileName = fileName, UserId = 1};
        await _dbContext.Documents.AddAsync(newDocument);
        await _dbContext.SaveChangesAsync();

        stream.Seek(0, SeekOrigin.Begin);
        
        var document = new PutObjectArgs()
            .WithBucket("docx-templater-storage")
            .WithObject(newDocument.Id.ToString())
            .WithObjectSize(stream.Length)
            .WithStreamData(stream);
        await _client.PutObjectAsync(document);
        return newDocument.Id;
    }

    public async Task DeleteObjectAsync(string id)
    {
        var rmArgs = new RemoveObjectArgs()
            .WithBucket("docx-templater-storage")
            .WithObject(id);
        await _client.RemoveObjectAsync(rmArgs);
    }
}