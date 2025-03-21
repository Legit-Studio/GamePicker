namespace Backend.Src.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        public virtual ICollection<GameTag> GameTags { get; set; } = [];
    }
}