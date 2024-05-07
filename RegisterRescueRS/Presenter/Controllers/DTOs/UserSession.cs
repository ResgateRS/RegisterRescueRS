using static RegisterRescueRS.Tools.JwtManager;

namespace RegisterRescueRS.Auth;

public class UserSession : IJwtPayload
{
    public Guid ShelterId { get; set; }
}