using Microsoft.AspNetCore.Mvc;
using RegisterRescueRS.Attributes;
using RegisterRescueRS.Domain.Application.Services;
using RegisterRescueRS.DTOs;
using RegisterRescueRS.Presenter.Controllers.App.V1.DTOs;

namespace RegisterRescueRS.Presenter.Controllers.App.V1;

[ApiController]
[Route("api/v{version:apiVersion}/Shelter")]
[ApiVersion("1.0")]
public class ShelterController(IServiceProvider serviceProvider) : BaseController(serviceProvider)
{
    [HttpPost]
    [MapToApiVersion("1.0")]
    public async Task<IResponse<ResponseDTO>> CreateShelter(CreateShelterDTO dto) =>
        await this.serviceProvider.GetRequiredService<ShelterService>()
            .CreateShelter(dto);

    [HttpPost("Needs")]
    [MapToApiVersion("1.0")]
    public async Task<IResponse<ResponseDTO>> UpsertNeeds(ShelterNeedsDTO dto) =>
        await this.serviceProvider.GetRequiredService<ShelterService>()
            .UpsertNeeds(dto);

    [SkipAuthentication]
    [PaginatedRequest("Id da ultima Doação", PaginationType.Cursor, typeof(Guid))]
    [HttpGet("ListDonations")]
    [MapToApiVersion("1.0")]
    public async Task<IResponse<IEnumerable<DonationDTO>>> ListDonations() =>
        await this.serviceProvider.GetRequiredService<ShelterService>()
            .ListDonations();

    [SkipAuthentication]
    [PaginatedRequest("Id do ultimo Voluntário", PaginationType.Cursor, typeof(Guid))]
    [HttpGet("ListVolunteers")]
    [MapToApiVersion("1.0")]
    public async Task<IResponse<IEnumerable<VolunteerDTO>>> ListVolunteers() =>
        await this.serviceProvider.GetRequiredService<ShelterService>()
            .ListVolunteers();
}

