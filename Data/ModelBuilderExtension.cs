using BooksOnLoan.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BooksOnLoan.Data
{
    public static class ModelBuilderExtension
    {
        public static void Seed(this ModelBuilder builder)
        {
            const string adminEmail = "aa@aa.aa";
            const string memberEmail = "mm@mm.mm";
            const string seedPassword = "P@$$w0rd";

            var passwordHasher = new PasswordHasher<CustomUser>();

            var adminRole = new CustomRole()
            {
                Name = "Admin",
                CreatedDate = DateTime.Now
            };
            adminRole.NormalizedName = adminRole.Name.ToUpper();

            var memberRole = new CustomRole()
            {
                Name = "Member",
                CreatedDate = DateTime.Now
            };
            memberRole.NormalizedName = memberRole.Name.ToUpper();

            List<CustomRole> roles = new List<CustomRole>
            {
                adminRole,
                memberRole
            };

            builder.Entity<CustomRole>().HasData(roles);

            var adminUser = new CustomUser()
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                FirstName = "Adam",
                LastName = "Atkins"
            };
            adminUser.NormalizedUserName = adminUser.UserName.ToUpper();
            adminUser.NormalizedEmail = adminUser.Email.ToUpper();
            adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, seedPassword);

            var memberUser = new CustomUser()
            {
                UserName = memberEmail,
                Email = memberEmail,
                EmailConfirmed = true,
                FirstName = "Mike",
                LastName = "Moore"
            };
            memberUser.NormalizedUserName = memberUser.UserName.ToUpper();
            memberUser.NormalizedEmail = memberUser.Email.ToUpper();
            memberUser.PasswordHash = passwordHasher.HashPassword(memberUser, seedPassword);

            List<CustomUser> users = new List<CustomUser>
            {
                adminUser,
                memberUser
            };

            builder.Entity<CustomUser>().HasData(users);

            List<IdentityUserRole<string>> userRoles =
            [
                new IdentityUserRole<string>
                {
                    UserId = users[0].Id,
                    RoleId = roles.First(r => r.Name == "Admin").Id
                },
                new IdentityUserRole<string>
                {
                    UserId = users[1].Id,
                    RoleId = roles.First(r => r.Name == "Member").Id
                },
            ];

            builder.Entity<IdentityUserRole<string>>().HasData(userRoles);
        }
    }
}