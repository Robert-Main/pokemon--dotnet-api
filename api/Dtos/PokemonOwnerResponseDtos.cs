namespace api.Dtos
{
    public class PokemonOwnerResponseDtos
    {
        public int OwnerId { get; set; }
        public OwnerResponseDtos? Owner { get; set; }
    }
}
