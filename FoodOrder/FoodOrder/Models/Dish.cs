using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FoodOrder.Models
{
    public class Order
    {
        public string Id { get; set; }
        public DateTime DateOfCreation { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public OrderStatus Status { get; set; }
        public double Price { get; set; }
        public ICollection<Dish> Dishes { get; set; }
        public Order()
        {
            Dishes = new List<Dish>();
        }
    }
    public class Menu
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfCreation { get; set; }
        public ICollection<Pack> Packs { get; set; }
        public Menu()
        {
            Packs = new List<Pack>();
        }
    }

    public class Pack
    {
        public string Id { get; set; }
        public DateTime DateOfCreation { get; set; }
        public Dish MainDish { get; set; }
        public Dish Second { get; set; }
        public Dish Salad { get; set; }
        public Dish Drink { get; set; }
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


    public class Dish
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfCreation { get; set; }
        public DishCategory DishCategory { get; set; }
        public string DishCategoryId { get; set; }
        public bool HasGarnish { get; set; }
        public string GarnishId { get; set; }
        public Dish Garnish { get; set; }
        public double Proteins { get; set; }
        public double Fats { get; set; }
        public double Carbonhydrates { get; set; }
        public double Kilocalories { get; set; }
        public string ImagePath { get; set; }
        public double Price { get; set; }

    }

    public enum OrderStatus
    {
        Waiting,
        Done,
        Cancelled
    }
}