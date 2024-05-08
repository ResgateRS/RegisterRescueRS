using Microsoft.AspNetCore.Mvc;
using RegisterRescueRS.Attributes;
using RegisterRescueRS.Domain.Application.Services;
using RegisterRescueRS.DTOs;
using RegisterRescueRS.Presenter.Controllers.App.V1.DTOs;

namespace RegisterRescueRS.Presenter.Controllers.App.V1;

[ApiController]
[Route("api/v{version:apiVersion}/Houseds")]
[ApiVersion("1.0")]
public class HousedsController(IServiceProvider serviceProvider) : BaseController(serviceProvider)
{
    [SkipAuthentication]
    [PaginatedRequest("Id do ultimo abrigado", PaginationType.Cursor, typeof(Guid))]
    [HttpGet]
    [MapToApiVersion("1.0")]
    public async Task<IResponse<IEnumerable<HousedCardDTO>>> GetHouseds(string searchTerm) =>
        await this.serviceProvider.GetRequiredService<HousedsService>()
            .handle(searchTerm);
}