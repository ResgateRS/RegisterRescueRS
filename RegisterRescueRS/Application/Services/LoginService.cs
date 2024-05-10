using RegisterRescueRS.Auth;
using RegisterRescueRS.Domain.Application.Entities;
using RegisterRescueRS.Domain.Application.Services.Interfaces;
using RegisterRescueRS.DTOs;
using RegisterRescueRS.Infrastructure.Repositories;
using RegisterRescueRS.Presenter.Controllers.App.V1.DTOs;
using RegisterRescueRS.Tools;
using System.Security.Cryptography;
using System.Text;

namespace RegisterRescueRS.Domain.Application.Services;

public class LoginService(IServiceProvider serviceProvider, UserSession userSession) : BaseService(serviceProvider, userSession), IService
{
    public async Task<IResponse<LoginResponseDTO>> handle(LoginRequestDTO dto)
    {
        if (string.IsNullOrEmpty(dto.Login))
            throw new Exception("Login é necessário");

        if (string.IsNullOrEmpty(dto.Password))
            throw new Exception("Senha é necessária");

        var shelter = await this._serviceProvider.GetRequiredService<ShelterRepository>()
            .GetShelter(dto.Login, GetMd5Hash(dto.Password)) ??
                throw new Exception("Usuário ou senha inválidos");

        _userSession.ShelterId = shelter.ShelterId;
        _userSession.Adm = shelter.Adm;

        var jwt = _serviceProvider.GetRequiredService<JwtTool>();
        jwt.setUserData(userSession);
        string token = jwt.generateToken();

        return Response<LoginResponseDTO>.Success(new LoginResponseDTO { Token = token });
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

    public IResponse<ResponseDTO> Validate(ValidateRequestDTO dto) =>
        JwtManager.IsValidToken(dto.Token) ?
            Response<ResponseDTO>.Success(new ResponseDTO { Message = "Validated" }) :
            Response<ResponseDTO>.Fail("Invalid token");
}