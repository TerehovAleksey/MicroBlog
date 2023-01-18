using Microsoft.AspNetCore.Components.Web;
using System.IO;
using System.Text;

namespace MicroBlog.WebApi.Controllers
{
    [ApiController]
    [Route("api/posts")]
    [Produces(MediaTypeNames.Application.Json)]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly List<PostLinkDto> _posts;

        public PostsController(IPostService postService)
        {
            _postService = postService;
            _posts = GeneratePosts();
        }

        [HttpGet("tags")]
        [ProducesResponseType(typeof(IEnumerable<TagDto>), StatusCodes.Status200OK)]
        public Task<IEnumerable<TagDto>> GetTagsAsync()
        {
            return _postService.GetAllTagsAsync();
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PostDto>), StatusCodes.Status200OK)]
        public async Task<IEnumerable<PostLinkDto>> GetPostsAsync([FromQuery] PostFilter filter)
        {
            var baseUri = $"{Request.Scheme}://{Request.Host.Host}:{Request.Host.Port ?? 80}";
            var folderName = Path.Combine(baseUri, "img");
            foreach (var post in _posts)
            {
                post.Cover = Path.Combine(folderName, post.Cover);
            }

            IEnumerable<PostLinkDto>? result;
            if (!string.IsNullOrEmpty(filter.Tag))
            {
                result = _posts.Where(p => p.MainTag == filter.Tag);
            }
            else if (!string.IsNullOrEmpty(filter.Author))
            {
                result = _posts.Where(p => p.Author == filter.Author);
            }
            else
            {
                result = _posts;
            }

            return result;
        }

        [HttpGet("{slug}")]
        public async Task<IActionResult> GetPostAsync([FromRoute] string slug, [FromServices] IHostEnvironment env)
        {
            var post = _posts.Find(p => p.Slug == slug);
            if (post != null)
            {
                var baseUri = $"{Request.Scheme}://{Request.Host.Host}:{Request.Host.Port ?? 80}";
                var folderName = Path.Combine(baseUri, "img");

                string content = string.Empty;
                var testContentFile = Path.Combine(env.ContentRootPath, "wwwroot", "files", "test.md");
                using (StreamReader streamReader = new(testContentFile, Encoding.UTF8))
                {
                    content = streamReader.ReadToEnd();
                }

                return Ok(new PostDto
                {
                    Author = post.Author,
                    Content = content,
                    Cover = Path.Combine(folderName, post.Cover),
                    DatePublished = post.DatePublished,
                    Slug = slug,
                    Tags = new List<TagDto>()
                    {
                        new TagDto(Guid.NewGuid(), post.MainTag)
                    },
                    Title = post.Title
                });
            }
            return NotFound();
        }

        private static List<PostLinkDto> GeneratePosts()
        {
            return new List<PostLinkDto>
            {
                new()
                {
                    Title = "Is the Designer Facing Extinction?",
                    Author = "Terehoff",
                    DatePublished = DateTime.Today,
                    Description =
                        "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen bo…",
                    Cover = "post-1.jpg",
                    MainTag = "Coding",
                    Slug = "is-the-designer-facing-extinction"
                },
                new()
                {
                    Title = "Guide to WordPress Post Revisions",
                    Author = "Terehoff",
                    DatePublished = DateTime.Today.AddDays(-1),
                    Description =
                        "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen bo…",
                    Cover = "post-2.jpg",
                    MainTag = "Coding",
                    Slug = "guide-to-wordPress-post-revisions"
                },
                new()
                {
                    Title = "How To Choose The Right Hosting For Your Blog",
                    Author = "Terehoff",
                    DatePublished = DateTime.Today.AddDays(-2),
                    Description =
                        "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen bo…",
                    Cover = "post-3.jpg",
                    MainTag = "Hosting",
                    Slug = "how-to-choose-the-right-hosting-for-your-blog"
                },
                new()
                {
                    Title = "Teach Your Kids to Code Playground with Tynker",
                    Author = "Terehoff",
                    DatePublished = DateTime.Today.AddDays(-3),
                    Description =
                        "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen bo…",
                    Cover = "post-4.jpg",
                    MainTag = "Design",
                    Slug = "teach-your-kids-to-code-playground-with-tynker"
                },
                new()
                {
                    Title = "Getting Started with JavaScript Promises",
                    Author = "Terehoff",
                    DatePublished = DateTime.Today.AddDays(-4),
                    Description =
                        "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen bo…",
                    Cover = "post-5.jpg",
                    MainTag = "Coding",
                    Slug = "getting-started-with-javascript-promises"
                },
                new()
                {
                    Title = "Surface Studio vs iMac – Which Should You Pick?",
                    Author = "Terehoff",
                    DatePublished = DateTime.Today.AddDays(-5),
                    Description =
                        "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen bo…",
                    Cover = "post-6.jpg",
                    MainTag = "Design",
                    Slug = "surface-studio-vs-imac"
                },
                new()
                {
                    Title = "Create Device Mockups in Browser with DeviceMock",
                    Author = "Terehoff",
                    DatePublished = DateTime.Today.AddDays(-6),
                    Description =
                        "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen bo…",
                    Cover = "post-7.jpg",
                    MainTag = "Coding",
                    Slug = "create-device-mockups-in-browser-with-deviceMock"
                }
            };
        }
    }
}