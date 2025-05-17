using EFIntro.Data;
using EFIntro.Entities;
using EFIntro.Service.DTOs.Author;
using EFIntro.Service.DTOs.Book;
using EFIntro.Service.Interfaces;
using EFIntro.Service.Mappers;
using EFIntro.Service.Validators;

namespace EFIntro.Service.Services
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork _unitOfWork = null!;

        public BookService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public bool Delete(int bookId, out List<string> errors)
        {
            errors = new List<string>();
            if (_unitOfWork.Books.GetById(bookId) is null)
            {
                errors.Add("Book ID not found");
                return false;
            }
            _unitOfWork.Books.Delete(bookId);
            _unitOfWork.Complete();
            return true;
        }

        public bool Exist(string bookTitle, int bookAuthorId, int? excludeId = null)
        {
            return _unitOfWork.Books.Exist(bookTitle, bookAuthorId, excludeId);
        }

        public List<BookListDto> GetAll(string sortedBy = "Title")
        {
            var books = _unitOfWork.Books.GetAll(sortedBy);
            return books.Select(BookMapper.ToBookListDto).ToList();
        }

        public BookDto? GetById(int bookId)
        {
            var book = _unitOfWork.Books.GetById(bookId);
            return book is null ? null : BookMapper.ToDto(book);
        }


        public List<BooksWithAuthorDto> BooksGroupByAuthor()
        {
            var booksWithAuthors = _unitOfWork.Books.GetAll();
            var grouped = booksWithAuthors
                .GroupBy(b => new { b.Author!.Id, b.Author.FirstName, b.Author.LastName })
                .Select(g => new BooksWithAuthorDto
                {
                    Author = new AuthorDto
                    {
                        Id = g.Key.Id,
                        FirstName = g.Key.FirstName,
                        LastName = g.Key.LastName,
                    },
                    Books = g.Select(BookMapper.ToDto).ToList() // Changed from ToBookListDto to ToDto
                }).ToList();
            return grouped;
        }

        public bool Create(BookCreateDto bookDto, out List<string> errors)
        {
            errors = new List<string>();
            Book book = BookMapper.ToEntity(bookDto);
            if (_unitOfWork.Books.Exist(book.Title, book.AuthorId))
            {
                errors.Add("Book already exist");
                return false;
            }
            BookValidator bookValidator = new BookValidator();
            if (!UniversalValidator.IsValid(book, bookValidator, out errors))
            {
                return false;
            }
            _unitOfWork.Books.Add(book);
            _unitOfWork.Complete();
            return true;
        }

        public bool Update(BookUpdateDto bookDto, out List<string> errors)
        {
            errors = new List<string>();
            Book book = BookMapper.ToEntity(bookDto);
            if (_unitOfWork.Books.Exist(book.Title, book.AuthorId, book.Id))
            {
                errors.Add("Book already exist");
                return false;
            }
            BookValidator bookValidator = new BookValidator();
            if (!UniversalValidator.IsValid(book, bookValidator, out errors))
            {
                return false;
            }
            _unitOfWork.Books.Update(book);
            _unitOfWork.Complete();
            return true;
        }
    }
}
