using MicroBlog.UI.Models;
using Microsoft.AspNetCore.Components;

namespace MicroBlog.UI.Services;

public class FilterService : IFilterService
{
    private readonly NavigationManager _navigationManager;
    
    public PostFilter Filter { get; }
    public event Action<PostFilter>? FilterChanged;

    public FilterService(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;
        Filter = new PostFilter();
    }

    public void SearchByTag(string tag)
    {
        ClearFilter();
        Filter.Tag = tag;
        _navigationManager.NavigateTo("/");
        FilterChanged?.Invoke(Filter);
    }

    public void SearchByAuthor(string author)
    {
        ClearFilter();
        Filter.Author = author;
        _navigationManager.NavigateTo("/");
        FilterChanged?.Invoke(Filter);
    }

    public void SearchByDate(string date)
    {
        ClearFilter();
        Filter.Date = date;
        _navigationManager.NavigateTo("/");
        FilterChanged?.Invoke(Filter);
    }

    public void SearchByQuery(string query)
    {
        ClearFilter();
        Filter.Query = query;
        _navigationManager.NavigateTo("/");
        FilterChanged?.Invoke(Filter);
    }

    public void Reset()
    {
        ClearFilter();
        _navigationManager.NavigateTo("/");
        FilterChanged?.Invoke(Filter);
    }

    private void ClearFilter()
    {
        Filter.Tag = null;
        Filter.Author = null;
        Filter.Date = null;
        Filter.Query = null;
        Filter.Page = 1;
    }
}