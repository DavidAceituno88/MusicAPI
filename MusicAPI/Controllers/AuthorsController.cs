using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicAPI.Models;

namespace MusicAPI.Controllers
{
    [Route("api/AuthorsController")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;

        public AuthorsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }
        //Basic Data Seeder... 
        [HttpGet("Seed")]
        public async Task<ActionResult> Seed()
        {
            if(!_context.Authors.Any())
            {
                List<Author> authors = new List<Author>
                {
                    new Author 
                    {
                        AuthorId= 1, 
                        Name ="Jose", 
                            Songs = new List<Song>()
                            { 
                            
                            new Song  {  SongId = 1, Name = "Song 1", AuthorId = 1 },
                            new Song  {  SongId = 2, Name = "Song 2", AuthorId = 1 }

                            } 
                    },
                    new Author 
                    {
                        AuthorId= 2,
                        Name ="David",
                            Songs = new List<Song>()
                            {

                            new Song  {  SongId = 3, Name = "Song 3", AuthorId = 2 },
                            new Song  {  SongId = 4, Name = "Song 4", AuthorId = 2 }

                            }
                    }
                };
                _context.Authors.AddRange(authors);
                _context.SaveChanges();
                return Ok(authors);
            }
            return BadRequest("The Author table is not empty");

        }

        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDTO>>> GetAuthor()
        {
          if (_context.Authors == null)
          {
              return NotFound();
          }

            var author = await _context.Authors.Include(x => x.Songs).ToListAsync();
            return mapper.Map<List<AuthorDTO>>(author);
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDTO>> GetAuthor(int id)
        {
          if (_context.Authors == null)
          {
              return NotFound();
          }
            var author = await _context.Authors.Include(authorDb => authorDb.Songs).FirstOrDefaultAsync(x => x.AuthorId == id);

            if (author == null)
            {
                return NotFound();
            }

            return mapper.Map<AuthorDTO>(author);
        }

        // PUT: api/Authors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor(int id, Author author)
        {
            if (id != author.AuthorId)
            {
                return BadRequest();
            }

            _context.Entry(author).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Authors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AuthorCreationDTO>> PostAuthor(AuthorCreationDTO authorCreationDTO)
        {
            //Here we validate to check that we do not add duplicates of the same entity
            var exists = await _context.Authors.AnyAsync(x => x.Name == authorCreationDTO.Name);
          
            if (exists)
          {
              return BadRequest($"An Author with the name: {authorCreationDTO.Name} already exists ");
          }
            //Here we map the DTO to the original Entity
            var author = mapper.Map<Author>(authorCreationDTO);
            
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost ("Super Post")]
        public async Task<ActionResult<List<AuthorCreationDTO>>> SuperPostAuthor(List<AuthorCreationDTO> authors)
        {
            
            var authorList = mapper.Map<List<Author>>(authors);
            _context.Authors.AddRange(authorList);
            _context.SaveChanges();
            return Ok();

        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            if (_context.Authors == null)
            {
                return NotFound();
            }
            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AuthorExists(int id)
        {
            return (_context.Authors?.Any(e => e.AuthorId == id)).GetValueOrDefault();
        }
    }
}
