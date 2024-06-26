using System.Text;
using System.Text.Json;
using RegisterRescueRS.DTOs;
using RegisterRescueRS.Tools;
using RegisterRescueRS.Auth;
using Microsoft.AspNetCore.Http.Features;
using RegisterRescueRS.Presenter.Controllers;
using Microsoft.AspNetCore.Mvc;
using RegisterRescueRS.Attributes;

namespace RegisterRescueRS.Middleware;

public class AuthenticationMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context, UserSession _userSession, IServiceProvider serviceProvider)
    {

        string hashLogin = context.Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "") ?? "";

        EndpointMetadataCollection? metadata = context.Features.Get<IEndpointFeature>()?.Endpoint?.Metadata;

        if (metadata != null)
        {
            bool skipAuthentication = metadata.GetMetadata<SkipAuthenticationAttribute>() != null;

            if (!skipAuthentication)
            {
                if (!JwtManager.IsValidToken(hashLogin, out UserSession? userSession))
                    throw new MessageException("Login expirado.", ResultType.ErrorLogin);

                _userSession.ShelterId = userSession!.ShelterId;
                _userSession.ShelterName = userSession!.ShelterName;
                _userSession.Adm = userSession!.Adm;
            }
        }

        await _next(context);

    }

}
