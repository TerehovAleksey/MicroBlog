namespace MicroBlog.WebApi.Controllers;

[ApiController]
[Route("api/youtube")]
[Produces(MediaTypeNames.Application.Json)]
public class YoutubeController : ControllerBase
{
    private const string CacheKey = "recommendations";
    
    private readonly IYouTubeService _youtubeService;
    private readonly IMemoryCache _cache;

    public YoutubeController(IYouTubeService youtubeService, IMemoryCache cache)
    {
        _youtubeService = youtubeService;
        _cache = cache;
    }

    [HttpGet("random")]
    [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRecommendationsAsync()
    {
        if (_cache.TryGetValue(CacheKey, out List<string>? result))
        {
            if (result is not null && result.Any()) 
            {
                return Ok(result);
            }
        }

        var records = await _youtubeService.GetRecommendationsAsync();
        result = records.ToList();

        if (!result.Any())
        {
            return Ok(result);
        }
        
        var memoryCacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromDays(1))
            .SetPriority(CacheItemPriority.Normal);
        
        _cache.Set(CacheKey, result, memoryCacheEntryOptions);

        return Ok(result);
    }

    [HttpGet("my")]
    [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetMyLastVideoAsync()
    {
        return NotFound();
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<YouTubeDetailedRecommendationDto>), StatusCodes.Status200OK)]
    public Task<IEnumerable<YouTubeDetailedRecommendationDto>> GetAllRecommendationsAsync() =>
        _youtubeService.GetAllRecommendationsAsync();

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> AddRecommendationAsync([FromBody] YouTubeRecommendationDto dto)
    {
        await _youtubeService.AddRecommendationAsync(dto);
        _cache.Remove(CacheKey);
        return Ok();
    }
}