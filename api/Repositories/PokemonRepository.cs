using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using api.Data;
using api.Dtos;
using api.Interfaces;
using api.models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class PokemonRepository : IPokemonInterface
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public PokemonRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool?> DeletePokemon(int id)
        {
            var pokemon = await _context.Pokemons.FirstOrDefaultAsync(p => p.Id == id);
            if (pokemon == null)
            {
                return null;
            }

            _context.Pokemons.Remove(pokemon);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<PokemonResponseDtos?> GetPokemonById(int id)
        {
            var pokemon = await _context.Pokemons
                .Include(p => p.Reviews)
                .ThenInclude(r => r.Reviewer)
                .Include(p => p.PokemonCategories)
                .ThenInclude(pc => pc.Category)
                .Include(p => p.PokemonOwners)
                .ThenInclude(po => po.Owner)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (pokemon == null)
            {
                return null;
            }
            return _mapper.Map<PokemonResponseDtos>(pokemon);
        }

        public async Task<IEnumerable<PokemonResponseDtos>> ListAllPokemons()
        {
            var pokemons = await _context.Pokemons
                .Include(p => p.Reviews)
                .ThenInclude(r => r.Reviewer)
                .Include(p => p.PokemonCategories)
                .ThenInclude(pc => pc.Category)
                .Include(p => p.PokemonOwners)
                .ThenInclude(po => po.Owner)
                .ToListAsync();

            return _mapper.Map<IEnumerable<PokemonResponseDtos>>(pokemons);
        }

        public async Task<PokemonDtos?> UpdatePokemon(int id, PokemonDtos pokemon)
        {
            var existingPokemon = await _context.Pokemons.FirstOrDefaultAsync(p => p.Id == id);
            if (existingPokemon == null)
            {
                return null;
            }

            _mapper.Map(pokemon, existingPokemon);

            await _context.SaveChangesAsync();

            return _mapper.Map<PokemonDtos>(existingPokemon);
        }

        public async Task<PokemonDtos?> CreatePokemon(PokemonCreateDtos pokemon)
        {
            var newPokemon = _mapper.Map<Pokemon>(pokemon);
            newPokemon.CreatedAt = DateTime.UtcNow;
            _context.Pokemons.Add(newPokemon);
            await _context.SaveChangesAsync();
            return _mapper.Map<PokemonDtos>(newPokemon);
        }
    }
}