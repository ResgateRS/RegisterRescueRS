using RegisterRescueRS.Domain.Application.Entities;

namespace RegisterRescueRS.Presenter.Controllers.App.V1.DTOs;
public class HousedDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Cellphone { get; set; }
    public int Age { get; set; }
    public bool Responsable { get; set; }

    internal static HousedDTO FromEntity(HousedEntity entity) =>
        new()
        {
            Id = entity.HousedId,
            Name = entity.Name,
            Age = entity.Age,
            Responsable = entity.IsFamilyResponsable,
            Cellphone = entity.Cellphone
        };
}