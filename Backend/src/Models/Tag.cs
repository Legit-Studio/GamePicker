namespace Backend.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        public virtual ICollection<Game> Games { get; set; } = [];
    }
}