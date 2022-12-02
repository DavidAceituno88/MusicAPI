namespace MusicAPI.Models
{
    public class AuthorDTO
    {
        public string Name { get; set; }
        public List<SongOnlyDTO>? Songs { get; set; }
    }
}
