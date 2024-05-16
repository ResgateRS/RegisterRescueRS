namespace RegisterRescueRS.Domain.Application.Entities;

public class ShelterEntity
{
    public Guid ShelterId { get; set; }
    public string Login { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string ShelterName { get; set; } = null!;
    public string Address { get; set; } = null!;
    public bool Adm { get; set; }
    public double Latitude { get; set; } = 0;
    public double Longitude { get; set; } = 0;
    public bool? Verified { get; set; }
    public string? ShelterCellphone { get; set; } = null!;

    public ICollection<FamilyEntity> Families { get; set; } = [];
    public ICollection<ShelterNeedsEntity>? ShelterNeeds { get; set; }

    internal double GetDistance(double latitude, double longitude) =>
        Math.Sqrt(Math.Pow(Latitude - latitude, 2) + Math.Pow(Longitude - longitude, 2));
}