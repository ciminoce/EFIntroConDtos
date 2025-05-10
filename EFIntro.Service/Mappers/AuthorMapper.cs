using EFIntro.Entities;
using EFIntro.Service.DTOs.Author;

namespace EFIntro.Service.Mappers
{
    public static class AuthorMapper
    {
        public static AuthorDto ToDto(Author author) => new()
        {
            Id = author.Id,
            FirstName = author.FirstName,
            LastName = author.LastName,
        };

        public static Author ToEntity(AuthorCreateDto authorDto) => new()
        {
            FirstName = authorDto.FirstName,
            LastName = authorDto.LastName
        };

        public static Author ToEntity(AuthorUpdateDto authorDto) => new()
        {
            Id = authorDto.Id,
            FirstName = authorDto.FirstName,
            LastName = authorDto.LastName
        };

    }
}
