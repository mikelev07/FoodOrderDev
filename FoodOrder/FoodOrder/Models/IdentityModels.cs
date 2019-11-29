using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
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
            
            return userIdentity;
        }
    }

    public class UserViewModel
    {
        public string Id { get; set; }

        /// <summary>Логин</summary>
        public string UserName { get; set; }

        /// <summary>Имя</summary>
        public string Name { get; set; }

        /// <summary>Фамилия</summary>
        public string Surname { get; set; }

        /// <summary>Отчество</summary>
        public string Patronymic { get; set; }

        public int Age { get; set; } //надо развернуть в обычное свойство, дабы логику с корректным возрастом задать

        public string CompanyId { get; set; }

        public Company Company { get; set; }

        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public bool? IsVisible { get; set; }

        public string PhoneNumber { get; set; }

        /// <summary>Уже сделал заказ сегодня</summary>
        public bool HasOrderToday { get; set; }

        public bool IsCheckInstruction { get; set; }
    }

    public class NewEmployeeViewModel
    {
        public string Id { get; set; }
        /// <summary>Логин</summary>
        public string UserName { get; set; }
        /// <summary>Имя</summary>
        public string Name { get; set; }

        /// <summary>Фамилия</summary>
        public string Surname { get; set; }

        /// <summary>Отчество</summary>
        public string Patronymic { get; set; }

        public int Age { get; set; }
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Текущий пароль")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Значение {0} должно содержать символов не менее: {2}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Новый пароль")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение нового пароля")]
        [Compare("NewPassword", ErrorMessage = "Новый пароль и его подтверждение не совпадают.")]
        public string ConfirmPassword { get; set; }
    }

    public class EditUserViewModel
    {
        public string Id { get; set; }

        /// <summary>Логин</summary>
        public string UserName { get; set; }

        /// <summary>Имя</summary>
        public string Name { get; set; }

        /// <summary>Фамилия</summary>
        public string Surname { get; set; }

        /// <summary>Отчество</summary>
        public string Patronymic { get; set; }

        public int Age { get; set; } //надо развернуть в обычное свойство, дабы логику с корректным возрастом задать

        public string Email { get; set; }

        public string PhoneNumber { get; set; }
    }

    public class User : IdentityUser
    {
        /// <summary>Имя</summary>
        public string Name { get; set; }

        /// <summary>Фамилия</summary>
        public string Surname { get; set; }

        /// <summary>Отчество</summary>
        public string Patronymic { get; set; }

        public int Age { get; set; }

        public bool IsCheckInstruction { get; set; }

        public string CompanyId { get; set; }

        public bool? IsVisible { get; set; }

        public Company Company { get; set; }

        public DateTime RegistrationDate { get; set; }

        /// <summary>Указывает на то, что пользователь не используется</summary>
        public bool NotActual { get; set; }

        public ICollection<Order> Orders { get; set; }

        public string ImagePath { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }

  

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

            modelBuilder.Entity<Dish>().HasMany(c => c.Garnishes)
                  .WithMany(s => s.Dishes)
                  .Map(t => t.MapLeftKey("DishId")
                  .MapRightKey("GarnishId")
                  .ToTable("DishGarnish"));

            modelBuilder.Entity<Dish>().HasMany(c => c.Packs)
              .WithMany(s => s.Dishes)
              .Map(t => t.MapLeftKey("DishId")
              .MapRightKey("PackId")
              .ToTable("DishPack"));

            modelBuilder.Entity<Order>().HasMany(c => c.Packs)
            .WithMany(s => s.Orders)
            .Map(t => t.MapLeftKey("OrderId")
            .MapRightKey("PackId")
            .ToTable("OrderPack"));

        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<DishCategory> DishCategories { get; set; }
        public DbSet<RoleViewModel> IdentityRoles { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<DishStatistic> DishStatistics { get; set; }
        public DbSet<ChoosenDish> ChoosenDishes { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Pack> Packs { get; set; }
        public DbSet<Garnish> Garnishes { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Order> Orders { get; set; }

        public System.Data.Entity.DbSet<FoodOrder.Models.UserViewModel> UserViewModels { get; set; }
    }
}