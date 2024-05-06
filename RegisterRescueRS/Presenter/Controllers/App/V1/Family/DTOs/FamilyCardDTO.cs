using RegisterRescueRS.Domain.Application.Entities;

namespace RegisterRescueRS.Presenter.Controllers.App.V1.DTOs;
public class FamilyCardDTO
{
    public Guid FamilyId { get; set; }
    public int TotalPeopleNumber { get; set; }
    public string Responsable { get; set; } = null!;

    public static FamilyCardDTO FromEntity(FamilyEntity entity) =>
        new()
        {
            FamilyId = entity.FamilyId,
            TotalPeopleNumber = entity.Houseds.Count(),
            Responsable = entity.Responsable.Name
        };
}
