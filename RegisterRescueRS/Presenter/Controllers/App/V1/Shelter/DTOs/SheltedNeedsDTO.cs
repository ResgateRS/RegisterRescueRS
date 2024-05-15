using System.Text.Json.Serialization;
using RegisterRescueRS.Domain.Application.Entities;

namespace RegisterRescueRS.Presenter.Controllers.App.V1.DTOs;
public class ShelterNeedsDTO
{
    //TODO: Update fields in frontend to match original DTO and remove JsonPropertyName attributes
    public bool AcceptingVolunteers { get; set; }

    public bool AcceptingDoctors { get; set; }

    [JsonPropertyName("acceptingVeterinary")]
    public bool AcceptingVeterinarians { get; set; }

    public bool AcceptingDonations { get; set; }

    [JsonPropertyName("acceptingUnsheltered")]
    public bool Avaliable { get; set; }

    public string? DonationsDescription { get; set; }

    [JsonPropertyName("formLink")]
    public string? VolunteersSubscriptionLink { get; set; }

    internal static ShelterNeedsDTO FromEntity(ShelterNeedsEntity entity) =>
        new ShelterNeedsDTO
        {
            AcceptingVolunteers = entity.AcceptingVolunteers,
            AcceptingDoctors = entity.AcceptingDoctors,
            AcceptingVeterinarians = entity.AcceptingVeterinarians,
            AcceptingDonations = entity.AcceptingDonations,
            Avaliable = entity.Avaliable,
            DonationsDescription = entity.DonationDescription,
            VolunteersSubscriptionLink = entity.VolunteersSubscriptionLink
        };
}