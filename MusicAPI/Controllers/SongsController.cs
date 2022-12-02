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
    [Route("api/SongsControllers")]
    [ApiController]
    public class SongsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;

        public SongsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        // GET: api/Songs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SongDTO>>> GetSongs()
        {
          if (_context.Songs == null)
          {
              return NotFound();
            }

            var song = await _context.Songs.ToListAsync();
            return mapper.Map<List<SongDTO>>(song);
        }

        // GET: api/Songs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SongDTO>> GetSong(int id)
        {
          if (_context.Songs == null)
          {
              return NotFound();
          }

          //I included the author so that later we can get the name of the author
            var songExist = await _context.Songs.Include(x => x.Author).FirstOrDefaultAsync(x =>x.SongId == id);

            if (songExist == null)
            {
                return NotFound();
            }

            var song = mapper.Map<SongDTO>(songExist);
            //Assign the name of the autor included in the songExist query 
            song.AuthorName = songExist.Author.Name;
            return song;
        }

        // PUT: api/Songs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSong(int id, Song song)
        {
            if (id != song.SongId)
            {
                return BadRequest();
            }

            _context.Entry(song).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SongExists(id))
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

        // POST: api/Songs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SongCreationDTO>> PostSong(SongCreationDTO songCreationDTO)
        {
          if (_context.Songs == null)
          {
              return Problem("Entity set 'ApplicationDbContext.Songs'  is null.");
          }
            
            var authorExists = await _context.Authors.AnyAsync(authorDb => authorDb.AuthorId == songCreationDTO.AuthorId);
            
            // Verify for Son names duplicates on the same author.
            var songsExists = await _context.Songs.AnyAsync(s => s.AuthorId == songCreationDTO.AuthorId && s.Name == songCreationDTO.Name);
            
            if(songsExists)
            {
                return BadRequest($"The song: {songCreationDTO.Name} already exist for the author with id: {songCreationDTO.AuthorId} ");
            }
            if (!authorExists)
            {
                return NotFound("The Author doesn't exists");
            }
            
            //Here we map the DTO to the original Entity
            var song = mapper.Map<Song>(songCreationDTO);

            _context.Songs.Add(song);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Songs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSong(int id)
        {
            if (_context.Songs == null)
            {
                return NotFound();
            }
            var song = await _context.Songs.FindAsync(id);
            if (song == null)
            {
                return NotFound();
            }

            _context.Songs.Remove(song);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SongExists(int id)
        {
            return (_context.Songs?.Any(e => e.SongId == id)).GetValueOrDefault();
        }
    }
}
