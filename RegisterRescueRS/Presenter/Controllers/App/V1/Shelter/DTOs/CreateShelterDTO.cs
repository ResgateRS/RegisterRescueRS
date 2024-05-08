namespace RegisterRescueRS.Presenter.Controllers.App.V1.DTOs;
public class CreateShelterDTO
{
    public string Name { get; set; } = null!;
    public string Login { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Address { get; set; } = null!;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
