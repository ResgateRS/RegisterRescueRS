namespace RegisterRescueRS.Domain.Application.Entities;

public class ShelterEntity
{
    public Guid ShelterId { get; set; }
    public string Login { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string ShelterName { get; set; } = null!;
    public string Address { get; set; } = null!;

    public ICollection<FamilyEntity> Families { get; set; } = [];
    public ICollection<ShelterNeedEntity> ShelterNeeds { get; set; } = [];
}