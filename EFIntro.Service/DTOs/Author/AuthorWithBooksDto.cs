using EFIntro.Service.DTOs.Book;

namespace EFIntro.Service.DTOs.Author
{
    public class AuthorWithBooksDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public List<BookDto> Books { get; set; } = null!;
    }
}
