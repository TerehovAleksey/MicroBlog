﻿namespace MicroBlog.WebApi.Models;

public class Tag
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
}