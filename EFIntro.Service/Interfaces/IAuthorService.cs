using EFIntro.Entities;
using EFIntro.Service.DTOs.Author;

namespace EFIntro.Service.Interfaces
{
    public interface IAuthorService
    {
        bool Create(AuthorCreateDto authorDto, out List<string> errors);
        bool Update(AuthorUpdateDto authorDto, out List<string> errors);
        bool Delete(int authorId, out List<string> errors);
        bool Exist(string firstName, string lastName, int? excludeId = null);
        List<AuthorDto> GetAll(string sortedBy = "LastName");
        AuthorDto? GetById(int authorId);
        AuthorDto? GetByName(string firstName, string lastName);
        List<AuthorWithBooksDto> GetAllWithBooks();
        List<AuthorBooksCountDto> AuthorsWithBooksCount();
    }
}
