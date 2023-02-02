namespace MicroBlog.UI.Core.Services.Files;

public interface IFileService
{
    public Task<string?> UploadImage(MultipartFormDataContent content);
}