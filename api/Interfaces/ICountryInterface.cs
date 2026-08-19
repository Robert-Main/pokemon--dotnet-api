using api.Dtos;

namespace api.Interfaces;

public interface ICountryInterface
{
    Task<IEnumerable<Country>> ListAllCountries();
    Task<Country?> GetCountryById(int id);
    Task<Country> CreateCountry(Country country);
    Task<Country?> UpdateCountry(int id, Country country);
    Task<bool?> DeleteCountry(int id);
}
