using Microsoft.AspNetCore.Mvc;
using RescueRS.Presenter.Controllers.App.V1.Enums;
using RegisterRescueRS.Domain.Application.Entities;
using RegisterRescueRS.Domain.Application.Services.Interfaces;
using RegisterRescueRS.Infrastructure.Repositories;
using RegisterRescueRS.Presenter.Controllers.App.V1.DTOs;
using RegisterRescueRS.Tools;
using System.Transactions;

namespace RegisterRescueRS.Domain.Application.Services;

public class FamilyService(FamilyRepository mainRepository, ShelterRepository shelterRepository, HousedRepository housedRepository) : BaseService<FamilyRepository>(mainRepository), IService
{
    private ShelterRepository shelterRepository = shelterRepository;
    private HousedRepository _housedRepository = housedRepository;

    public async Task<ActionResult<IEnumerable<FamilyCardDTO>>> ListFamilies(int page, int size, string searchTerm, string authToken)
    {
        if (page < 1)
            return new BadRequestObjectResult(new ResponseDTO
            {
                DebugMessage = "Page must be greater than 0",
                Message = "Um erro aconteceu, tente novamente!"
            });

        if (size < 1)
            return new BadRequestObjectResult(new ResponseDTO
            {
                Message = "Um erro aconteceu, tente novamente!",
                DebugMessage = "Size must be greater than 0"
            });

        if (!Guid.TryParse(JwtManager.ExtractPayload<string>(authToken.Split(" ")[1], LoginClaimsEnum.ShelterId), out Guid shelterId))
            return new BadRequestObjectResult(new ResponseDTO
            {
                Message = "Um erro aconteceu, tente novamente!",
                DebugMessage = "Invalid Token"
            });

        var families = await this._mainRepository.ListFamilies(page, size, searchTerm, shelterId);

        return new OkObjectResult(families.Select(x => new FamilyCardDTO
        {
            FamilyId = x.FamilyId,
            Responsable = x.Responsable.Name,
            TotalPeopleNumber = x.Houseds.Count()
        }));
    }

    public async Task<ActionResult<ResponseDTO>> PostFamily(FamilyRequestDTO dto, string authToken)
    {
        if (!Guid.TryParse(JwtManager.ExtractPayload<string>(authToken.Split(" ")[1], LoginClaimsEnum.ShelterId), out Guid shelterId))
            return new BadRequestObjectResult(new ResponseDTO
            {
                Message = "Um erro aconteceu, tente novamente!",
                DebugMessage = "Invalid Token"
            });

        var shelter = await this.shelterRepository.GetShelterById(shelterId);
        if (shelter == null)
            return new BadRequestObjectResult(new ResponseDTO
            {
                Message = "Abrigo não encontrado",
                DebugMessage = "Shelter not found"
            });

        if (!dto.Houseds.Any())
            return new BadRequestObjectResult(new ResponseDTO
            {
                Message = "Nenhum abrigado informado",
                DebugMessage = "No housed informed"
            });

        if (!dto.Houseds.Any(x => x.Responsable))
            return new BadRequestObjectResult(new ResponseDTO
            {
                Message = "É obrigatório haver um responsável na família",
                DebugMessage = "No responsable housed informed"
            });

        if (dto.Houseds.Where(x => x.Responsable).Count() > 1)
            return new BadRequestObjectResult(new ResponseDTO
            {
                Message = "Só pode haver um responsável na família"
            });

        HousedEntity responsable = dto.Houseds
            .Where(x => x.Responsable)
            .Select(x => new HousedEntity()
            {
                Name = x.Name,
                Age = x.Age,
                IsFamilyResponsable = x.Responsable,
                RegisteredAt = DateTimeOffset.Now,
                UpdatedAt = DateTimeOffset.Now,
                HousedId = Guid.NewGuid(),
            })
            .First();

        FamilyEntity family = new()
        {
            ShelterId = shelterId,
            ResponsableId = responsable.HousedId,
            RegisteredAt = DateTimeOffset.Now,
            UpdatedAt = DateTimeOffset.Now,
        };

        using (TransactionScope ts = new(TransactionScopeAsyncFlowOption.Enabled))
        {
            family = await this._mainRepository.InsertOrUpdate(family);

            var houseds = dto.Houseds
                .Where(x => !x.Responsable)
                .Select(x => new HousedEntity
                {
                    Name = x.Name,
                    Age = x.Age,
                    IsFamilyResponsable = x.Responsable,
                    FamilyId = family.FamilyId,
                    RegisteredAt = DateTimeOffset.Now,
                    UpdatedAt = DateTimeOffset.Now
                });

            var temp = houseds.ToList();
            temp.Add(responsable);
            houseds = temp;

            await this._housedRepository.InsertRange(houseds);

            ts.Complete();
        }

        return new OkObjectResult(new ResponseDTO
        {
            Message = "Família cadastrada com sucesso",
            DebugMessage = "Family registered successfully"
        });
    }

    internal async Task<ActionResult<FamilyDTO>> FamilyDetails(Guid familyId)
    {
        if (Guid.Empty == familyId)
            return new BadRequestObjectResult(new ResponseDTO
            {
                Message = "Família não encontrada",
                DebugMessage = "Family not found"
            });

        var family = await this._mainRepository.GetFamilyById(familyId);

        if (family == null)
            return new BadRequestObjectResult(new ResponseDTO
            {
                Message = "Família não encontrada",
                DebugMessage = "Family not found"
            });

        return new OkObjectResult(FamilyDTO.FromEntity(family));
    }
}