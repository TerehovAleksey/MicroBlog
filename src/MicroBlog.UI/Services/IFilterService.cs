namespace MicroBlog.UI.Services;

public interface IFilterService
{
    public PostFilter Filter { get; }
    public event Action<PostFilter>? FilterChanged;
    public void Reset();
    public void SearchByTag(string tag);
    public void SearchByAuthor(string author);
    public void SearchByDate(DateTime date);
    public void SearchByQuery(string query);
}