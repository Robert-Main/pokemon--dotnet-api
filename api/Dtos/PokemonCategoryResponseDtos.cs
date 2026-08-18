using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.models;

namespace api.Dtos
{
    public class PokemonCategoryResponseDtos
    {
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
