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
        if (string.IsNullOrEmpty(dto.ShelterCellphone))
            throw new Exception("Telefone é necessário");

        ShelterEntity entity = new()
        {
            Login = dto.Login,
            Password = GetMd5Hash(dto.Password),
            ShelterName = dto.Name,
            Address = dto.Address,
            Adm = false,
            Latitude = dto.Latitude ?? 0,
            Longitude = dto.Longitude ?? 0,
            Verified = false,
            ShelterCellphone = dto.ShelterCellphone
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
                DonationDescription = dto.DonationsDescription,
                Avaliable = dto.Avaliable,
                VolunteersSubscriptionLink = dto.VolunteersSubscriptionLink,
                UpdatedAt = DateTimeOffset.Now
            });

        return Response<ResponseDTO>.Success(new ResponseDTO
        {
            Message = "Needs updated successfully!"
        });
    }

    public async Task<IResponse<IEnumerable<VolunteerDTO>>> ListVolunteers(double? latitude, double? longitude, string? searchTerm)
    {
        var entities = await this._serviceProvider.GetRequiredService<ShelterNeedsRepository>()
            .ListVolunteers(latitude, longitude, searchTerm);

        return Response<IEnumerable<VolunteerDTO>>.Success(entities.Select(VolunteerDTO.FromEntity));
    }

    public async Task<IResponse<IEnumerable<DonationDTO>>> ListDonations(double? latitude, double? longitude, string? searchTerm)
    {
        var entities = await this._serviceProvider.GetRequiredService<ShelterNeedsRepository>()
            .ListDonations(latitude, longitude, searchTerm);

        return Response<IEnumerable<DonationDTO>>.Success(entities.Select(DonationDTO.FromEntity));
    }

    public async Task<IResponse<ShelterNeedsDTO>> GetNeeds()
    {
        var entity = await this._serviceProvider.GetRequiredService<ShelterNeedsRepository>()
            .GetShelterNeeds(_userSession.ShelterId);

        return Response<ShelterNeedsDTO>.Success(entity == null ? new ShelterNeedsDTO() : ShelterNeedsDTO.FromEntity(entity));
    }

    public async Task<IResponse<IEnumerable<ShelterDTO>>> GetUnverifieds()
    {
        if (!_userSession.Adm)
            throw new Exception("Acesso negado");

        var entities = await this._serviceProvider.GetRequiredService<ShelterRepository>()
            .GetUnverifieds();

        return Response<IEnumerable<ShelterDTO>>.Success(entities.Select(ShelterDTO.FromEntity));
    }

    public async Task<IResponse<ResponseDTO>> VerifyShelter(VerifyShelterDTO dto)
    {
        if (!_userSession.Adm)
            throw new Exception("Acesso negado");

        var shelter = await this._serviceProvider.GetRequiredService<ShelterRepository>()
            .GetShelterById(dto.ShelterId) ??
            throw new Exception("Abrigo não encontrado");

        shelter.Verified = true;
        shelter.Latitude = dto.Latitude;
        shelter.Longitude = dto.Longitude;

        await this._serviceProvider.GetRequiredService<ShelterRepository>()
            .InsertOrUpdate(shelter);

        return Response<ResponseDTO>.Success(new ResponseDTO { Message = "Abrigo verificado!" });
    }
}