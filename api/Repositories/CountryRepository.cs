using api.Data;
using api.Dtos;
using api.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using CountryModel = api.models.Country;

namespace api.Repositories;

public class CountryRepository(DataContext context, IMapper mapper) : ICountryInterface
{
    public async Task<IEnumerable<Country>> ListAllCountries() =>
        mapper.Map<IEnumerable<Country>>(await context.Countries.AsNoTracking().ToListAsync());

    public async Task<Country?> GetCountryById(int id) =>
        mapper.Map<Country>(await context.Countries.AsNoTracking().FirstOrDefaultAsync(country => country.Id == id));

    public async Task<Country> CreateCountry(Country country)
    {
        var entity = mapper.Map<CountryModel>(country);
        context.Countries.Add(entity);
        await context.SaveChangesAsync();
        return mapper.Map<Country>(entity);
    }

    public async Task<Country?> UpdateCountry(int id, Country country)
    {
        var entity = await context.Countries.FindAsync(id);
        if (entity is null) return null;
        mapper.Map(country, entity);
        await context.SaveChangesAsync();
        return mapper.Map<Country>(entity);
    }

    public async Task<bool?> DeleteCountry(int id)
    {
        var entity = await context.Countries.FindAsync(id);
        if (entity is null) return null;
        context.Countries.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }
}
