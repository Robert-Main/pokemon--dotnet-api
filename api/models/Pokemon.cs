using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.models
{
    public class Pokemon
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime? CreatedAt { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public ICollection<PokemonCategory>? PokemonCategories { get; set; }
        public ICollection<PokemonOwner>? PokemonOwners { get; set; }
    }

}