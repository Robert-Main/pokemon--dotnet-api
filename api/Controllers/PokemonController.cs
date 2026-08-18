using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos;
using api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonInterface _pokemonService;
        public PokemonController(IPokemonInterface pokemonService)
        {
            _pokemonService = pokemonService;
        }

        [HttpGet]
        [ProducesResponseType(200,Type = typeof(IEnumerable<PokemonResponseDtos>))]
        public async Task<ActionResult<IEnumerable<PokemonResponseDtos>>> ListAllPokemons()
        {
            var pokemons = await _pokemonService.ListAllPokemons();
            return Ok(new { message = "Pokemons retrieved successfully", data = pokemons });
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(PokemonResponseDtos))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PokemonResponseDtos>> GetPokemonById(int id)
        {
            var pokemon = await _pokemonService.GetPokemonById(id);
            if (pokemon == null)
            {
                return NotFound(new { message = "Pokemon not found" });
            }
            return Ok(new { message = "Pokemon retrieved successfully", data = pokemon });
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(PokemonDtos))]
        [ProducesResponseType(400)]
        public async Task<ActionResult<PokemonDtos>> CreatePokemon([FromBody] PokemonDtos pokemon)
        {
            var createdPokemon = await _pokemonService.CreatePokemon(pokemon);
            if (createdPokemon == null)
            {
                return BadRequest(new { message = "Failed to create pokemon" });
            }
            return CreatedAtAction(nameof(GetPokemonById), new { id = createdPokemon.Id }, new { message = "Pokemon created successfully", data = createdPokemon });
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200, Type = typeof(PokemonDtos))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PokemonDtos>> UpdatePokemon(int id, [FromBody] PokemonDtos pokemon)
        {
            var updatedPokemon = await _pokemonService.UpdatePokemon(id, pokemon);
            if (updatedPokemon == null)
            {
                return NotFound(new { message = "Pokemon not found" });
            }
            return Ok(new { message = "Pokemon updated successfully", data = updatedPokemon });
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<bool>> DeletePokemon(int id)
        {
            var result = await _pokemonService.DeletePokemon(id);
            if (!result.HasValue)
            {
                return NotFound(new { message = "Pokemon not found" });
            }
            return Ok(new { message = "Pokemon deleted successfully" });
        }
    }
}