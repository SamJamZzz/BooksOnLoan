using System.ComponentModel.DataAnnotations;

namespace BooksOnLoan.Models
{
    public class Book
    {
        [Key]
        public int BookCode { get; set; }
        public string? Author { get; set; }
        public string? Title { get; set; }
        [Display(Name = "Year Published")]
        public int YearPublished { get; set; }
        public int Quantity { get; set; }
    }
}