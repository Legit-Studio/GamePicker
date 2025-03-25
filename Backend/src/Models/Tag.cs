namespace Backend.Models
{
    public sealed class Tag
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        public ICollection<Game> Games { get; set; } = [];
    }
}