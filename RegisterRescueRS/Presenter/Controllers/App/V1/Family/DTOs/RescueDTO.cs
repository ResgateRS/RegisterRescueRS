using RegisterRescueRS.Domain.Application.Entities;

namespace RegisterRescueRS.Presenter.Controllers.App.V1.DTOs;
public class FamilyDTO
{
    public Guid FamilyId { get; set; }
    public Guid ShelterId { get; set; }
    public string Responsable { get; set; } = null!;

    public DateTimeOffset RegisteredAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public IEnumerable<HousedDTO> Houseds { get; set; } = [];

    public static FamilyDTO FromEntity(FamilyEntity entity) =>
        new()
        {
            FamilyId = entity.FamilyId,
            ShelterId = entity.ShelterId,
            Responsable = entity.Houseds.First(x => x.IsFamilyResponsable).Name,
            RegisteredAt = entity.RegisteredAt,
            UpdatedAt = entity.UpdatedAt,
            Houseds = entity.Houseds.Select(HousedDTO.FromEntity),
        };
}
