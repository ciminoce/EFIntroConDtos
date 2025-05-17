using EFIntro.Data.Interfaces;

namespace EFIntro.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LibraryContext _context;

        public UnitOfWork(LibraryContext context, 
            IAuthorRepository authors, 
            IBookRepository books)
        {
            _context = context;
            Authors = authors;
            Books = books;
        }

        public IAuthorRepository Authors { get; }
        public IBookRepository Books { get; }

        public int Complete()
        {
            return _context.SaveChanges();
        }
    }
}
