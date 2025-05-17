using EFIntro.Entities;
using EFIntro.Service.DTOs.Book;

namespace EFIntro.Service.Mappers
{
    public static class BookMapper
    {
        public static BookDto ToDto(Book book) => new()
        {
            Id = book.Id,
            Title = book.Title,
            Pages=book.Pages,
            PublishDate=book.PublishDate,
            AuthorId=book.AuthorId

        };
        public static BookListDto ToBookListDto(Book book) => new()
        {
            Id = book.Id,
            Title = book.Title

        };

        public static Book ToEntity(BookCreateDto bookDto) => new()
        {
            Title = bookDto.Title,
            Pages = bookDto.Pages,
            PublishDate = bookDto.PublishDate,
            AuthorId = bookDto.AuthorId
        };

        public static Book ToEntity(BookUpdateDto bookDto) => new()
        {
            Id=bookDto.Id,
            Title = bookDto.Title,
            Pages = bookDto.Pages,
            PublishDate = bookDto.PublishDate,
            AuthorId = bookDto.AuthorId
        };

    }

}
