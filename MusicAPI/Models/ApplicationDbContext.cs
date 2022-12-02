using Microsoft.EntityFrameworkCore;
using MusicAPI.Models;

namespace MusicAPI.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options) 
            : base(options) 
        {
            
        }

        public DbSet<Song> Songs { get; set; } 

        public DbSet<Author> Authors { get; set; }

       

    }

}
