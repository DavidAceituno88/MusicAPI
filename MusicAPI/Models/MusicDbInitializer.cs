using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace MusicAPI.Models
{
    public class MusicDbInitializer
    {
        private readonly ApplicationDbContext context;

        public MusicDbInitializer(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void Seed()
        {
            if(!context.Authors.Any())
            {
                var authors = new List<Author>()
                {
                    new Author()
                    {
                        AuthorId = 1, Name = "Jose"
                    },
                    new Author()
                    { 
                        AuthorId = 2, Name ="David"
                    }
                };

                context.Authors.AddRange(authors);
                context.SaveChanges();
            }
        }
    }
}
