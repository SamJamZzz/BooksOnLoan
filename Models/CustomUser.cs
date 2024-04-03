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
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }
        public string? Country { get; set; }
        public ICollection<Hold>? Holds { get; set; }
    }
}