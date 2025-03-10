using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.SignalR;

namespace MinhCoach_Notification_Service.Api.Hubs;

public class ReminderHub : Hub
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ReminderHub(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    public override async Task OnConnectedAsync()
    {
        var userId = GetUserId();
        
        if (userId != Guid.Empty)
        {
            Console.WriteLine($"User {userId} connected");
            await Groups.AddToGroupAsync(Context.ConnectionId, userId.ToString());
        }
        
        await base.OnConnectedAsync();
    }

    private Guid GetUserId()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        
        var token = httpContext.Request.Query["access_token"];
        if (string.IsNullOrEmpty(token)) return Guid.Empty;
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = jwtTokenHandler.ReadToken(token) as JwtSecurityToken;

        if (jwtToken == null) return  Guid.Empty;
        var userIdClaim = jwtToken?.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.UniqueName);
        if (userIdClaim == null) return Guid.Empty;
        
        if (!Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return  Guid.Empty;
        }

        return userId;
    }
}