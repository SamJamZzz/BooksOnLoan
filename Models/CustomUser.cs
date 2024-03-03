using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BooksOnLoan.Models
{
    public class CustomUser : IdentityUser
    {
        public CustomUser() : base() { }
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }
        public ICollection<Hold>? Holds { get; set; }
    }
}