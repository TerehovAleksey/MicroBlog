namespace MicroBlog.UI.Services;

public class NavigationService : INavigationService
{
    private readonly NavigationManager _navigationManager;

    public event Action<string>? Navigated;

    public NavigationService(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;
    }

    public void Navigate(string slug)
    {
        if (string.IsNullOrWhiteSpace(slug))
        {
            return;
        }
        _navigationManager.NavigateTo(@"/post/" + slug);
        Navigated?.Invoke(slug);
    }
}
