using System;
using System.Collections.Generic;
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
        public ICollection<ComplexDish> Dishes { get; set; }
        public Order()
        {
            Dishes = new List<ComplexDish>();
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
        public Dish MainDish { get; set; }
        public Dish Second { get; set; }
        public Dish Salad { get; set; }
        public Dish Drink { get; set; }
    }

    public class Dish
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public TypeOfDish TypeOfDish { get; set; }
        public NutritionalValue NutritionalValue { get; set; }
        public string ImagePath { get; set; }
        public bool HasGarnish { get; set; }
        public double Price { get; set; }
    }

    public class ComplexDish: Dish
    {
        public Dish Garnish { get; set; }
        public ComplexDish()
        {
            HasGarnish = true;
        }
    }

    public class NutritionalValue
    {
        public double Proteins { get; set; }
        public double Fats { get; set; }
        public double Carbonhydrates { get; set; }
        public double Kilocalories { get; set; }
    }

    public enum TypeOfDish
    {
        First,
        Second,
        Garnish,
        Salad,
        Drink,
        Snack
    }

    public enum OrderStatus
    {
        Waiting,
        Done,
        Cancelled
    }
}