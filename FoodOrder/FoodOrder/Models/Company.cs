using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FoodOrder.Models
{
    public class CompanyViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string LogotypePath { get; set; } /*= "~/Content/Custom/images/some-company-logotype.jpg";*/
        //public HttpPostedFileBase LogotypeFile { get; set; }
        public TypeOfPayment TypeOfPayment { get; set; }
        public bool UnlimitedOrders { get; set; }
        public string Description { get; set; }
        public string GeneratedPassword { get; set; }
        public string RepresentativeLogin { get; set; }
        public string Requisites { get; set; }
        public string Whatsapp { get; set; }
        public string Telegram { get; set; }
        public bool? IsVisible { get; set; }
        public string CompanyImagePath { get; set; }
        [NotMapped]
        public HttpPostedFileBase CompanyImageFile { get; set; }
    }
    public class Company
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool? IsVisible { get; set; }
        public string LogotypePath { get; set; } /*= "~/Content/Custom/images/some-company-logotype.jpg";*/
        //public HttpPostedFileBase LogotypeFile { get; set; }
        public TypeOfPayment TypeOfPayment { get; set; }
        public bool UnlimitedOrders { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Whatsapp { get; set; }
        public string Telegram { get; set; }
        public string Description { get; set; }
        public string GeneratedPassword { get; set; }
        public string Requisites { get; set; }
        public string RepresentativeId { get; set; }
        public virtual User Representative { get; set; }
        public List<User> Employees { get; set; }
        public int OrdersCount { get; set; }
        public string CompanyImagePath { get; set; }
        [NotMapped]
        public HttpPostedFileBase CompanyImageFile { get; set; }

        public Company()
        {
            Employees = new List<User>();
        }
    }


    public enum TypeOfPayment
    {
        [Display(Name = "Наличный расчет")]
        Cash,
        [Display(Name = "Банковский перевод")]
        Bank,
        [Display(Name = "Электронные деньги")]
        ElectronicMoney
    }
}