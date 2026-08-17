using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.models
{
    public class Owner
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Gym { get; set; }
        public Country? Country { get; set; }
        public ICollection<PokemonOwner>? PokemonOwners { get; set; }

    }
}