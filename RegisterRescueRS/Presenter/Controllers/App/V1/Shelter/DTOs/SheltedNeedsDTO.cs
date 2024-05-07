using RegisterRescueRS.Domain.Application.Entities;

namespace RegisterRescueRS.Presenter.Controllers.App.V1.DTOs;
public class ShelterNeedsDTO
{
    public Guid ShelterId { get; set; }
    public bool AcceptingVolunteers { get; set; }
    public bool AcceptingDoctors { get; set; }
    public bool AcceptingVeterinarians { get; set; }
    public bool AcceptingDonations { get; set; }
    public string? DonationDescription { get; set; }

    internal static ShelterNeedsDTO FromEntity(ShelterNeedsEntity entity) =>
        new()
        {
            ShelterId = entity.ShelterId,
            AcceptingVolunteers = entity.AcceptingVolunteers,
            AcceptingDoctors = entity.AcceptingDoctors,
            AcceptingVeterinarians = entity.AcceptingVeterinarians,
            AcceptingDonations = entity.AcceptingDonations,
            DonationDescription = entity.DonationDescription
        };
}