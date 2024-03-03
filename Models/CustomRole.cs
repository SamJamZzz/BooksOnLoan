using Microsoft.AspNetCore.Identity;

namespace BooksOnLoan.Models
{
    public class CustomRole : IdentityRole
    {
        public CustomRole() : base() { }
        public DateTime CreatedDate { get; set; }
    }
}