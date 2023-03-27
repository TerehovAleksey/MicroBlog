namespace MicroBlog.WebApi.Services.Interfaces;

public interface IPostService
{
    /// <summary>
    /// Получение всех тегов
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<TagDto>> GetAllTagsAsync();
    
    /// <summary>
    /// Получение списка постов для главной страницы
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public Task<PagedList<PostListItemDto>> GetPostsByFilterAsync(PostFilter filter);

    /// <summary>
    /// Получение поста по сегмету пути
    /// </summary>
    /// <param name="slug"></param>
    /// <returns></returns>
    public Task<PostDto?> GetPostBySlugAsync(string slug);

    /// <summary>
    /// Получение соседних постов
    /// </summary>
    /// <param name="postId"></param>
    /// <returns></returns>
    public Task<PostNavigationDto> GetPostNavigationAsync(Guid postId);

    /// <summary>
    /// Получение рекомендаций для поста
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<IEnumerable<PostListItemDto>> GetRecommendationsAsync(Guid id);
    
    /// <summary>
    /// Получение самых популярных постов
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<PostListItemDto>> GetPopularPostsAsync();

    #region AdminRegion

    public Task<IEnumerable<PostEditListItemDto>> GetPostsByStatusAsync(PublishedStatus status);
    public Task<PostDto?> GetPostByIdAsync(Guid id);
    public Task<Guid> CreatePostAsync(Guid userId, PostEditDto post);
    public Task<bool> UpdatePostAsync(Guid id, PostEditDto post);
    public Task<bool> PublishPostAsync(Guid postId, bool isPublished);
    public Task<bool> FeaturePostAsync(Guid postId, bool isFeatured);
    public Task<bool> DeletePostAsync(Guid postId);
    
    #endregion

}