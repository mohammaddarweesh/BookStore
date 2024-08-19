using BookStore.Enums;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Dtos
{
    public class BookDto
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public Genre Genre { get; set; }
        [Required]
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; } = true;
    }
}
