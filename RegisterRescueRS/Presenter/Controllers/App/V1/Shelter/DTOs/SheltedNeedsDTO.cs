using RegisterRescueRS.Domain.Application.Entities;

namespace RegisterRescueRS.Presenter.Controllers.App.V1.DTOs;
public class ShelterNeedsDTO
{
    public bool AcceptingVolunteers { get; set; }
    public bool AcceptingDoctors { get; set; }
    public bool AcceptingVeterinarians { get; set; }
    public bool AcceptingDonations { get; set; }
    public bool Avaliable { get; set; }
    public string? DonationDescription { get; set; }
    public string? VolunteersSubscriptionLink { get; set; }

    internal static ShelterNeedsDTO FromEntity(ShelterNeedsEntity entity) =>
        new ShelterNeedsDTO
        {
            AcceptingVolunteers = entity.AcceptingVolunteers,
            AcceptingDoctors = entity.AcceptingDoctors,
            AcceptingVeterinarians = entity.AcceptingVeterinarians,
            AcceptingDonations = entity.AcceptingDonations,
            Avaliable = entity.Avaliable,
            DonationDescription = entity.DonationDescription,
            VolunteersSubscriptionLink = entity.VolunteersSubscriptionLink
        };
}