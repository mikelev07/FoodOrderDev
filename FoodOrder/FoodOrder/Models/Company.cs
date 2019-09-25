using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FoodOrder.Models
{
    public class CompanyViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Logotype { get; set; } /*= "~/Content/Custom/images/some-company-logotype.jpg";*/
        public TypeOfPayment TypeOfPayment { get; set; }
        public bool UnlimitedOrders { get; set; }
        public string RepresentativeLogin { get; set; }
    }
    public class Company
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Logotype { get; set; } /*= "~/Content/Custom/images/some-company-logotype.jpg";*/
        public TypeOfPayment TypeOfPayment { get; set; }
        public bool UnlimitedOrders { get; set; }
        public string RepresentativeId { get; set; }
        public virtual User Representative { get; set; }
        public List<User> Employees { get; set; }

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