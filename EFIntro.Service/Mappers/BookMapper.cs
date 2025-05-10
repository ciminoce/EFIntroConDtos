using EFIntro.Entities;
using EFIntro.Service.DTOs.Book;

namespace EFIntro.Service.Mappers
{
    public static class BookMapper
    {
        public static BookDto ToDto(Book book) => new()
        {
            Id = book.Id,
            Title = book.Title
        };
    }
}
