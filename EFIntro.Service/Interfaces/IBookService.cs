using EFIntro.Entities;
using EFIntro.Service.DTOs.Book;

namespace EFIntro.Service.Interfaces
{
    public interface IBookService
    {
        bool Create(BookCreateDto bookDto, out List<string> errors);
        bool Update(BookUpdateDto bookDto, out List<string> errors);

        bool Delete(int bookId, out List<string> errors);
        bool Exist(string bookTitle, int bookAuthorId, int? excludeId = null);
        List<BookListDto> GetAll(string sortedBy = "Title");
        BookDto? GetById(int bookId);
        List<BooksWithAuthorDto> BooksGroupByAuthor();
    }
}
