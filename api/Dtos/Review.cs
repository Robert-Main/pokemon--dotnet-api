using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos
{
    public class Review
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Text { get; set; }
        public int Rating { get; set; }
        public int ReviewerId { get; set; }
        public int PokemonId { get; set; }
    }
}
