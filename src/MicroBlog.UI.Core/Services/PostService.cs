using System.Net.Http.Json;
using MicroBlog.Common.Dtos;
using Microsoft.AspNetCore.WebUtilities;
using WebApi.Paging;

namespace MicroBlog.UI.Core.Services;

public class PostService : IPostService
{
    private readonly HttpClient _httpClient;

    public PostService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PagedList<PostLinkDto>?> GetPostsAsync(PostFilter filter)
    {
        var query = GetQueryString("posts", filter);
        var posts = await _httpClient.GetFromJsonAsync<IEnumerable<PostLinkDto>>(query);
        if (posts is not null)
        {
            return new PagedList<PostLinkDto>(posts, new MetaData(posts.Count(), posts.Count(), 1));
        }
        return null;
    }

    public async Task<IEnumerable<PostLinkDto>?> GetMostPopularPostsAsync()
    {
        var posts = await _httpClient.GetFromJsonAsync<IEnumerable<PostLinkDto>>("posts");
        return posts?.Take(3);
    }

    public Task<IEnumerable<TagDto>?> GetAllTagsAsync() => _httpClient.GetFromJsonAsync<IEnumerable<TagDto>>("posts/tags");

    public Task<PostDto?> GetPostAsync(string slug) => _httpClient.GetFromJsonAsync<PostDto>($"posts/{slug}");

    private static string GetQueryString(string url, PostFilter filter)
    {
        var dic = new Dictionary<string, string>();
        if (filter.Page > 1)
        {
            dic.Add(nameof(filter.Page), filter.Page.ToString());
        }
        if (!string.IsNullOrEmpty(filter.Author))
        {
            dic.Add(nameof(filter.Author), filter.Author);
        }
        if (!string.IsNullOrEmpty(filter.Tag))
        {
            dic.Add(nameof(filter.Tag), filter.Tag);
        }
        if (!string.IsNullOrEmpty(filter.Query))
        {
            dic.Add(nameof(filter.Query), filter.Query);
        }
        if (filter.Date != null)
        {
            dic.Add(nameof(filter.Date), ((DateTime)filter.Date).ToShortDateString());
        }

        return QueryHelpers.AddQueryString(url, dic);
    }
}