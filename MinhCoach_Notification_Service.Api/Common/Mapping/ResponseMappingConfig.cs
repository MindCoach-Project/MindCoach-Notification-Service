using Mapster;
using MinhCoach_Notification_Service.App.Common.Response;

namespace MinhCoach_Notification_Service.Api.Common.Mapping;

public class ResponseMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig(typeof(ObjectResponse<>), typeof(ApiResponse<>))
            .Map("Message", "Message")
            .Map("Data", "Data");
    }
}