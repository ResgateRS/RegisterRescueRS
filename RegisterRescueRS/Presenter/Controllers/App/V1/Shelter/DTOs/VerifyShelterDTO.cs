namespace RegisterRescueRS.Presenter.Controllers.App.V1.DTOs;

public class VerifyShelterDTO
{
    public Guid ShelterId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}