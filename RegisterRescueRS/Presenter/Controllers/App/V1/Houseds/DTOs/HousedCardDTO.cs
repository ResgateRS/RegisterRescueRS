using RegisterRescueRS.Domain.Application.Entities;

namespace RegisterRescueRS.Presenter.Controllers.App.V1.DTOs;
public class HousedCardDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Cellphone { get; set; }
    public int Age { get; set; }
    public bool Responsable { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    internal static HousedCardDTO FromEntity(HousedEntity entity) =>
        new()
        {
            Id = entity.HousedId,
            Name = entity.Name,
            Age = entity.Age,
            Responsable = entity.IsFamilyResponsable,
            Cellphone = entity.Cellphone,
            Latitude = entity.Family.Shelter.Latitude,
            Longitude = entity.Family.Shelter.Longitude,
            UpdatedAt = entity.UpdatedAt
        };
}