using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FoodOrder.Models
{
    // В профиль пользователя можно добавить дополнительные данные, если указать больше свойств для класса ApplicationUser. Подробности см. на странице https://go.microsoft.com/fwlink/?LinkID=317594.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Обратите внимание, что authenticationType должен совпадать с типом, определенным в CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Здесь добавьте утверждения пользователя
            return userIdentity;
        }
    }

    public class UserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string SecondName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class User : IdentityUser
    {
        public string SecondName { get; set; }
        public string CompanyId { get; set; }
        public Company Company { get; set; }
        public ICollection<Order> Orders { get; set; }

        public User()
        {
            Orders = new List<Order>();
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager, string authenticationType)
        {
            // Обратите внимание, что authenticationType должен совпадать с типом, определенным в CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Здесь добавьте утверждения пользователя
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Company>().HasMany(m => m.Employees);
            modelBuilder.Entity<Order>()
                .HasRequired(m => m.User)
                .WithMany(m => m.Orders)
                .HasForeignKey(m => m.UserId)
                .WillCascadeOnDelete(true);
            modelBuilder.Entity<User>()
                .HasOptional(m=>m.Company)
                .WithMany(m=>m.Employees)
                .HasForeignKey(m=>m.CompanyId)
                .WillCascadeOnDelete(true);
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<RoleViewModel> IdentityRoles { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<ComplexDish> ComplexDishes { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<UserViewModel> UserViewModels { get; set; }
    }
}