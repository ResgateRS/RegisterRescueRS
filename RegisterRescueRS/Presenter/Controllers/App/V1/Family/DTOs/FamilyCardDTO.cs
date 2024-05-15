using RegisterRescueRS.Domain.Application.Entities;

namespace RegisterRescueRS.Presenter.Controllers.App.V1.DTOs;
public class FamilyCardDTO
{
    public Guid FamilyId { get; set; }
    public int TotalPeopleNumber { get; set; }
    public string Responsable { get; set; } = null!;
    public string? Shelter { get; set; }
    public string? Cellphone { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    internal static FamilyCardDTO FromEntity(FamilyEntity entity) =>
        new()
        {
            FamilyId = entity.FamilyId,
            Responsable = entity.Houseds.First(x => x.IsFamilyResponsable).Name,
            Cellphone = entity.Houseds.First(x => x.IsFamilyResponsable).Cellphone,
            Shelter = $"{entity.Shelter.ShelterName} - {entity.Shelter.Address}",
            TotalPeopleNumber = entity.Houseds.Count(x => x.Active),
            Latitude = entity.Shelter.Latitude,
            Longitude = entity.Shelter.Longitude,
            UpdatedAt = entity.UpdatedAt
        };
}
