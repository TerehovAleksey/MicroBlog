namespace MicroBlog.Common.Validations.Youtube;

public class YouTubeRecommendationDtoValidator : AbstractValidator<YouTubeRecommendationDto>
{
    public YouTubeRecommendationDtoValidator()
    {
        RuleFor(x => x.VideoId)
            .NotNull()
            .NotEmpty()
            .MaximumLength(160);

        RuleFor(x => x.VideoName)
            .MaximumLength(250);

        RuleFor(x => x.ChannelName)
            .MaximumLength(50);

        RuleFor(x => x.ChannelLink)
            .MaximumLength(160);
    }
}