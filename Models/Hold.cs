using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BooksOnLoan.Models
{
    public class Hold
    {
        public int Id { get; set; }
        [Display(Name = "Hold Date")]
        public DateTime HoldDate { get; set; }
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }
        [Display(Name = "Returned")]
        public bool IsReturned { get; set; }

        [ForeignKey("BookCode")]
        public Book? Book { get; set; }
        public int BookCode { get; set; }

        [ForeignKey("UserId")]
        public CustomUser? User { get; set; }
        public string? UserId { get; set; }
    }
}