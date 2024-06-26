namespace RegisterRescueRS.Domain.Application.Entities;

public class FamilyEntity
{
    public Guid FamilyId { get; set; }
    public Guid ShelterId { get; set; }
    public ShelterEntity Shelter { get; set; } = null!;

    public DateTimeOffset RegisteredAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public bool Active { get; set; }

    public ICollection<HousedEntity> Houseds { get; set; } = [];
}