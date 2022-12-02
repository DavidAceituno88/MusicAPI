using AutoMapper;
using MusicAPI.Models;

namespace MusicAPI.Utilities
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles() 
        {
            CreateMap<AuthorCreationDTO, Author>();
            CreateMap<SongCreationDTO, Song>();
            CreateMap<Author, AuthorDTO>();
            CreateMap<Song, SongDTO>(); 
            CreateMap<Song, SongOnlyDTO>(); 
        }
    }
}
