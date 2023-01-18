namespace MicroBlog.UI.Services;

public interface INavigationService
{
    public event Action<string>? Navigated;
    public void Navigate(string slug);
}
