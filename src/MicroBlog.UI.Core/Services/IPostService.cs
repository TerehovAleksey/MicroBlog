using MicroBlog.Common.Dtos;
using WebApi.Paging;

namespace MicroBlog.UI.Core.Services;

public interface IPostService
{
    public Task<PagedList<PostLinkDto>?> GetPostsAsync(PostFilter filter);
    public Task<IEnumerable<PostLinkDto>?> GetMostPopularPostsAsync();
    public Task<IEnumerable<TagDto>?> GetAllTagsAsync();
    public Task<PostDto?> GetPostAsync(string slug);
}