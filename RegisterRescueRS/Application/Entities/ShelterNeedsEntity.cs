namespace RegisterRescueRS.Domain.Application.Entities;

public class ShelterNeedsEntity
{
    public Guid ShelterNeedsId { get; set; }
    public Guid ShelterId { get; set; }
    public bool AcceptingVolunteers { get; set; }
    public bool AcceptingDoctors { get; set; }
    public bool AcceptingVeterinarians { get; set; }
    public bool AcceptingDonations { get; set; }
    public bool Avaliable { get; set; }
    public string? DonationDescription { get; set; }
    public string? VolunteersSubscriptionLink { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public ShelterEntity Shelter { get; set; } = null!;
}