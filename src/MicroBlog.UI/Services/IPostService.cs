using MicroBlog.UI.Models;

namespace MicroBlog.UI.Services;

public interface IPostService
{
    public Task<PostsModel> GetPostsAsync(PostFilter? filter);
    public Task<IEnumerable<BasePostModel>> GetMostPopularPostsAsync();
    public Task<IEnumerable<string>> GetAllTagsAsync();
}