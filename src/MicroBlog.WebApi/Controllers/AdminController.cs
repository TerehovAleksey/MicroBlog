namespace MicroBlog.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/admin")]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IPostService _postService;

        public AdminController(UserManager<User> userManager, IPostService postService)
        {
            _userManager = userManager;
            _postService = postService;
        }

        [HttpGet("post/{status}")]
        public async Task<IEnumerable<PostEditListItemDto>> GetAllPostsAsync([FromRoute] PublishedStatus status)
        {
            var posts = await _postService.GetPostsByStatusAsync(status);
            return posts;
        }

        [HttpPost("post")]
        public async Task<IActionResult> CreatePostAsync([FromBody] PostEditDto post)
        {
            var user = await GetUserByClaims();

            if (user == null)
            {
                return Unauthorized();
            }

            await _postService.CreatePostAsync(user.Id, post);
            return NoContent();
        }

        [HttpPut("post/{id:guid}")]
        public async Task<IActionResult> UpdatePostAsync([FromRoute] Guid id, [FromBody] PostEditDto post)
        {
            var result = await _postService.UpdatePostAsync(id, post);
            return result ? NoContent() : NotFound();
        }

        [HttpPut("post/publish/{postId}")]
        public async Task<IActionResult> PublishPostAsync([FromRoute] Guid postId, [FromBody] bool value)
        {
            var result = await _postService.PublishPostAsync(postId, value);
            return result ? NoContent() : NotFound();
        }

        [HttpPut("post/feature/{postId}")]
        public async Task<IActionResult> FeaturePostAsync([FromRoute] Guid postId, [FromBody] bool value)
        {
            var result = await _postService.FeaturePostAsync(postId, value);
            return result ? NoContent() : NotFound();
        }

        [HttpDelete("post/{id}")]
        public async Task<IActionResult> DeletePostAsync([FromRoute] Guid id)
        {
            var result = await _postService.DeletePostAsync(id);
            return result ? NoContent() : NotFound();
        }

        [HttpGet("post/{id:guid}")]
        public async Task<IActionResult> GetPostByIdAsync([FromRoute] Guid id)
        {
            var post = await _postService.GetPostByIdAsync(id);
            if (post is null)
            {
                return NotFound();
            }

            post.Cover = GetCoverFullPath(post.Cover);

            return Ok(post);
        }

        private Task<User?> GetUserByClaims()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? string.Empty;
            return _userManager.FindByEmailAsync(email);
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
    }
}
