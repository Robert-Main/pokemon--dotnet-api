using System;

namespace api.Dtos
{
    public class PokemonCreateDtos
    {
        public string? Name { get; set; }
        public DateTime BirthDate { get; set; }
    }
}