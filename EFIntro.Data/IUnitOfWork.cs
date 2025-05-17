using EFIntro.Data.Interfaces;

namespace EFIntro.Data
{
    public interface IUnitOfWork
    {
        IAuthorRepository Authors { get; }
        IBookRepository Books { get; }
        int Complete();
    }
}
