using EFIntro.Data.Interfaces;
using EFIntro.Entities;
using EFIntro.Service.DTOs.Author;
using EFIntro.Service.DTOs.Book;
using EFIntro.Service.Interfaces;
using EFIntro.Service.Mappers;
using EFIntro.Service.Validators;

namespace EFIntro.Service.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository = null!;

        public AuthorService(IAuthorRepository repository)
        {
            _authorRepository = repository;
        }

        public List<AuthorBooksCountDto> AuthorsWithBooksCount()
        {
            var authorWithBooks = _authorRepository.GetAllWithBooks();
            return authorWithBooks.Select(a => new AuthorBooksCountDto
            {
                Id = a.Id,
                FullName = $"{a.FirstName} {a.LastName}",
                BooksCount=a.Books is null?0: a.Books.Count,
                Books = a.Books != null
                ? a.Books.Select(BookMapper.ToDto).ToList()
                : new List<BookDto>()
             }).ToList();

        }

        public bool Create(AuthorCreateDto authorDto, out List<string> errors)
        {
            errors = new List<string>();
            Author author=AuthorMapper.ToEntity(authorDto);
            if (_authorRepository.Exist(authorDto.FirstName, 
                    authorDto.LastName))
            {
                errors.Add("Author already exist");
                return false;
            }
            var authorValidator = new AuthorValidator();
            if(!UniversalValidator.IsValid(author,authorValidator, out errors))
            {
                return false;
            }
            _authorRepository.Add(author);
            _authorRepository.SaveChanges();
            return true;
        }

        public bool Create(AuthorCreateDto authorDto, out AuthorDto? authorCreated, out List<string> errors)
        {
            errors = new List<string>();
            authorCreated = null;
            Author author = AuthorMapper.ToEntity(authorDto);
            if (_authorRepository.Exist(authorDto.FirstName,
                    authorDto.LastName))
            {
                errors.Add("Author already exist");
                return false;
            }
            var authorValidator = new AuthorValidator();
            if (!UniversalValidator.IsValid(author, authorValidator, out errors))
            {
                return false;
            }
            _authorRepository.Add(author);
            _authorRepository.SaveChanges();
            authorCreated = AuthorMapper.ToDto(author);
            return true;
        }

        public bool Delete(int authorId, out List<string>errors)
        {
            errors = new List<string>();
            if(_authorRepository.GetById(authorId) is null)
            {
                errors.Add("Author does not exist!!");
                return false;
            }
            if (_authorRepository.HasDependencies(authorId))
            {
                //TODO: Pedir al otro repo los libros
                errors.Add("Author with dependencies!!!");
                return false;
            }
            _authorRepository.Delete(authorId);
            _authorRepository.SaveChanges();
            return true;
        }

        public bool Exist(string firstName, string lastName, int? excludeId = null)
        {
            return _authorRepository.Exist(firstName, lastName, excludeId);
        }

        public List<AuthorDto> GetAll(string sortedBy = "LastName")
        {
            var authors=_authorRepository.GetAll(sortedBy);
            return authors.Select(AuthorMapper.ToDto).ToList();
        }

        public List<AuthorWithBooksDto> GetAllWithBooks()
        {
            var authorWithBooks=_authorRepository.GetAllWithBooks();
            return authorWithBooks.Select(a=>new AuthorWithBooksDto
            {
                Id = a.Id,
                FullName=$"{a.FirstName} {a.LastName}",
                Books=a.Books!=null
                    ? a.Books.Select(BookMapper.ToDto).ToList()
                    :new List<BookDto>()
            }).ToList();
        }

        public AuthorDto? GetById(int authorId)
        {
            var author= _authorRepository.GetById(authorId);
            return author is null?null: AuthorMapper.ToDto(author);
        }

        public AuthorDto? GetByName(string firstName, string lastName)
        {
            var author= _authorRepository.GetByName(firstName, lastName);
            return author is null ? null : AuthorMapper.ToDto(author);

        }

        public bool Update(AuthorUpdateDto authorDto, out List<string> errors)
        {
            errors = new List<string>();
            Author author = AuthorMapper.ToEntity(authorDto);
            if(_authorRepository.Exist(authorDto.FirstName, 
                authorDto.LastName,
                authorDto.Id))
            {
                errors.Add("Author already exist!!!");
                return false;
            }
            var authorValidator = new AuthorValidator();
            if (!UniversalValidator.IsValid(author, authorValidator, out errors))
            {
                return false;
            }
            _authorRepository.Update(author);
            _authorRepository.SaveChanges();
            return true;

        }
    }
}
