using EFIntro.Service.DTOs.Author;

namespace EFIntro.Service.DTOs.Book
{
    public class BooksWithAuthorDto
    {
        public AuthorDto Author { get; set; } = null!;
        public List<BookDto>? Books { get; set; }
    }
}
