using BookStore.Enums;

namespace BookStore.Dtos
{
    public class CreateBookDto
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public Genre Genre { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
    }
}
