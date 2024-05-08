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
}