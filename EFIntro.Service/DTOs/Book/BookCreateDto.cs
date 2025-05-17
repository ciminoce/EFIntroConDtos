namespace EFIntro.Service.DTOs.Book
{
    public class BookCreateDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public DateOnly PublishDate { get; set; }
        public int Pages { get; set; }
        public int AuthorId { get; set; }

    }
}
