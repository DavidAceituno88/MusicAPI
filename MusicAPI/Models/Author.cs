namespace MusicAPI.Models
{
    public class Author
    {
        public int AuthorId { get; set; }
        public string Name { get; set; }
        public List<Song>? Songs { get; set; }
    }
}
