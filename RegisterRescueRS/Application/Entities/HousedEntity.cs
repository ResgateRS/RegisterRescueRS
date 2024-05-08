namespace RegisterRescueRS.Domain.Application.Entities;

public class HousedEntity
{
    public Guid HousedId { get; set; }
    public Guid FamilyId { get; set; }
    public string Name { get; set; } = null!;
    public int Age { get; set; }
    public string? Cellphone { get; set; }
    public bool IsFamilyResponsable { get; set; }
    public DateTimeOffset RegisteredAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public FamilyEntity Family { get; set; } = null!;
}