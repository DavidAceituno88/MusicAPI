using System.Security.Permissions;

namespace MusicAPI.Models
{
    public class Song
    {
        public int SongId { get; set; }
        public string Name { get; set; }
        public int AuthorId { get; set; }
        public Author Author{ get; set; }
    }
}
