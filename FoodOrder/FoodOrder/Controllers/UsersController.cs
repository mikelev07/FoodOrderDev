﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FoodOrder.Models;
using Microsoft.AspNet.Identity;
using System.Web.Security;
using Microsoft.AspNet.Identity.Owin;

namespace FoodOrder.Controllers
{
    public class UsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        public UsersController()
        {
          
        }
        public UsersController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); ;
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Users
        public async Task<ActionResult> Index()
        {
            return View(await db.Users.ToListAsync());
        }

        // Get: Users/MyDetails
        [Authorize]
        public async Task<ActionResult> MyDetails()
        {
            var userId = User.Identity.GetUserId();

            if (await UserManager.IsInRoleAsync(userId, "admin"))
            {
                var companies = await db.Companies.ToListAsync();
                return View("MyDetailsAdmin", companies);
            }
            if (await UserManager.IsInRoleAsync(userId, "representative"))
            {
                var company = await db.Companies.Where(c => c.RepresentativeId == userId).FirstOrDefaultAsync();
                var employees = company.Employees.ToList();
                ViewBag.CompanyName = company.Name;
                return View("MyDetailsCompany", employees);
            }
            if (await UserManager.IsInRoleAsync(userId, "cook"))
            {
                return View("MyDetailsCook");
            }
            if (await UserManager.IsInRoleAsync(userId, "employee"))
            {
                return View("MyDetailsEmployee");
            }

            return RedirectToAction("Index", "Home");
        }

        // GET: Users/Details/5
        [Authorize]
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.Where(i => id == i.Id).FirstOrDefaultAsync();
            if (user == null)
            {
                return HttpNotFound();
            }


            var userId = user.Id;
            if (await UserManager.IsInRoleAsync(userId, "representative"))
            {
                var company = await db.Companies.Where(c => c.RepresentativeId == userId).FirstOrDefaultAsync();
                return View("DetailsCompany", company);
            }
            if (await UserManager.IsInRoleAsync(userId, "cook"))
            {
                return View("DetailsCook", user);
            }
            if (await UserManager.IsInRoleAsync(userId, "employee"))
            {
                return View("DetailsEmployee", user);
            }

            return RedirectToAction("Index", "Home");
        }


        // GET: Users/Create
        [Authorize(Roles = "representative")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserViewModel uvm)
        {
            //пока так 
            if (/*ModelState.IsValid*/true)
            {
                var newRandomPassword = CreateRandomPassword();
                var user = new User {
                    UserName = uvm.Email,
                    SecondName = uvm.SecondName,
                    Email = uvm.Email,
                    PhoneNumber = uvm.PhoneNumber
                };

                var result = await UserManager.CreateAsync(user, newRandomPassword);
                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user.Id, "employee");
                    return RedirectToAction("MyDetails", "Users");
                }
            }

            return View(uvm);
        }

        // GET: Users/CreateCompany
        [Authorize(Roles = "admin")]
        public ActionResult CreateCompany()
        {
            return View();
        }

        // POST: Users/CreateCompany
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateCompany(CompanyViewModel cvm)
        {
            if (ModelState.IsValid)
            {
                var company = new Company {
                    Name = cvm.Name,
                    Logotype = cvm.Logotype,
                    TypeOfPayment = cvm.TypeOfPayment,
                    UnlimitedOrders = cvm.UnlimitedOrders
                };

                var newRandomPassword = CreateRandomPassword();
                //куда в юзера логин засунуть???
                //создать на вьюхе могу, но как его передать в нового юзера - хз
                var newRepresentative = new User();

                var result = await UserManager.CreateAsync(newRepresentative, newRandomPassword);
                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(newRepresentative.Id, "representative");
                    company.Representative = newRepresentative;
                    return RedirectToAction("MyDetails", "Users");
                }
            }

            return View(cvm);
        }


        // GET: Users/Edit/5
        [Authorize(Roles = "admin, representative")]
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.Where(i => id == i.Id).FirstOrDefaultAsync();
            if (user == null)
            {
                return HttpNotFound();
            }

            var uvm = new UserViewModel
            {
                UserName = user.UserName,
                SecondName = user.SecondName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
            return View(uvm);
        }

        // POST: Users/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "UserName,SecondName,Email,PhoneNumber")] UserViewModel uvm)
        {
            if (ModelState.IsValid)
            {
                var user = await db.Users.Where(u => u.Email == uvm.Email).FirstOrDefaultAsync();
                user.UserName = uvm.UserName;
                user.SecondName = uvm.SecondName;
                user.Email = uvm.Email;
                user.PhoneNumber = uvm.PhoneNumber;
                db.Entry(user).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("MyDetails", "Users");
            }
            return View(uvm);
        }

        // GET: Users/EditCompany/5
        [Authorize(Roles = "admin, representative")]
        public async Task<ActionResult> EditCompany(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var company = await db.Companies.Where(i => i.Id == id).FirstOrDefaultAsync();
            if (company == null)
            {
                return HttpNotFound();
            }
            var cvm = new CompanyViewModel
            {
                Name = company.Name,
                Logotype = company.Logotype,
                TypeOfPayment = company.TypeOfPayment,
                UnlimitedOrders = company.UnlimitedOrders,
                //откуда взять логин у юзера?? та же проблема при создании компании: см. CreateCompany
                //пока захардкодил
                RepresentativeLogin = "pisos"
            };
            return View(cvm);
        }

        // POST: Users/EditCompany/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditCompany([Bind(Include = "Name,Logotype,TypeOfPayment,UnlimitedOrders")] CompanyViewModel cvm)
        {
            if (ModelState.IsValid)
            {
                var company = await db.Companies.Where(c => c.Name == cvm.Name).FirstOrDefaultAsync();
                company.Name = cvm.Name;
                company.Logotype = cvm.Logotype;
                company.TypeOfPayment = cvm.TypeOfPayment;
                company.UnlimitedOrders = cvm.UnlimitedOrders;
                db.Entry(company).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("MyDetails", "Users");
            }
            return View(cvm);
        }

        // GET: Users/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> DeleteCompany(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.Where(i => id == i.Id).FirstOrDefaultAsync();
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("DeleteCompany")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteCompanyConfirmed(string id)
        {
            User user = await db.Users.Where(i => id == i.Id).FirstOrDefaultAsync();
            db.Users.Remove(user);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public string CreateRandomPassword()
        {
            return "sosiPisos";
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
