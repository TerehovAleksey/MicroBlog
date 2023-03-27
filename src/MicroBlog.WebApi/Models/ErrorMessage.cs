namespace MicroBlog.WebApi.Models;

public class ErrorMessage
{
    [JsonPropertyName("errors")]
    public Message Message { get; init; }

    private ErrorMessage()
    {
        Message = new Message();
    }

    public static ErrorMessage Create(string message)
    {
        return new ErrorMessage()
        {
            Message = new Message() { Errors = new List<string>() { message } }
        };
    }

    public static ErrorMessage Create(IEnumerable<string> messages)
    {
        return new ErrorMessage()
        {
            Message = new Message() { Errors = new List<string>(messages) }
        };
    }
}

public class Message
{
    [JsonPropertyName("messages")]
    public IEnumerable<string>? Errors { get; init; }
}
