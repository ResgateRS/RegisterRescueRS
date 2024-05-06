using Microsoft.AspNetCore.Mvc;
using RegisterRescueRS.Domain.Application.Services;
using RegisterRescueRS.Presenter.Controllers.App.V1.DTOs;

namespace RegisterRescueRS.Presenter.Controllers.App.V1;

[ApiController]
[Route("api/v{version:apiVersion}/Login")]
[ApiVersion("1.0")]
public class LoginController(IServiceProvider serviceProvider) : BaseController(serviceProvider)
{
    [SkipAuthenticationFilter]
    [HttpPost]
    [MapToApiVersion("1.0")]
    public async Task<ActionResult<LoginResponseDTO>> Login(LoginRequestDTO dto) =>
        await this.serviceProvider.GetRequiredService<LoginService>()
            .handle(dto);

    [HttpPost("CreateShelter")]
    [MapToApiVersion("1.0")]
    public async Task<ActionResult<LoginResponseDTO>> CreateShelter(CreateShelterDTO dto) =>
        await this.serviceProvider.GetRequiredService<LoginService>()
            .CreateShelter(dto);
}

