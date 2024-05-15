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
    public async Task<IResponse<IEnumerable<FamilyCardDTO>>> ListFamilies(string? searchTerm, bool global)
    {
        IEnumerable<FamilyEntity> families = await this._serviceProvider.GetRequiredService<FamilyRepository>()
            .ListFamilies(searchTerm, global ? null : _userSession.ShelterId);

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

        FamilyEntity? familyEntity = null;

        if (dto.FamilyId != null || dto.FamilyId == Guid.Empty)
        {
            familyEntity = await this._serviceProvider.GetRequiredService<FamilyRepository>()
                .GetFamilyById(dto.FamilyId.Value) ??
                    throw new Exception("Família não encontrada");
            if (familyEntity.ShelterId != _userSession.ShelterId)
                throw new Exception("Família não pertence ao abrigo");
        }

        familyEntity ??= new()
        {
            FamilyId = Guid.Empty,
            ShelterId = _userSession.ShelterId,
            RegisteredAt = DateTimeOffset.Now,
            Active = true
        };
        familyEntity.UpdatedAt = DateTimeOffset.Now;

        using (TransactionScope ts = new(TransactionScopeAsyncFlowOption.Enabled))
        {
            familyEntity = await this._serviceProvider.GetRequiredService<FamilyRepository>()
                .InsertOrUpdate(familyEntity);

            var houseds = dto.Houseds
                .Select(x => new HousedEntity
                {
                    Name = x.Name,
                    Cellphone = x.Cellphone,
                    Age = x.Age,
                    IsFamilyResponsable = x.Responsable,
                    FamilyId = familyEntity.FamilyId,
                    RegisteredAt = DateTimeOffset.Now,
                    UpdatedAt = DateTimeOffset.Now,
                    HousedId = Guid.NewGuid(),
                    Active = true
                });

            await this._serviceProvider.GetRequiredService<HousedRepository>()
                .UpsertRange(houseds);

            ts.Complete();
        }

        return Response<ResponseDTO>.Success(new ResponseDTO { Message = $"Família {(dto.FamilyId == null ? "cadastrada" : "atualizada")} com sucesso" });
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

    public async Task<IResponse<IEnumerable<FamilyCardDTO>>> GlobalListFamilies(string? searchTerm)
    {
        var families = await this._serviceProvider.GetRequiredService<FamilyRepository>()
            .ListFamilies(searchTerm);

        return Response<IEnumerable<FamilyCardDTO>>.Success(families.Select(FamilyCardDTO.FromEntity));
    }

    public async Task<IResponse<ResponseDTO>> DeleteFamily(Guid familyId)
    {
        if (Guid.Empty == familyId)
            throw new Exception("Família não encontrada");

        var family = await this._serviceProvider.GetRequiredService<FamilyRepository>()
            .GetFamilyById(familyId) ??
            throw new Exception("Família não encontrada");

        if (family.ShelterId != _userSession.ShelterId)
            throw new Exception("Família não pertence ao seu abrigo");

        using (TransactionScope ts = new(TransactionScopeAsyncFlowOption.Enabled))
        {
            family.Active = false;
            family.UpdatedAt = DateTimeOffset.Now;

            await this._serviceProvider.GetRequiredService<FamilyRepository>()
                .InsertOrUpdate(family);

            await this._serviceProvider.GetRequiredService<HousedRepository>()
                .UpsertRange([], familyId);

            ts.Complete();
        }

        return Response<ResponseDTO>.Success(new ResponseDTO { Message = "Família desativada com sucesso" });
    }
}