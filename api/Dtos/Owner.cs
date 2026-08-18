using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos
{
    public class Owner
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Gym { get; set; }
        public int CountryId { get; set; }
        public ICollection<PokemonDtos>? Pokemons { get; set; }
        public Country? Country { get; set; }
    }
}