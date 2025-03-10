using MinhCoach_Notification_Service.Api;
using MinhCoach_Notification_Service.Api.Hubs;
using MinhCoach_Notification_Service.App;
using MinhCoach_Notification_Service.Infra;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        .AddPresentation()
        .AddApplication()   
        .AddInfrastructure(builder.Configuration);
}

var app = builder.Build();
{
    app.UseHttpsRedirection(); 
  
    app.UseWebSockets();
    
    app.UseCors("AllowAll");

    app.UseAuthentication();
    app.UseAuthorization();
    
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.MapControllers();
    
    app.MapHub<ReminderHub>("/reminderHub");
    
    app.Run();
}