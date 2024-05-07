namespace RegisterRescueRS.Domain.Application.Entities;

public class ShelterNeedEntity
{
    public Guid ShelterNeedId { get; set; }
    public Guid ShelterId { get; set; }
    public bool AcceptingVolunteers { get; set; }
    public bool AcceptingDoctors { get; set; }
    public bool AcceptingVeterinarians { get; set; }
    public bool AcceptingDonations { get; set; }
    public string? DonationDescription { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public ShelterEntity Shelter { get; set; } = null!;
}