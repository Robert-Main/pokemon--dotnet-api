using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos;

namespace api.Interfaces
{
    public interface IPokemonInterface
    {
        Task<IEnumerable<PokemonResponseDtos>> ListAllPokemons();
        Task<PokemonResponseDtos?> GetPokemonById(int id);
        Task<PokemonDtos?> CreatePokemon(PokemonCreateDtos pokemon);
        Task<PokemonDtos?> UpdatePokemon(int id, PokemonDtos pokemon);
        Task<bool?> DeletePokemon(int id);
    }
}