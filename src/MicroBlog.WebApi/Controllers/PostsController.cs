namespace MicroBlog.WebApi.Controllers
{
    [ApiController]
    [Route("api/posts")]
    [Produces(MediaTypeNames.Application.Json)]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        /// <summary>
        /// Получение списка всех тегов
        /// </summary>
        /// <returns></returns>
        [HttpGet("tags")]
        [ProducesResponseType(typeof(IEnumerable<TagDto>), StatusCodes.Status200OK)]
        public Task<IEnumerable<TagDto>> GetTagsAsync() => _postService.GetAllTagsAsync();

        /// <summary>
        /// Получение списка постов по фильтру
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PostListItemDto>), StatusCodes.Status200OK)]
        public async Task<IEnumerable<PostListItemDto>> GetPostsAsync([FromQuery] PostFilter filter)
        {
            var posts = await _postService.GetPostsByFilterAsync(filter);

            var body = JsonSerializer.Serialize(posts.MetaData, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            Response.Headers.Add("X-Pagination", body);

            return FixCoverPath(posts);
        }

        /// <summary>
        /// Получение поста по сегменту пути
        /// </summary>
        /// <returns></returns>
        [HttpGet("{slug}")]
        [ProducesResponseType(typeof(PostDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPostAsync([FromRoute] string slug)
        {
            var post = await _postService.GetPostBySlugAsync(slug);
            if (post is null)
            {
                return NotFound();
            }

            post.Cover = GetCoverFullPath(post.Cover);

            return Ok(post);
        }

        /// <summary>
        /// Получение соседних постов
        /// </summary>
        /// <returns></returns>
        [HttpGet("nav/{id:guid}")]
        [ProducesResponseType(typeof(PostNavigationDto), StatusCodes.Status200OK)]
        public Task<PostNavigationDto> GetPostNavigationAsync([FromRoute] Guid id) => _postService.GetPostNavigationAsync(id);

        /// <summary>
        /// Получение рекомендаций на основе поста
        /// </summary>
        /// <returns></returns>
        [HttpGet("like/{id:guid}")]
        [ProducesResponseType(typeof(IEnumerable<PostListItemDto>), StatusCodes.Status200OK)]
        public async Task<IEnumerable<PostListItemDto>> GetRecommendationAsync([FromRoute] Guid id)
        {
            var posts = await _postService.GetRecommendationsAsync(id);
            return FixCoverPath(posts);
        }

        /// <summary>
        /// Получение самых популярных постов
        /// </summary>
        /// <returns></returns>
        [HttpGet("popular")]
        [ProducesResponseType(typeof(IEnumerable<PostListItemDto>), StatusCodes.Status200OK)]
        public async Task<IEnumerable<PostListItemDto>> GetPopularPostsAsync()
        {
            var posts = await _postService.GetPopularPostsAsync();
            return FixCoverPath(posts);
        }

        private string GetCoverFullPath(string path)
        {
            var baseUri = $"{Request.Scheme}://{Request.Host.Host}:{Request.Host.Port ?? 80}";
            var folderName = Path.Combine(baseUri, "img");

            if (string.IsNullOrEmpty(path))
            {
                return Path.Combine(baseUri, "post.jpg");
            }

            return Path.Combine(folderName, path);
        }

        private IEnumerable<PostListItemDto> FixCoverPath(IEnumerable<PostListItemDto> posts)
        {
            var postsList = posts.ToList();

            foreach (var post in postsList)
            {
                post.Cover = GetCoverFullPath(post.Cover);
            }

            return postsList;
        }
    }
}