using Microsoft.AspNetCore.Mvc;
using RescueRS.Presenter.Controllers.App.V1.Enums;
using RegisterRescueRS.Domain.Application.Entities;
using RegisterRescueRS.Domain.Application.Services.Interfaces;
using RegisterRescueRS.Infrastructure.Repositories;
using RegisterRescueRS.Presenter.Controllers.App.V1.DTOs;
using RegisterRescueRS.Tools;
using System.Security.Cryptography;
using System.Text;

namespace RegisterRescueRS.Domain.Application.Services;

public class LoginService(IServiceProvider serviceProvider) : BaseService(serviceProvider), IService
{
    public async Task<ActionResult<LoginResponseDTO>> handle(LoginRequestDTO dto)
    {
        if (string.IsNullOrEmpty(dto.Login))
            return new BadRequestObjectResult(new ResponseDTO
            {
                DebugMessage = "Login é necessário",
                Message = "An error occurred, try again!"
            });

        if (string.IsNullOrEmpty(dto.Password))
            return new BadRequestObjectResult(new ResponseDTO
            {
                DebugMessage = "Senha é necessária",
                Message = "An error occurred, try again!"
            });

        var shelter = await this._serviceProvider.GetRequiredService<ShelterRepository>()
            .GetShelter(dto.Login, GetMd5Hash(dto.Password));

        if (shelter == null)
            return new BadRequestObjectResult(new ResponseDTO
            {
                DebugMessage = "Usuário ou senha inválidos",
                Message = "An error occurred, try again!"
            });

        //TODO JWT
        Dictionary<string, object> claims = new()
        {
            { LoginClaimsEnum.ShelterId,  shelter.ShelterId.ToString()},
        };

        return new OkObjectResult(new LoginResponseDTO
        {
            Token = JwtManager.GenerateToken(claims)
        });
    }

    internal static string GetMd5Hash(string input)
    {
        using (MD5 md5Hash = MD5.Create())
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            StringBuilder sBuilder = new();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }
    }

    public async Task<ActionResult> CreateShelter(CreateShelterDTO dto)
    {
        if (string.IsNullOrEmpty(dto.Login))
            return new BadRequestObjectResult(new ResponseDTO
            {
                DebugMessage = "Login é necessário",
                Message = "An error occurred, try again!"
            });
        if (string.IsNullOrEmpty(dto.Password))
            return new BadRequestObjectResult(new ResponseDTO
            {
                DebugMessage = "Senha é necessário",
                Message = "An error occurred, try again!"
            });
        if (string.IsNullOrEmpty(dto.Name))
            return new BadRequestObjectResult(new ResponseDTO
            {
                DebugMessage = "Nome é necessário",
                Message = "An error occurred, try again!"
            });

        ShelterEntity entity = new()
        {
            Login = dto.Login,
            Password = GetMd5Hash(dto.Password),
            ShelterName = dto.Name
        };

        await this._serviceProvider.GetRequiredService<ShelterRepository>()
            .InsertOrUpdate(entity);

        return new OkObjectResult(new ResponseDTO
        {
            Message = "Shelter created successfully!"
        });
    }
}