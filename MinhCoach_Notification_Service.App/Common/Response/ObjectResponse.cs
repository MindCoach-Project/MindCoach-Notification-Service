namespace MinhCoach_Notification_Service.App.Common.Response;

public class ObjectResponse<T>
{
    public string Message { get; set; }
    public T Data { get; set; }

    public ObjectResponse(string message, T data)
    {
        Message = message;
        Data = data;
    }
}
