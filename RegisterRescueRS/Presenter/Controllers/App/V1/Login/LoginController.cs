using Microsoft.AspNetCore.Mvc;
using RegisterRescueRS.Attributes;
using RegisterRescueRS.Domain.Application.Services;
using RegisterRescueRS.DTOs;
using RegisterRescueRS.Presenter.Controllers.App.V1.DTOs;

namespace RegisterRescueRS.Presenter.Controllers.App.V1;

[ApiController]
[Route("api/v{version:apiVersion}/Login")]
[ApiVersion("1.0")]
public class LoginController(IServiceProvider serviceProvider) : BaseController(serviceProvider)
{
    [SkipAuthentication]
    [HttpPost]
    [MapToApiVersion("1.0")]
    public async Task<IResponse<LoginResponseDTO>> Login(LoginRequestDTO dto) =>
        await this.serviceProvider.GetRequiredService<LoginService>()
            .handle(dto);

    [HttpPost("Validate")]
    [MapToApiVersion("1.0")]
    public IResponse<ResponseDTO> Validate(ValidateRequestDTO dto) =>
        this.serviceProvider.GetRequiredService<LoginService>()
            .Validate(dto);

}

