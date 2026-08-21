using api.Data;
using api.Dtos;
using api.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OwnerModel = api.models.Owner;

namespace api.Repositories;

public class OwnerRepository(DataContext context, IMapper mapper) : IOwnerInterface
{
    public async Task<IEnumerable<Owner>> ListAllOwners() =>
        mapper.Map<IEnumerable<Owner>>(await context.Owners.AsNoTracking().Include(owner => owner.Country).ToListAsync());

    public async Task<Owner?> GetOwnerById(int id) =>
        mapper.Map<Owner>(await context.Owners.AsNoTracking().Include(owner => owner.Country).FirstOrDefaultAsync(owner => owner.Id == id));

    public async Task<Owner?> CreateOwner(OwnerCreateDtos owner)
    {
        if (owner.CountryId is not null && !await context.Countries.AnyAsync(country => country.Id == owner.CountryId)) return null;
        var entity = mapper.Map<OwnerModel>(owner);
        context.Owners.Add(entity);
        await context.SaveChangesAsync();
        await context.Entry(entity).Reference(owner => owner.Country).LoadAsync();
        return mapper.Map<Owner>(entity);
    }

    public async Task<Owner?> UpdateOwner(int id, Owner owner)
    {
        if (owner.CountryId is not null && !await context.Countries.AnyAsync(country => country.Id == owner.CountryId)) return null;
        var entity = await context.Owners.FindAsync(id);
        if (entity is null) return null;
        mapper.Map(owner, entity);
        await context.SaveChangesAsync();
        await context.Entry(entity).Reference(owner => owner.Country).LoadAsync();
        return mapper.Map<Owner>(entity);
    }

    public async Task<bool?> DeleteOwner(int id)
    {
        var entity = await context.Owners.FindAsync(id);
        if (entity is null) return null;
        context.Owners.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }
}
