﻿namespace MicroBlog.UI.Models;

public class PostCardModel : BasePostModel
{
    public string Text { get; init; } = default!;
    public string Tag { get; init; } = default!;
    public string Author { get; init; } = default!;
}