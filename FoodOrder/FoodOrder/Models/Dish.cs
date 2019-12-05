using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FoodOrder.Models
{

    public class CookStatisticViewModel {
        public List<DishStatistic> DishStatistics { get; set; }

    }

    public class CompanyDishStat
    {
        public string CompanyName { get; set; }
        public List<StatGarnDish> statGarnDishes { get; set; }
    }

    public class StatGarnDish
    {
       // public string Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public string OrderId { get; set; }
    }

    public class Order
    {
        public string Id { get; set; }
        public DateTime DateOfCreation { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }

        public string OrderSpecId { get; set; }
        public OrderStatus Status { get; set; }
        public double Price { get; set; }
        public ICollection<ChoosenDish> ChoosenDishes { get; set; }
        public ICollection<Pack> Packs { get; set; }
        public Order()
        {
            Packs = new List<Pack>();
            ChoosenDishes = new List<ChoosenDish>();
        }
    }

    public class DishStatistic
    {
        public string Id { get; set; }
        public string DishName { get; set; }
        public int Count { get; set; }
        public string DishCategoryName { get; set; }
        public bool IsReady { get; set; }
    }

    public class MenuViewModel
    {
        public Menu Menu { get; set; }
        public ICollection<DishCategory> DishCategories { get; set; }
        public MenuViewModel()
        {
            DishCategories = new List<DishCategory>();
        }
    }

    public class Menu
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfCreation { get; set; }
        public ICollection<Dish> Dishes { get; set; }
        public ICollection<Pack> Packs { get; set; }
        public Menu()
        {
            Packs = new List<Pack>();
            Dishes = new List<Dish>();
        }
    }

    public class Pack
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfCreation { get; set; }
        public Menu Menu { get; set; }
        public string MenuId { get; set; }
        public ICollection<Dish> Dishes { get; set; }
        public ICollection<Order> Orders { get; set; }
        public string GarnishesForPacks { get; set; }

        public Pack()
        {
           
            Dishes = new List<Dish>();
            Orders = new List<Order>();
        }
    }


    public class DishCategory
    {
        public string Id { get; set; }

        public string Name { get; set; }
        public DateTime DateOfCreation { get; set; }

        public ICollection<Dish> Dishes { get; set; }

        public DishCategory()
        {
            Dishes = new List<Dish>();
        }

    }

    public class ChoosenDish
    {
        public string Id { get; set; }
        public string DishName { get; set; }
        public string DishCategoryName { get; set; }
        public double Proteins { get; set; }
        public double Fats { get; set; }

        public double Weight { get; set; }
        public double Carbonhydrates { get; set; }
        public double Kilocalories { get; set; }
        public string ImagePath { get; set; }
        public string GarinshName { get; set; }
        public string OrderId { get; set; }
        public Order Order { get; set; }
    }

    public class Dish
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfCreation { get; set; }
        public DishCategory DishCategory { get; set; }
        public string DishCategoryId { get; set; }
        public bool HasGarnish { get; set; }
      //  public string GarnishId { get; set; }
      //  public Dish Garnish { get; set; }
        public double Proteins { get; set; }
        public double Weight { get; set; }
        public double Fats { get; set; }
        public double Carbonhydrates { get; set; }
        public double Kilocalories { get; set; }
        public string ImagePath { get; set; }
        public double Price { get; set; }

        public ICollection<Garnish> Garnishes { get; set; }
        public ICollection<Garnish> GarnishesForPacks { get; set; }
        public ICollection<Pack> Packs { get; set; }

        public ICollection<Menu> Menus { get; set; }

        public Dish()
        {
            GarnishesForPacks = new List<Garnish>();
            Garnishes = new List<Garnish>();
            Packs = new List<Pack>();
            Menus = new List<Menu>();
           
        }

    }

    public class Garnish
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ICollection<Dish> Dishes { get; set; }
        public Garnish()
        {
            Dishes = new List<Dish>();
        }

    }


    public enum OrderStatus
    {
        Waiting,
        Done,
        Cancelled
    }
}