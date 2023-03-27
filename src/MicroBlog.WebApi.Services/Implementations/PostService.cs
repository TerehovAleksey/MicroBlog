namespace MicroBlog.WebApi.Services.Implementations;

public class PostService : IPostService
{
    private readonly AppDbContext _context;

    public PostService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TagDto>> GetAllTagsAsync()
    {
        var tags = await _context.Tags
            .AsNoTracking()
            .Select(t => new { t.Id, t.Name })
            .OrderBy(t => t.Name)
            .ToListAsync();

        return tags.Select(t => new TagDto(t.Id, t.Name));
    }

    public async Task<PagedList<PostListItemDto>> GetPostsByFilterAsync(PostFilter filter)
    {
        var posts = _context.Posts
            .AsNoTracking()
            .Where(p => p.DatePublished != null)
            .OrderByDescending(p => p.DatePublished)
            .Include(p => p.User)
            .Include(p => p.Tags)
            .Select(p => p);

        if (!string.IsNullOrEmpty(filter.Query))
        {
            posts = posts.Where(p => p.Title.ToLower().Contains(filter.Query.ToLower()));
        }

        if (!string.IsNullOrEmpty(filter.Author))
        {
            posts = posts.Where(p => 
            p.User.UserName.ToLower().Contains(filter.Author.ToLower()));
        }

        if (filter.Date != null)
        {
            posts = posts.Where(p => p.DatePublished.Value.DayOfYear == filter.Date.Value.DayOfYear);
        }

        if (!string.IsNullOrEmpty(filter.Tag))
        {
            posts = posts.Where(p => p.Tags.Select(t => t.Name).Contains(filter.Tag));
        }

        var count = await posts.CountAsync();
        var selected = await posts
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(p => new { p.Id, p.Title, p.User.UserName, p.Slug, p.Cover, p.Description, p.DatePublished, p.Tags.First().Name })
            .ToListAsync();

        var dtos = selected.Select(p => new PostListItemDto
        {
            Id = p.Id,
            Author = p.UserName ?? string.Empty,
            Title = p.Title,
            Cover = p.Cover ?? string.Empty,
            DatePublished = (DateTime)p.DatePublished,
            Slug = p.Slug,
            MainTag = p.Name,
            Description = p.Description
        });

        return new PagedList<PostListItemDto>(dtos, count, filter.Page, filter.PageSize);
    }

    public async Task<PostDto?> GetPostBySlugAsync(string slug)
    {
        var post = await _context.Posts
            .AsNoTracking()
            .Where(p => p.DatePublished != null)
            .Include(p => p.User)
            .Include(p => p.Tags)
            .Where(p => p.Slug == slug)
            .Select(p => new { p.Id, p.User.UserName, p.Content, p.Description, p.Cover, p.DatePublished, p.Slug, p.Title, p.Tags })
            .FirstOrDefaultAsync();

        if (post == null)
        {
            return null;
        }

        return new PostDto()
        {
            Slug = post.Slug,
            Title = post.Title,
            Cover = post.Cover ?? string.Empty,
            DatePublished = (DateTime)post.DatePublished!,
            Description = post.Description,
            Content = post.Content,
            Id = post.Id,
            Author = post.UserName ?? string.Empty,
            Tags = post.Tags.Select(t => t.Name)
        };
    }

    public async Task<PostNavigationDto> GetPostNavigationAsync(Guid postId)
    {
        PostNavigationDto nav = new();

        var date = await _context.Posts.AsNoTracking()
            .Where(p => p.Id == postId)
            .Select(p => new { p.DatePublished })
            .FirstOrDefaultAsync();

        if (date?.DatePublished == null)
        {
            return nav;
        }

        var previous = await _context.Posts
            .AsNoTracking()
            .Where(p => p.DatePublished != null && p.DatePublished < date.DatePublished)
            .OrderByDescending(p => p.DatePublished)
            .Select(p => new { p.Title, p.Slug })
            .FirstOrDefaultAsync();

        if (previous is not null)
        {
            nav.PreviousPost = new PostNavigationItem
            {
                Title = previous.Title,
                Slug = previous.Slug
            };
        }

        var next = await _context.Posts
            .AsNoTracking()
            .Where(p => p.DatePublished != null && p.DatePublished > date.DatePublished)
            .OrderByDescending(p => p.DatePublished)
            .Select(p => new { p.Title, p.Slug })
            .FirstOrDefaultAsync();

        if (next is not null)
        {
            nav.NextPost = new PostNavigationItem
            {
                Title = next.Title,
                Slug = next.Slug
            };
        }

        return nav;
    }

    public async Task<IEnumerable<PostListItemDto>> GetRecommendationsAsync(Guid id)
    {
        var tags = await _context.Posts.AsNoTracking()
            .Where(p => p.Id == id)
            .Select(p => new { p.Tags })
            .FirstOrDefaultAsync();

        if (tags is not null && tags.Tags.Any())
        {
            var posts = await _context.Posts
                .AsNoTracking()
                .Where(p => p.DatePublished != null && p.Id != id)
                .Where(p => p.Tags.Any(t => tags.Tags.Contains(t)))
                .OrderByDescending(p => p.Rating)
                .Select(p => new { p.Id, p.Title, p.User.UserName, p.Slug, p.Cover, p.Description, p.DatePublished, p.Tags.First().Name })
                .Take(3)
                .ToListAsync();

            return posts.Select(p => new PostListItemDto
            {
                Id = p.Id,
                Author = p.UserName ?? string.Empty,
                Title = p.Title,
                Cover = p.Cover ?? string.Empty,
                DatePublished = (DateTime)p.DatePublished!,
                Slug = p.Slug,
                MainTag = p.Name,
                Description = p.Description
            });
        }

        return new List<PostListItemDto>();
    }

