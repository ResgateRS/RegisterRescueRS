using Microsoft.AspNetCore.Mvc;
using RegisterRescueRS.Domain.Application.Services;
using RegisterRescueRS.DTOs;
using RegisterRescueRS.Presenter.Controllers.App.V1.DTOs;

namespace RegisterRescueRS.Presenter.Controllers.App.V1;

[ApiController]
[Route("api/v{version:apiVersion}/Login")]
[ApiVersion("1.0")]
public class ShelterController(IServiceProvider serviceProvider) : BaseController(serviceProvider)
{
    [HttpPost("CreateShelter")]
    [MapToApiVersion("1.0")]
    public async Task<IResponse<ResponseDTO>> CreateShelter(CreateShelterDTO dto) =>
        await this.serviceProvider.GetRequiredService<ShelterService>()
            .CreateShelter(dto);
}

