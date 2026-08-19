using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos;
using api.Interfaces;
using api.models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class PokemonRepository : IPokemonInterface
    {
        private readonly DataContext _context;
        public PokemonRepository(DataContext context)
        {
            _context = context;
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
                .FirstOrDefaultAsync(p => p.Id == id);
            if (pokemon == null)
            {
                return null;
            }
            return new PokemonResponseDtos
            {
                Id = pokemon.Id,
                Name = pokemon.Name,
                BirthDate = pokemon.BirthDate,
                CreatedAt = pokemon.CreatedAt,
                Reviews = pokemon.Reviews?.Select(r => new ReviewResponseDtos
                {
                    Id = r.Id,
                    Title = r.Title,
                    Text = r.Text,
                    Rating = r.Rating,
                    Reviewer = r.Reviewer != null ? new api.Dtos.Reviewer
                    {
                        Id = r.Reviewer.Id,
                        FirstName = r.Reviewer.FirstName,
                        LastName = r.Reviewer.LastName
                    } : null
                }).ToList(),
                PokemonCategories = pokemon.PokemonCategories?.Select(pc => new PokemonCategoryResponseDtos
                {
                    CategoryId = pc.CategoryId,
                    Category = pc.Category != null ? new api.Dtos.Category
                    {
                        Id = pc.Category.Id,
                        Name = pc.Category.Name
                    } : null
                }).ToList()
            };
        }

        public async Task<IEnumerable<PokemonResponseDtos>> ListAllPokemons()
        {
            var pokemons = await _context.Pokemons
                .Include(p => p.Reviews)
                .ThenInclude(r => r.Reviewer)
                .Include(p => p.PokemonCategories)
                .ThenInclude(pc => pc.Category)
                .ToListAsync();

            return pokemons.Select(p => new PokemonResponseDtos
            {
                Id = p.Id,
                Name = p.Name,
                BirthDate = p.BirthDate,
                CreatedAt = p.CreatedAt,
                Reviews = p.Reviews?.Select(r => new ReviewResponseDtos
                {
                    Id = r.Id,
                    Title = r.Title,
                    Text = r.Text,
                    Rating = r.Rating,
                    Reviewer = r.Reviewer != null ? new api.Dtos.Reviewer
                    {
                        Id = r.Reviewer.Id,
                        FirstName = r.Reviewer.FirstName,
                        LastName = r.Reviewer.LastName
                    } : null
                }).ToList(),
                PokemonCategories = p.PokemonCategories?.Select(pc => new PokemonCategoryResponseDtos
                {
                    CategoryId = pc.CategoryId,
                    Category = pc.Category != null ? new api.Dtos.Category
                    {
                        Id = pc.Category.Id,
                        Name = pc.Category.Name
                    } : null
                }).ToList()
            }).ToList();
        }

        public async Task<PokemonDtos?> UpdatePokemon(int id, PokemonDtos pokemon)
        {
            var existingPokemon = await _context.Pokemons.FirstOrDefaultAsync(p => p.Id == id);
            if (existingPokemon == null)
            {
                return null;
            }

            existingPokemon.Name = pokemon.Name;
            existingPokemon.BirthDate = pokemon.BirthDate;
            existingPokemon.CreatedAt = pokemon.CreatedAt;

            await _context.SaveChangesAsync();

            return new PokemonDtos
            {
                Id = existingPokemon.Id,
                Name = existingPokemon.Name,
                BirthDate = existingPokemon.BirthDate,
                CreatedAt = existingPokemon.CreatedAt
            };
        }

        public async Task<PokemonDtos?> CreatePokemon(PokemonCreateDtos pokemon)
        {
            var newPokemon = new Pokemon
            {
                Name = pokemon.Name,
                BirthDate = pokemon.BirthDate,
                CreatedAt = DateTime.UtcNow
            };
            _context.Pokemons.Add(newPokemon);
            await _context.SaveChangesAsync();
            return new PokemonDtos
            {
                Id = newPokemon.Id,
                Name = newPokemon.Name,
                BirthDate = newPokemon.BirthDate,
                CreatedAt = newPokemon.CreatedAt
            };
        }
    }
}