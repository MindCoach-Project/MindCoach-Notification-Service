namespace MinhCoach_Notification_Service.Domain.Common.Models;

public abstract class Model<TId, TIdType> : Entity<TId>
    where TId : ModelId<TIdType>
{
    public new TId Id { get; private set; }
    protected Model(TId id) : base(id)
    {
        Id = id;
    }

#pragma warning disable CS8618
    protected Model()
    {

    }
#pragma warning disable CS8618
}
