using RegisterRescueRS.Domain.Application.Entities;

namespace RegisterRescueRS.Presenter.Controllers.App.V1.DTOs;

public class ShelterDTO
{
    public Guid ShelterId { get; set; }
    public string ShelterName { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string ShelterCellphone { get; set; } = null!;

    internal static ShelterDTO FromEntity(ShelterEntity entity) =>
        new()
        {
            ShelterId = entity.ShelterId,
            ShelterName = entity.ShelterName,
            Address = entity.Address,
            ShelterCellphone = entity.ShelterCellphone
        };
}