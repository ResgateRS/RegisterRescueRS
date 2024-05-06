using RegisterRescueRS.Domain.Application.Entities;

namespace RegisterRescueRS.Presenter.Controllers.App.V1.DTOs;
public class FamilyRequestDTO
{
    public Guid? FamilyId { get; set; }

    public IEnumerable<HousedDTO> Houseds { get; set; } = [];
}
