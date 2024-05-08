using Microsoft.AspNetCore.Mvc;
using RescueRS.Presenter.Controllers.App.V1.Enums;
using RegisterRescueRS.Domain.Application.Entities;
using RegisterRescueRS.Domain.Application.Services.Interfaces;
using RegisterRescueRS.Infrastructure.Repositories;
using RegisterRescueRS.Presenter.Controllers.App.V1.DTOs;
using RegisterRescueRS.Tools;
using System.Transactions;
using RegisterRescueRS.Auth;
using RegisterRescueRS.DTOs;

namespace RegisterRescueRS.Domain.Application.Services;

public class FamilyService(IServiceProvider serviceProvider, UserSession userSession) : BaseService(serviceProvider, userSession), IService
{
    public async Task<IResponse<IEnumerable<FamilyCardDTO>>> ListFamilies(string? searchTerm)
    {
        var families = await this._serviceProvider.GetRequiredService<FamilyRepository>()
            .ListFamilies(searchTerm, _userSession.ShelterId);

        return Response<IEnumerable<FamilyCardDTO>>.Success(families.Select(FamilyCardDTO.FromEntity));
    }

    public async Task<IResponse<ResponseDTO>> PostFamily(FamilyRequestDTO dto)
    {
        if (!dto.Houseds.Any())
            throw new Exception("Nenhum abrigado informado");

        if (!dto.Houseds.Any(x => x.Responsable))
            throw new Exception("É obrigatório haver um responsável na família");

        if (dto.Houseds.Where(x => x.Responsable).Count() > 1)
            throw new Exception("Só pode haver um responsável na família");

        if (dto.Houseds.Any(x => x.Age < 0))
            throw new Exception("Não pode haver uma idade negativa");

        var shelter = await this._serviceProvider.GetRequiredService<ShelterRepository>()
            .GetShelterById(_userSession.ShelterId) ??
        throw new Exception("Abrigo não encontrado");

        FamilyEntity family = new()
        {
            FamilyId = dto.FamilyId ?? Guid.Empty,
            ShelterId = _userSession.ShelterId,
            RegisteredAt = DateTimeOffset.Now,
            UpdatedAt = DateTimeOffset.Now,
        };

        using (TransactionScope ts = new(TransactionScopeAsyncFlowOption.Enabled))
        {
            family = await this._serviceProvider.GetRequiredService<FamilyRepository>()
                .InsertOrUpdate(family);

            var houseds = dto.Houseds
                .Select(x => new HousedEntity
                {
                    Name = x.Name,
                    Cellphone = x.Cellphone,
                    Age = x.Age,
                    IsFamilyResponsable = x.Responsable,
                    FamilyId = family.FamilyId,
                    RegisteredAt = DateTimeOffset.Now,
                    UpdatedAt = DateTimeOffset.Now,
                    HousedId = Guid.NewGuid()
                });

            await this._serviceProvider.GetRequiredService<HousedRepository>()
                .UpsertRange(houseds);

            ts.Complete();
        }

        return Response<ResponseDTO>.Success(new ResponseDTO { Message = "Família cadastrada com sucesso" });
    }

    public async Task<IResponse<FamilyDTO>> FamilyDetails(Guid familyId)
    {
        if (Guid.Empty == familyId)
            throw new Exception("Família não encontrada");

        var family = await this._serviceProvider.GetRequiredService<FamilyRepository>()
            .GetFamilyById(familyId) ??
            throw new Exception("Família não encontrada");

        return Response<FamilyDTO>.Success(FamilyDTO.FromEntity(family));
    }

    public async Task<IResponse<IEnumerable<FamilyCardDTO>>> GlobalListFamilies(string searchTerm)
    {
        var families = await this._serviceProvider.GetRequiredService<FamilyRepository>()
            .ListFamilies(searchTerm);

        return Response<IEnumerable<FamilyCardDTO>>.Success(families.Select(FamilyCardDTO.FromEntity));
    }
}