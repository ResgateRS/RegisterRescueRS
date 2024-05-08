using RegisterRescueRS.Auth;
using RegisterRescueRS.Domain.Application.Entities;
using RegisterRescueRS.Domain.Application.Services.Interfaces;
using RegisterRescueRS.DTOs;
using RegisterRescueRS.Infrastructure.Repositories;
using RegisterRescueRS.Presenter.Controllers.App.V1.DTOs;
using System.Security.Cryptography;
using System.Text;

namespace RegisterRescueRS.Domain.Application.Services;

public class ShelterService(IServiceProvider serviceProvider, UserSession userSession) : BaseService(serviceProvider, userSession), IService
{
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

    public async Task<IResponse<ResponseDTO>> CreateShelter(CreateShelterDTO dto)
    {
        if (string.IsNullOrEmpty(dto.Login))
            throw new Exception("Login é necessário");
        if (string.IsNullOrEmpty(dto.Password))
            throw new Exception("Senha é necessário");
        if (string.IsNullOrEmpty(dto.Name))
            throw new Exception("Nome é necessário");
        if (string.IsNullOrEmpty(dto.Address))
            throw new Exception("Endereço é necessário");

        // if (!_userSession.Adm)
        //     throw new Exception("Acesso negado");

        ShelterEntity entity = new()
        {
            Login = dto.Login,
            Password = GetMd5Hash(dto.Password),
            ShelterName = dto.Name,
            Address = dto.Address,
            Adm = false
        };

        await this._serviceProvider.GetRequiredService<ShelterRepository>()
            .InsertOrUpdate(entity);

        return Response<ResponseDTO>.Success(new ResponseDTO
        {
            Message = "Shelter created successfully!"
        });
    }

    public async Task<IResponse<ResponseDTO>> UpsertNeeds(ShelterNeedsDTO dto)
    {
        _ = await this._serviceProvider.GetRequiredService<ShelterRepository>()
            .GetShelterById(_userSession.ShelterId) ??
            throw new Exception("Abrigo não encontrado");

        await this._serviceProvider.GetRequiredService<ShelterNeedsRepository>()
            .InsertOrUpdate(new ShelterNeedsEntity
            {
                ShelterId = _userSession.ShelterId,
                AcceptingVolunteers = dto.AcceptingVolunteers,
                AcceptingDoctors = dto.AcceptingDoctors,
                AcceptingVeterinarians = dto.AcceptingVeterinarians,
                AcceptingDonations = dto.AcceptingDonations,
                DonationDescription = dto.DonationDescription,
                Avaliable = dto.Avaliable,
                VolunteersSubscriptionLink = dto.VolunteersSubscriptionLink,
                UpdatedAt = DateTimeOffset.Now
            });

        return Response<ResponseDTO>.Success(new ResponseDTO
        {
            Message = "Needs updated successfully!"
        });
    }

    public async Task<IResponse<IEnumerable<VolunteerDTO>>> ListVolunteers()
    {
        var entities = await this._serviceProvider.GetRequiredService<ShelterNeedsRepository>()
            .ListVolunteers();

        return Response<IEnumerable<VolunteerDTO>>.Success(entities.Select(VolunteerDTO.FromEntity));
    }

    public async Task<IResponse<IEnumerable<DonationDTO>>> ListDonations()
    {
        var entities = await this._serviceProvider.GetRequiredService<ShelterNeedsRepository>()
            .ListDonations();

        return Response<IEnumerable<DonationDTO>>.Success(entities.Select(DonationDTO.FromEntity));
    }
}