using RegisterRescueRS.Domain.Application.Services.Interfaces;
using RegisterRescueRS.Infrastructure.Repositories;
using RegisterRescueRS.Presenter.Controllers.App.V1.DTOs;
using RegisterRescueRS.Auth;
using RegisterRescueRS.DTOs;

namespace RegisterRescueRS.Domain.Application.Services;

public class HousedsService(IServiceProvider serviceProvider, UserSession userSession) : BaseService(serviceProvider, userSession), IService
{
    public async Task<IResponse<IEnumerable<HousedCardDTO>>> handle(string? searchTerm)
    {
        var houseds = await this._serviceProvider.GetRequiredService<HousedRepository>()
            .GetHouseds(searchTerm);

        return Response<IEnumerable<HousedCardDTO>>.Success(houseds.Select(HousedCardDTO.FromEntity));
    }
}