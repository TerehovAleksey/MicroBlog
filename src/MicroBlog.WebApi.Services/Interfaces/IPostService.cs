namespace MicroBlog.WebApi.Services.Interfaces;

public interface IPostService
{
    public Task<IEnumerable<TagDto>> GetAllTagsAsync();
}