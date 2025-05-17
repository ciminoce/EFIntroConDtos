namespace EFIntro.Service.DTOs.Book
{
    public class BookUpdateDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public int Pages { get; set; }
        public DateOnly PublishDate { get; set; }
        public int AuthorId { get; set; }
    }
}
