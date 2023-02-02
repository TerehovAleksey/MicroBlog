namespace MicroBlog.Common.Validations.Posts;

public class PostEditDtoValidator : AbstractValidator<PostEditDto>
{
    public PostEditDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(160);

        RuleFor(x => x.Slug)
            .NotEmpty()
            .MaximumLength(160);

        RuleFor(x => x.Cover)
            .MaximumLength(160);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(450);

        RuleFor(x => x.Content)
            .NotEmpty();

        RuleFor(x => x.Tags)
            .Must(t => t.Any());
    }
}
