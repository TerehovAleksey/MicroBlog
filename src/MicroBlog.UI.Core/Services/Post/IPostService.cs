namespace MicroBlog.UI.Core.Services.Post;

public interface IPostService
{
    /// <summary>
    /// Получение всех тегов
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<TagDto>?> GetAllTagsAsync();
    
    /// <summary>
    /// Получение списка постов по фильтру
    /// </summary>
    /// <returns></returns>
    public Task<PagedList<PostListItemDto>?> GetPostsAsync(PostFilter filter);

    /// <summary>
    /// Получение поста по url
    /// </summary>
    /// <returns></returns>
    public Task<PostDto?> GetPostAsync(string slug);

    /// <summary>
    /// Получение соседних постов
    /// </summary>
    /// <returns></returns>
    public Task<PostNavigationDto?> GetPostNavigation(Guid postId);

    /// <summary>
    /// Получение рекомендаций на основе поста
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<PostListItemDto>?> GetRecommendationsAsync(Guid id);

    /// <summary>
    /// Получение наиболее популярных постов
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<PostListItemDto>?> GetMostPopularPostsAsync();

    #region AdminRegion

    public Task<IEnumerable<PostEditListItemDto>?> GetAllPostsAsync(PublishedStatus status);
    public Task<PostDto?> GetPostByIdAsync(Guid id);
    public Task<bool> CreatePostAsync(PostEditDto post);
    public Task<bool> UpdatePostAsync(Guid postId, PostEditDto post);
    public Task PublishAsync(Guid postId);
    public Task UnPublishAsync(Guid postId);
    public Task FeatureAsync(Guid postId, bool state);
    public Task DeleteAsync(Guid postId);

    #endregion
}