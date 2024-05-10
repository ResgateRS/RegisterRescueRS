using static RegisterRescueRS.Tools.JwtManager;

namespace RegisterRescueRS.Auth;

public class UserSession : IJwtPayload
{
    public Guid ShelterId { get; set; }
    public string? ShelterName { get; set; }
    public bool Adm { get; set; }
}