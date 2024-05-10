using RegisterRescueRS.Domain.Application.Entities;

namespace RegisterRescueRS.Presenter.Controllers.App.V1.DTOs;
public class VolunteerDTO
{
    public Guid ShelterId { get; set; }
    public bool AcceptingVolunteers { get; set; }
    public bool AcceptingDoctors { get; set; }
    public bool AcceptingVeterinarians { get; set; }
    public bool AcceptingDonations { get; set; }
    public bool Avaliable { get; set; }
    public string? Address { get; set; }
    public string? VolunteersSubscriptionLink { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? ShelterName { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public static VolunteerDTO FromEntity(ShelterNeedsEntity entity) =>
        new()
        {
            ShelterId = entity.ShelterId,
            ShelterName = entity.Shelter.ShelterName,
            AcceptingVolunteers = entity.AcceptingVolunteers,
            AcceptingDoctors = entity.AcceptingDoctors,
            AcceptingVeterinarians = entity.AcceptingVeterinarians,
            AcceptingDonations = entity.AcceptingDonations,
            Address = entity.Shelter.Address,
            VolunteersSubscriptionLink = entity.VolunteersSubscriptionLink,
            Avaliable = entity.Avaliable,
            Latitude = entity.Shelter.Latitude,
            Longitude = entity.Shelter.Longitude,
            UpdatedAt = entity.UpdatedAt
        };
}