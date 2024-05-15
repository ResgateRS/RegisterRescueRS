using RegisterRescueRS.Domain.Application.Entities;

namespace RegisterRescueRS.Presenter.Controllers.App.V1.DTOs;
public class FamilyCardDTO
{
    public Guid FamilyId { get; set; }
    public int TotalPeopleNumber { get; set; }
    public string Responsable { get; set; } = null!;
    public string? Cellphone { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    // TODO: Update DTO attributes to expose shelter and shelter address properly
    internal static FamilyCardDTO FromEntity(FamilyEntity entity) =>
        new()
        {
            FamilyId = entity.FamilyId,
            Responsable = $"{entity.Houseds.First(x => x.IsFamilyResponsable).Name}\n{entity.Shelter.ShelterName} - {entity.Shelter.Address}",
            Cellphone = entity.Houseds.First(x => x.IsFamilyResponsable).Cellphone,
            TotalPeopleNumber = entity.Houseds.Count(x => x.Active),
            Latitude = entity.Shelter.Latitude,
            Longitude = entity.Shelter.Longitude,
            UpdatedAt = entity.UpdatedAt
        };
}
