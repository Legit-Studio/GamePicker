namespace Backend.Src.Models;

public class Game
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string SteamId { get; set; }
    public required string ImageUrl { get; set; }
    public DateTime? ReleaseDate { get; set; }
    public DateTime? AddedDate { get; set; }
    public virtual ICollection<Tag> Tags { get; set; } = [];
}