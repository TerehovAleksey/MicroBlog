namespace MicroBlog.UI.Core.Services.Post;

public class PostService : IPostService
{
    private readonly HttpClient _httpClient;
    private static readonly JsonSerializerOptions Options = new() { PropertyNameCaseInsensitive = true };

    public PostService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<IEnumerable<TagDto>?> GetAllTagsAsync() => _httpClient.GetFromJsonAsync<IEnumerable<TagDto>>("posts/tags");
    
    public async Task<PagedList<PostListItemDto>?> GetPostsAsync(PostFilter filter)
    {
        var query = GetQueryString("posts", filter);

        var httpResponse = await _httpClient.GetAsync(query);

        httpResponse.EnsureSuccessStatusCode();

        if (httpResponse?.IsSuccessStatusCode ?? false)
        {
            var content = await httpResponse.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<List<PostListItemDto>>(content, Options);
            var metadata = JsonSerializer.Deserialize<MetaData>(httpResponse.Headers.GetValues("x-pagination").First(), Options);
            
            if (data != null && metadata != null)
            {
                return new PagedList<PostListItemDto>(data, metadata);
            }

            return new PagedList<PostListItemDto>();
        }
        
        return null;
    }

    public Task<PostDto?> GetPostAsync(string slug) => _httpClient.GetFromJsonAsync<PostDto>($"posts/{slug}");

    public Task<PostNavigationDto?> GetPostNavigation(Guid postId) =>
        _httpClient.GetFromJsonAsync<PostNavigationDto>($"posts/nav/{postId}");

    public Task<IEnumerable<PostListItemDto>?> GetRecommendationsAsync(Guid id) =>
        _httpClient.GetFromJsonAsync<IEnumerable<PostListItemDto>>($"posts/like/{id}");

    public Task<IEnumerable<PostListItemDto>?> GetMostPopularPostsAsync() =>
        _httpClient.GetFromJsonAsync<IEnumerable<PostListItemDto>>("posts/popular");


    #region AdminRegion

    public Task<IEnumerable<PostEditListItemDto>?> GetAllPostsAsync(PublishedStatus status) =>
        _httpClient.GetFromJsonAsync<IEnumerable<PostEditListItemDto>>($"admin/post/{status}");

    public Task<PostDto?> GetPostByIdAsync(Guid id) =>
        _httpClient.GetFromJsonAsync<PostDto>($"admin/post/{id}");

    public async Task<bool> CreatePostAsync(PostEditDto post)
    {
        var httpResponse = await _httpClient.PostAsJsonAsync("admin/post", post, Options);
        if (httpResponse?.IsSuccessStatusCode ?? false)
        {
            return true;
        }

        return false;
    }

    public async Task<bool> UpdatePostAsync(Guid postId, PostEditDto post)
    {
        var httpResponse = await _httpClient.PutAsJsonAsync($"admin/post/{postId}", post, Options);
        if (httpResponse?.IsSuccessStatusCode ?? false)
        {
            return true;
        }

        return false;
    }

    public Task PublishAsync(Guid postId) => _httpClient.PutAsJsonAsync($"admin/post/publish/{postId}", true);

    public Task UnPublishAsync(Guid postId) => _httpClient.PutAsJsonAsync($"admin/post/publish/{postId}", false);

    public Task FeatureAsync(Guid postId, bool state) => _httpClient.PutAsJsonAsync($"admin/post/feature/{postId}", state);

    public Task DeleteAsync(Guid postId) => _httpClient.DeleteAsync($"admin/post/{postId}");

    #endregion

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
            var dateString = ((DateTime)filter.Date).ToString("yyyy-MM-dd");
            dic.Add(nameof(filter.Date), dateString);
        }

        return QueryHelpers.AddQueryString(url, dic);
    }
}