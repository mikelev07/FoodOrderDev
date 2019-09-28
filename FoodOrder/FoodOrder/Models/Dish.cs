﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodOrder.Models
{
    public class Menu
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfCreation { get; set; }
        public ICollection<ComplexDish> Dishes { get; set; }
        public Menu()
        {
            Dishes = new List<ComplexDish>();
        }
    }
    public class Dish
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public TypeOfDish TypeOfDish { get; set; }
        public NutritionalValue NutritionalValue { get; set; }
        public string ImagePath { get; set; }
        public bool HasGarnish { get; set; }
    }

    public class ComplexDish: Dish
    {
        public Dish GetDish { get; set; }
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
}