using RegisterRescueRS.Domain.Application.Entities;

namespace RegisterRescueRS.Presenter.Controllers.App.V1.DTOs;
public class DonationDTO
{
    public Guid ShelterId { get; set; }
    public bool AcceptingDonations { get; set; }
    public string? ShelterName { get; set; }
    public string? Address { get; set; }
    public string? DonationDescription { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    internal static DonationDTO FromEntity(ShelterNeedsEntity entity) =>
        new()
        {
            ShelterId = entity.ShelterId,
            ShelterName = entity.Shelter.ShelterName,
            AcceptingDonations = entity.AcceptingDonations,
            Address = entity.Shelter.Address,
            DonationDescription = entity.DonationDescription,
            Latitude = entity.Shelter.Latitude,
            Longitude = entity.Shelter.Longitude
        };
}