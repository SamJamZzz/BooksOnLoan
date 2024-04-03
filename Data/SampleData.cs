using BooksOnLoan.Models;

namespace BooksOnLoan.Data
{
    public class SampleData
    {
        public static List<Book> GetBooks()
        {
            List<Book> books = new List<Book>() {
                new Book() {
                    BookCode = 1,
                    Author = "Andrew Chevallier",
                    Title = "Encyclopedia of Herbal Medicine: 550 Herbs and Remedies for Common Ailments",
                    YearPublished = 2016,
                    Quantity = 1
                },
                new Book() {
                    BookCode = 2,
                    Author = "Michael T. Murray M.D. and Joseph Pizzorno",
                    Title = "The Encyclopedia of Natural Medicine Third Edition",
                    YearPublished = 2012,
                    Quantity = 3
                },
                new Book() {
                    BookCode = 3,
                    Author = "Thomas Easley and Steven Horne",
                    Title = "The Modern Herbal Dispensatory: A Medicine-Making Guide",
                    YearPublished = 2016,
                    Quantity = 1
                },
                new Book() {
                    BookCode = 4,
                    Author = "Cat Ellis",
                    Title = "Prepper's Natural Medicine: Life-Saving Herbs, Essential Oils and Natural Remedies for When There is No Doctor",
                    YearPublished = 2015,
                    Quantity = 2
                },
                new Book() {
                    BookCode = 5,
                    Author = "Rosemary Gladstar",
                    Title = "Rosemary Gladstar's Medicinal Herbs: A Beginner's Guide: 33 Healing Herbs to Know, Grow, and Use",
                    YearPublished = 2012,
                    Quantity = 1
                }
            };
            return books;
        }
    }
}