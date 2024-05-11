using Microsoft.AspNetCore.Mvc;
using RegisterRescueRS.Attributes;
using RegisterRescueRS.Domain.Application.Services;
using RegisterRescueRS.DTOs;
using RegisterRescueRS.Presenter.Controllers.App.V1.DTOs;

namespace RegisterRescueRS.Presenter.Controllers.App.V1;

[ApiController]
[Route("api/v{version:apiVersion}/Family")]
[ApiVersion("1.0")]
public class FamilyController(IServiceProvider serviceProvider) : BaseController(serviceProvider)
{
    [HttpPost]
    [MapToApiVersion("1.0")]
    public async Task<IResponse<ResponseDTO>> PostFamily(FamilyRequestDTO dto) =>
        await this.serviceProvider.GetRequiredService<FamilyService>()
            .PostFamily(dto);

    [PaginatedRequest("Id da ultima família", PaginationType.Cursor, typeof(Guid))]
    [HttpGet("List")]
    [MapToApiVersion("1.0")]
    public async Task<IResponse<IEnumerable<FamilyCardDTO>>> ListFamilies(string? searchTerm = null, bool global = false) =>
        await this.serviceProvider.GetRequiredService<FamilyService>()
                .ListFamilies(searchTerm, global);

    [PaginatedRequest("Id da ultima família", PaginationType.Cursor, typeof(Guid))]
    [HttpGet("GlobalList")]
    [MapToApiVersion("1.0")]
    public async Task<IResponse<IEnumerable<FamilyCardDTO>>> GlobalListFamilies(string searchTerm) =>
        await this.serviceProvider.GetRequiredService<FamilyService>()
                .GlobalListFamilies(searchTerm);

    [HttpGet("Details")]
    [MapToApiVersion("1.0")]
    public async Task<IResponse<FamilyDTO>> FamilyDetails(Guid familyId) =>
        await this.serviceProvider.GetRequiredService<FamilyService>()
            .FamilyDetails(familyId);
}