    public async Task<IEnumerable<PostListItemDto>> GetPopularPostsAsync()
    {
        var posts = await _context.Posts
            .AsNoTracking()
            .Where(p => p.DatePublished != null)
            .OrderByDescending(p => p.PostViews)
            .ThenBy(p => p.Rating)
            .Select(p => new { p.Id, p.Title, p.User.UserName, p.Slug, p.Cover, p.Description, p.DatePublished, p.Tags.First().Name })
            .Take(3)
            .ToListAsync();

        return posts.Select(p => new PostListItemDto
        {
            Id = p.Id,
            Author = p.UserName ?? string.Empty,
            Title = p.Title,
            Cover = p.Cover ?? string.Empty,
            DatePublished = (DateTime)p.DatePublished!,
            Slug = p.Slug,
            MainTag = p.Name,
            Description = p.Description
        });
    }

    #region AdminRegion

    public async Task<IEnumerable<PostEditListItemDto>> GetPostsByStatusAsync(PublishedStatus status)
    {
        var posts = _context.Posts
            .AsNoTracking();

        switch (status)
        {
            case PublishedStatus.Published:
                posts = posts.Where(p => p.DatePublished != null);
                break;
            case PublishedStatus.All:
                break;
            case PublishedStatus.Drafts:
                posts = posts.Where(p => p.DatePublished == null);
                break;
            case PublishedStatus.Featured:
                posts = posts.Where(p => p.IsFeatured == true);
                break;
        }

        var postList = await posts
            .Select(p => new { p.Id, p.Title, p.DatePublished, p.IsFeatured })
            .ToListAsync();

        return posts.Select(p => new PostEditListItemDto
        {
            Id = p.Id,
            Title = p.Title,
            DatePublished = p.DatePublished,
            IsFeatured = p.IsFeatured
        });
    }

    public async Task<PostDto?> GetPostByIdAsync(Guid id)
    {
        var post = await _context.Posts
            .AsNoTracking()
            .Where(p => p.Id == id)
            .Include(p => p.User)
            .Include(p => p.Tags)
            .Select(p => new { p.Id, p.User.UserName, p.Content, p.Description, p.Cover, p.DatePublished, p.Slug, p.Title, p.Tags })
            .FirstOrDefaultAsync();

        if (post == null)
        {
            return null;
        }

        return new PostDto()
        {
            Slug = post.Slug,
            Title = post.Title,
            Cover = post.Cover ?? string.Empty,
            DatePublished = post.DatePublished ?? DateTime.MinValue,
            Description = post.Description,
            Content = post.Content,
            Id = post.Id,
            Author = post.UserName ?? string.Empty,
            Tags = post.Tags.Select(t => t.Name)
        };
    }

    public async Task<Guid> CreatePostAsync(Guid userId, PostEditDto post)
    {
        var newPost = new Post
        {
            Content = post.Content,
            Cover = post.Cover,
            DateCreated = DateTime.UtcNow,
            DatePublished = null,
            DateUpdated = null,
            Description = post.Description,
            IsFeatured = true,
            PostViews = 0,
            Rating = 0,
            Slug = post.Slug,
            Title = post.Title,
            PostType = PostType.Post,
            UserId = userId
        };

        var existingTags = await _context.Tags
            .Where(t => post.Tags.Contains(t.Name))
            .ToListAsync();

        var notExistingTags = post.Tags
            .Where(t => !existingTags.Select(x => x.Name)
                .Contains(t));

        existingTags.AddRange(notExistingTags.Select(t => new Tag
        {
            Id = Guid.NewGuid(),
            Name = t
        }));

        newPost.Tags = existingTags;

        _context.Posts.Add(newPost);
        await _context.SaveChangesAsync();
        return newPost.Id;
    }

    public async Task<bool> UpdatePostAsync(Guid id, PostEditDto post)
    {
        var existingPost = await _context.Posts
            .Include(p => p.Tags)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (existingPost != null)
        {
            existingPost.Slug = post.Slug;
            existingPost.Title = post.Title;
            existingPost.Description = post.Description;
            existingPost.Content = post.Content;
            existingPost.DateUpdated = DateTime.UtcNow;

            if (!string.IsNullOrEmpty(post.Cover))
            {
                existingPost.Cover = post.Cover;
            }

            // tags
            var existingTags = await _context.Tags
                .Where(t => post.Tags.Contains(t.Name))
                .ToListAsync();

            var notExistingTags = post.Tags
                .Where(t => !existingTags.Select(x => x.Name)
                    .Contains(t));

            var newTags = notExistingTags
                .Select(t => new Tag
                {
                    Id = Guid.NewGuid(),
                    Name = t
                }).ToList();

            _context.Tags.AddRange(newTags);
            await _context.SaveChangesAsync();

            existingTags.AddRange(newTags);
            existingPost.Tags = existingTags;

            await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }

    public async Task<bool> PublishPostAsync(Guid postId, bool isPublished)
    {
        var post = await _context.Posts
            .FirstOrDefaultAsync(p => p.Id == postId);

        if (post is not null)
        {
            post.DatePublished = isPublished ? DateTime.UtcNow : null;
            await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }

    public async Task<bool> FeaturePostAsync(Guid postId, bool isFeatured)
    {
        var post = await _context.Posts
            .FirstOrDefaultAsync(p => p.Id == postId);

        if (post is not null)
        {
            post.IsFeatured = isFeatured;
            await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }

    public async Task<bool> DeletePostAsync(Guid postId)
    {
        try
        {
            var post = _context.Posts.Attach(new Post { Id = postId });
            post.State = EntityState.Deleted;
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    #endregion
}