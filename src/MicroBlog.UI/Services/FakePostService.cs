using MicroBlog.UI.Models;

namespace MicroBlog.UI.Services;

public class FakePostService : IPostService
{
    public async Task<PostsModel> GetPostsAsync(PostFilter? filter)
    {
        var posts = new List<PostCardModel>
        {
            new()
            {
                Title = "Is the Designer Facing Extinction?",
                Author = "Terehoff",
                Tag = "Coding",
                Date = DateOnly.FromDateTime(DateTime.Today),
                Text =
                    "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen bo…",
                ImageUri = "img/post-1.jpg"
            },
            new()
            {
                Title = "Guide to WordPress Post Revisions",
                Author = "Terehoff",
                Tag = "Coding",
                Date = DateOnly.FromDateTime(DateTime.Today.AddDays(-1)),
                Text =
                    "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen bo…",
                ImageUri = "img/post-2.jpg"
            },
            new()
            {
                Title = "How To Choose The Right Hosting For Your Blog",
                Author = "Terehoff",
                Tag = "Hosting",
                Date = DateOnly.FromDateTime(DateTime.Today.AddDays(-2)),
                Text =
                    "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen bo…",
                ImageUri = "img/post-3.jpg"
            },
            new()
            {
                Title = "Teach Your Kids to Code Playground with Tynker",
                Author = "Terehoff",
                Tag = "Design",
                Date = DateOnly.FromDateTime(DateTime.Today.AddDays(-3)),
                Text =
                    "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen bo…",
                ImageUri = "img/post-4.jpg"
            },
            new()
            {
                Title = "Getting Started with JavaScript Promises",
                Author = "Terehoff",
                Tag = "Coding",
                Date = DateOnly.FromDateTime(DateTime.Today.AddDays(-4)),
                Text =
                    "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen bo…",
                ImageUri = "img/post-5.jpg"
            },
            new()
            {
                Title = "Surface Studio vs iMac – Which Should You Pick?",
                Author = "Terehoff",
                Tag = "Design",
                Date = DateOnly.FromDateTime(DateTime.Today.AddDays(-5)),
                Text =
                    "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen bo…",
                ImageUri = "img/post-6.jpg"
            },
            new()
            {
                Title = "Create Device Mockups in Browser with DeviceMock",
                Author = "Terehoff",
                Tag = "Coding",
                Date = DateOnly.FromDateTime(DateTime.Today.AddDays(-6)),
                Text =
                    "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen bo…",
                ImageUri = "img/post-7.jpg"
            }
        };
        
        await Task.Delay(200);

        if (filter is not null)
        {
            if (!string.IsNullOrEmpty(filter.Tag))
            {
                posts = posts.Where(x => x.Tag.ToLower().Equals(filter.Tag.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(filter.Author))
            {
                posts = posts.Where(x => x.Author.ToLower().Equals(filter.Author.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(filter.Date))
            {
                posts = posts.Where(x => x.Date == DateOnly.Parse(filter.Date)).ToList();
            }

            if (!string.IsNullOrEmpty(filter.Query))
            {
                posts = posts.Where(x => x.Text.ToLower().Contains(filter.Query.ToLower())).ToList();
            }
        }
        
        var result = new PostsModel
        {
            Page = 1,
            TotalPages = 1,
            Posts = posts
        };

        return result;
    }

    public async Task<IEnumerable<BasePostModel>> GetMostPopularPostsAsync()
    {
        var posts = new List<BasePostModel>
        {
            new()
            {
                Title = "Is the Designer Facing Extinction?",
                Date = DateOnly.FromDateTime(DateTime.Today),
                ImageUri = "img/post-1.jpg"
            },
            new()
            {
                Title = "Guide to WordPress Post Revisions",
                Date = DateOnly.FromDateTime(DateTime.Today.AddDays(-1)),
                ImageUri = "img/post-2.jpg"
            },
            new()
            {
                Title = "How To Choose The Right Hosting For Your Blog",
                Date = DateOnly.FromDateTime(DateTime.Today.AddDays(-2)),
                ImageUri = "img/post-3.jpg"
            }
        };

        await Task.Delay(500);

        return posts;
    }

    public async Task<IEnumerable<string>> GetAllTagsAsync()
    {
        string[] tags =
        {
            "Business", "Coding", "Design", "Food", "Hosting", "Learn", "People", "Photography", "Projects", "Technology"
        };

        await Task.Delay(300);

        return tags;
    }
}