using EFIntro.Service.DTOs.Book;

namespace EFIntro.Service.DTOs.Author
{
    public class AuthorBooksCountDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public int BooksCount { get; set; }
        public List<BookDto> Books { get; set; } = null!;
    }
}
