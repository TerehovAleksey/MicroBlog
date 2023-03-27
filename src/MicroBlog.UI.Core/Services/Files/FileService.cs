namespace MicroBlog.UI.Core.Services.Files;

public class FileService : IFileService
{
    private readonly HttpClient _httpClient;

    public FileService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string?> UploadImage(MultipartFormDataContent content)
    {
        var response = await _httpClient.PostAsync("files/image", content);
        if (response?.IsSuccessStatusCode ?? false)
        {
            var newUploadResults = await response.Content.ReadAsStringAsync();
            return newUploadResults;
        }

        return null;
    }
}