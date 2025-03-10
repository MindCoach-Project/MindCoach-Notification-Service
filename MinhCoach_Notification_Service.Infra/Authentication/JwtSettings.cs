namespace MinhCoach_Notification_Service.Infra.Authentication;

public class JwtSettings
{
    public static string SectionName = "JwtSettings";
    public string Secret { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public double ExpiryMinutes { get; set; }
}