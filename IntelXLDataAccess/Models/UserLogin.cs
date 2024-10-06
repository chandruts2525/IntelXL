using System.Text.Json.Serialization;

namespace IntelXLDataAccess.Models;

public partial class UserLogin
{
    [JsonPropertyName("userLoginId")]
    public int UserLoginId { get; set; }
    [JsonPropertyName("userId")]
    public int UserId { get; set; }
    [JsonPropertyName("loginDttm")]
    public DateTime LoginDttm { get; set; }
    [JsonPropertyName("logoutDttm")]
    public DateTime? LogoutDttm { get; set; }
    [JsonPropertyName("deviceIpAddress")]
    public string DeviceIpAddress { get; set; } = null!;
    [JsonPropertyName("appUser")]
    public virtual AppUser? AppUser { get; set; }
}
