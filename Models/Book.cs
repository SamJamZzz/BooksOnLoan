using System.ComponentModel.DataAnnotations;

namespace BooksOnLoan.Models
{
    public class Book
    {
        [Key]
        public int BookCode { get; set; }
        [Required]
        public string? Author { get; set; }
        [Required]
        public string? Title { get; set; }
        [Required]
        [Display(Name = "Year Published")]
        public int YearPublished { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}