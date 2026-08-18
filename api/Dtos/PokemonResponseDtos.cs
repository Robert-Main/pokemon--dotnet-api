using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos
{
    public class PokemonResponseDtos : PokemonDtos
    {
        public List<ReviewResponseDtos>? Reviews { get; set; }
        public List<PokemonCategoryResponseDtos>? PokemonCategories { get; set; }
    }
}
