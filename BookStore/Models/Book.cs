using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; }
        public string? CoverPath { get; set; } = "https://png.pngtree.com/png-vector/20191027/ourmid/pngtree-book-cover-template-vector-realistic-illustration-isolated-on-gray-background-empty-png-image_1893997.jpg";
        public string? Language { get; set; }
        [Required]
        public int AuthorId { get; set; }
    }
}
