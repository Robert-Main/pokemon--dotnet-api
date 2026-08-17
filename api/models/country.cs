using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PokemonReviewApp.Models;

namespace api.models
{
    public class Country
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public ICollection<Owner>? Owners { get; set; }
    }
}