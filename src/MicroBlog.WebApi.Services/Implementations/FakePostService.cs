namespace MicroBlog.WebApi.Services.Implementations;

public class FakePostService : IPostService
{
    public Task<IEnumerable<TagDto>> GetAllTagsAsync()
    {
        var tags = new List<TagDto>()
        {
            new TagDto(Guid.NewGuid(), "Business"),
            new TagDto(Guid.NewGuid(), "Coding"),
            new TagDto(Guid.NewGuid(), "Design"),
            new TagDto(Guid.NewGuid(), "Food"),
            new TagDto(Guid.NewGuid(), "Hosting"),
            new TagDto(Guid.NewGuid(), "Learn"),
            new TagDto(Guid.NewGuid(), "People"),
            new TagDto(Guid.NewGuid(), "Photography"),
            new TagDto(Guid.NewGuid(), "Projects"),
            new TagDto(Guid.NewGuid(), "Technology")
        };

        return Task.FromResult((IEnumerable<TagDto>)tags);
    }
}
