using api.Dtos;
using api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CountryController(ICountryInterface countryService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Country>>> ListAllCountries() => Ok(new { message = "Countries retrieved successfully", data = await countryService.ListAllCountries() });

    [HttpGet("{id}")]
    public async Task<ActionResult<Country>> GetCountryById(int id) =>
        await countryService.GetCountryById(id) is { } country ? Ok(new { message = "Country retrieved successfully", data = country }) : NotFound(new { message = "Country not found" });

    [HttpPost]
    public async Task<ActionResult<Country>> CreateCountry(CountryCreateDtos country)
    {
        var created = await countryService.CreateCountry(country);
        return CreatedAtAction(nameof(GetCountryById), new { id = created.Id }, new { message = "Country created successfully", data = created });
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Country>> UpdateCountry(int id, Country country) =>
        await countryService.UpdateCountry(id, country) is { } updated ? Ok(new { message = "Country updated successfully", data = updated }) : NotFound(new { message = "Country not found" });

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCountry(int id) =>
        await countryService.DeleteCountry(id) is not null ? Ok(new { message = "Country deleted successfully" }) : NotFound(new { message = "Country not found" });
}
