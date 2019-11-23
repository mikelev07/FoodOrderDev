using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public TypeOfPayment TypeOfPayment { get; set; }
        public bool UnlimitedOrders { get; set; }
        public bool SingleChoice { get; set; }
        public bool MultiChoice { get; set; }
        public bool PacksPicker { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Description { get; set; }
        public string GeneratedPassword { get; set; }
        public string RepresentativeEmail { get; set; }
        public string Whatsapp { get; set; }
        public string Telegram { get; set; }
        public bool? IsVisible { get; set; }
        public string CompanyImagePath { get; set; }
        [NotMapped]
        public HttpPostedFileBase CompanyImageFile { get; set; }
        //public Requisites Requisites { get; set; }
        public string FullName { get; set; }

        /// <summary>БИН или ИИН</summary>
        public string BIN_IIN { get; set; }

        /// <summary>ИИК</summary>
        public string IIK { get; set; }

        /// <summary>РНН</summary>
        public string RNN { get; set; }

        /// <summary>БИК</summary>
        public string BIK { get; set; }

        /// <summary>Банковские реквизиты</summary>
        public string Bank { get; set; }

        /// <summary>Юридический адрес</summary>
        public string LegalAddress { get; set; }

        /// <summary>Фактический адрес</summary>
        public string ActualAddress { get; set; }

        /// <summary>Директор</summary>
        public string Director { get; set; }

        /// <summary>Телефон</summary>
        public string PhoneNumber { get; set; }

        /// <summary>Электронная почта</summary>
        public string Email { get; set; }
        //public CompanyViewModel()
        //{
        //    Requisites = new Requisites();
        //}
    }
    public class Company
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool? IsVisible { get; set; }
        public TypeOfPayment TypeOfPayment { get; set; }
        public bool UnlimitedOrders { get; set; }
        public bool SingleChoice { get; set; }
        public bool MultiChoice { get; set; }
        public bool PacksPicker { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Whatsapp { get; set; }
        public string Telegram { get; set; }
        public string Description { get; set; }
        public string GeneratedPassword { get; set; }
        public string RepresentativeId { get; set; }
        public virtual User Representative { get; set; }
        public List<User> Employees { get; set; }
        public int OrdersCount { get; set; }
        public string CompanyImagePath { get; set; }
        [NotMapped]
        public HttpPostedFileBase CompanyImageFile { get; set; }

        //public Requisites Requisites { get; set; }
        public string FullName { get; set; }

        /// <summary>БИН или ИИН</summary>
        public string BIN_IIN { get; set; }

        /// <summary>ИИК</summary>
        public string IIK { get; set; }

        /// <summary>РНН</summary>
        public string RNN { get; set; }

        /// <summary>БИК</summary>
        public string BIK { get; set; }

        /// <summary>Банковские реквизиты</summary>
        public string Bank { get; set; }

        /// <summary>Юридический адрес</summary>
        public string LegalAddress { get; set; }

        /// <summary>Фактический адрес</summary>
        public string ActualAddress { get; set; }

        /// <summary>Директор</summary>
        public string Director { get; set; }

        /// <summary>Телефон</summary>
        public string PhoneNumber { get; set; }

        /// <summary>Электронная почта</summary>
        public string Email { get; set; }

      

        public Company()
        {
            //Requisites = new Requisites();
            Employees = new List<User>();
        }
    }

    //public class Requisites
    //{
    //    public string Id { get; set; }
    //    /// <summary>Полное наименование</summary>
    //    public string FullName { get; set; }

    //    /// <summary>БИН или ИИН</summary>
    //    public string BIN_IIN { get; set; }

    //    /// <summary>ИИК</summary>
    //    public string IIK { get; set; }

    //    /// <summary>РНН</summary>
    //    public string RNN { get; set; }

    //    /// <summary>БИК</summary>
    //    public string BIK { get; set; }

    //    /// <summary>Банковские реквизиты</summary>
    //    public string Bank { get; set; }

    //    /// <summary>Юридический адрес</summary>
    //    public string LegalAddress { get; set; }

    //    /// <summary>Фактический адрес</summary>
    //    public string ActualAddress { get; set; }

    //    /// <summary>Директор</summary>
    //    public string Director { get; set; }

    //    /// <summary>Телефон</summary>
    //    public string PhoneNumber { get; set; }

    //    /// <summary>Электронная почта</summary>
    //    public string Email { get; set; }
    //}


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