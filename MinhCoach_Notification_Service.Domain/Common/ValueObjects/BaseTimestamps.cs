using MinhCoach_Notification_Service.Domain.Common.Models;

namespace MinhCoach_Notification_Service.Domain.Common.ValueObjects;

public class BaseTimestamps : ValueObject
{
    public DateTime CreatedAt { get; protected set; }
    public DateTime? UpdatedAt { get; protected set; }

    protected BaseTimestamps(DateTime createdAt, DateTime? updatedAt = null)
    {
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public BaseTimestamps UpdateTimestamp()
    {
        return new BaseTimestamps(CreatedAt, DateTime.UtcNow);
    }
    
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return CreatedAt;
        yield return UpdatedAt;
    }
}