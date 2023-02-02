namespace MicroBlog.WebApi.Services.Implementations;

public class YouTubeService : IYouTubeService
{
    private readonly AppDbContext _context;

    public YouTubeService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<string>> GetRecommendationsAsync(int count = 2)
    {
        var random = new Random();

        var maxCount = _context.YoutubeRecommendations
            .Count(x => x.IsEnable);

        if (maxCount == 0)
        {
            return new List<string>();
        }

        var toSkip = 0;
        if (maxCount > count)
        {
            toSkip = random.Next(1, _context.YoutubeRecommendations.Count());
        }

        var result = await _context.YoutubeRecommendations
            .Where(x => x.IsEnable)
            .OrderBy(x => x.Id)
            .Skip(toSkip)
            .Take(count)
            .ToListAsync();

        return result.Select(x => $"https://www.youtube.com/embed/{x.VideoId}");
    }

    public async Task<IEnumerable<YouTubeDetailedRecommendationDto>> GetAllRecommendationsAsync()
    {
        var result = await _context.YoutubeRecommendations
            .ToListAsync();

        return result.Select(x =>
            new YouTubeDetailedRecommendationDto(x.Id, x.VideoId, x.IsEnable, x.VideoName, x.ChannelName, x.ChannelLink));
    }

    public async Task AddRecommendationAsync(YouTubeRecommendationDto dto)
    {
        var model = new YoutubeRecommendation()
        {
            Id = Guid.NewGuid(),
            VideoId = dto.VideoId,
            ChannelLink = dto.ChannelLink,
            ChannelName = dto.ChannelName,
            VideoName = dto.ChannelName,
            IsEnable = true
        };

        _context.YoutubeRecommendations.Add(model);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> UpdateRecommendationAsync(Guid id, YouTubeRecommendationDto dto)
    {
        var result = await _context.YoutubeRecommendations
            .FirstOrDefaultAsync(x => x.Id == id);

        if (result is null)
        {
            return false;
        }

        result.ChannelName = dto.ChannelName;
        result.VideoName = dto.VideoName;
        result.VideoId = dto.VideoId;
        result.ChannelLink = dto.ChannelName;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteRecommendationAsync(Guid id)
    {
        var result = await _context.YoutubeRecommendations
            .FirstOrDefaultAsync(x => x.Id == id);

        if (result is null)
        {
            return false;
        }

        _context.Remove(result);
        await _context.SaveChangesAsync();

        return true;
    }
}