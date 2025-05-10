using EFIntro.Entities;

namespace EFIntro.Data.Interfaces
{
    public interface IAuthorRepository
    {
        void Add(Author author);
        void Delete(int authorId);
        void Update(Author author);
        bool Exist(string firstName, string lastName, int? excludeId = null);
        List<Author> GetAll(string sortedBy = "LastName");
        Author? GetById(int authorId);
        bool HasDependencies(int authorId);
        void LoadBooks(Author author);
        List<Author> GetAllWithBooks();
        Author? GetByName(string firstName, string lastName);
        void SaveChanges();
    }
}