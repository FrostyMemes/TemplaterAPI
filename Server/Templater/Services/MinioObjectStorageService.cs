using Minio;
using Templater.Services.Interfaces;

namespace Templater.Services;

public class MinioObjectStorageService: IObjectStorageService
{
    private MinioClient _client;

    public MinioObjectStorageService(MinioClient client)
    {
        _client = client;
    }

    public async Task GetObject()
    {
        /*var memStream = new MemoryStream();
        var loh = new GetObjectArgs().WithBucket("docx-templater-storage")
        .WithObject("").WithCallbackStream(stream => stream.CopyToAsync(memStream));
        await _client.GetObjectAsync(loh);*/
    }
}