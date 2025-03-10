using MinhCoach_Notification_Service.Domain.Common.Models;

namespace MinhCoach_Notification_Service.Domain.User.ValueObjects;

public class UserId : ModelId<Guid>
{
    public override Guid Value { get; protected set; }
    private UserId(Guid value)
    {
        Value = value;
    }
    
    public static UserId CreateUnique()
    {
        return new(Guid.NewGuid());
    }
    
    public static UserId Create(string id)
    {
        return new(Guid.Parse(id));
    }
    
    public static UserId Create(Guid id)
    {
        return new(id);
    }
    
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}