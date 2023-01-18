namespace MicroBlog.WebApi.Services.Implementations;

public class PostService : IPostService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public PostService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TagDto>> GetAllTagsAsync()
    {
        var tags = _context.Tags
            .AsNoTracking();

        var result = await _mapper.ProjectTo<TagDto>(tags).ToListAsync();
        return result;
    }
}