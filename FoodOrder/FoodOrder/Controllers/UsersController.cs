using System;
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
using System.Data.Entity.Validation;

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
                var companies = await db.Companies.Include(c => c.Representative).ToListAsync();
                return View("MyDetailsAdmin", companies);
            }
            if (await UserManager.IsInRoleAsync(userId, "representative"))
            {
                var company = await db.Companies.Include(c => c.Employees).Where(c => c.RepresentativeId == userId).FirstOrDefaultAsync();
                var employees = company.Employees.ToList();
                ViewBag.CompanyName = company.Name;

                var employeesViewModel = (from employee in employees
                                          select new
                                          {
                                              employee.Id,
                                              employee.UserName,
                                              employee.Email,
                                              employee.EmailConfirmed
                                          }).ToList().Select(async p => new UserViewModel()
                                          {
                                              Id = p.Id,
                                              UserName = p.UserName,
                                              Email = p.Email,
                                              EmailConfirmed = p.EmailConfirmed,
                                              HasOrderToday = await db.Orders.Where(u => u.UserId == p.Id).AnyAsync(u => DateTime.Compare(u.DateOfCreation.Date, DateTime.Today) == 0)
                                          });

                return View("MyDetailsCompany", employeesViewModel);
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
                var cvm = new CompanyViewModel
                {
                    Name = company.Name,
                    LogotypePath = company.LogotypePath,
                    TypeOfPayment = company.TypeOfPayment,
                    UnlimitedOrders = company.UnlimitedOrders
                };
                return View("DetailsCompany", cvm);
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

        [Authorize(Roles = "admin")]
        public ActionResult Statistic()
        {
            return View();
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
            uvm.Id = Guid.NewGuid().ToString();
            if (ModelState.IsValid)
            {
                var currentUserId = User.Identity.GetUserId();
                var companyId = (await db.Companies.Where(c => c.RepresentativeId == currentUserId).FirstOrDefaultAsync()).Id;
                var newRandomPassword = CreateRandomPassword();
                var user = new User
                {
                    Id = uvm.Id,
                    UserName = uvm.Email,
                    SecondName = uvm.SecondName,
                    Email = uvm.Email,
                    PhoneNumber = uvm.PhoneNumber,
                    CompanyId = companyId
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
            cvm.Id = Guid.NewGuid().ToString();
            var newRandomPassword = CreateRandomPassword();
            cvm.GeneratedPassword = newRandomPassword;
            if (ModelState.IsValid)
            {
                var company = new Company
                {
                    Id = cvm.Id,
                    Name = cvm.Name,
                    LogotypePath = cvm?.LogotypePath,
                    TypeOfPayment = cvm.TypeOfPayment,
                    UnlimitedOrders = cvm.UnlimitedOrders,
                    Description = cvm?.Description,
                    GeneratedPassword = cvm.GeneratedPassword,
                    Requisites = cvm.Requisites,
                    Whatsapp = cvm?.Whatsapp,
                    Telegram = cvm?.Telegram
                };

                var newRepresentative = new User
                {
                    Email = cvm.RepresentativeLogin,
                    UserName = cvm.RepresentativeLogin
                };
                //нужно ли будет закидывать в представителя CompanyId??
                var result = await UserManager.CreateAsync(newRepresentative, newRandomPassword);
                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(newRepresentative.Id, "representative");
                    company.RepresentativeId = newRepresentative.Id;
                    db.Companies.Add(company);
                    await db.SaveChangesAsync();

                    return RedirectToAction("MyDetails", "Users");
                }
            }

            return View(cvm);
        }

        public async Task<JsonResult> SendSpecEmail(string id, string genPass)
        {
            var res = "Заходите на сайт foodorder.somee.com,Ваш логин на сайте такой же как и ваша почта, Ваш пароль: " + genPass;
            await UserManager.SendEmailAsync(id, "Проверка электронной почты", res);

            return Json(true);
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
                Id = user.Id,
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
        public async Task<ActionResult> Edit([Bind(Include = "Id,UserName,SecondName,Email,PhoneNumber")] UserViewModel uvm)
        {
            if (ModelState.IsValid)
            {
                var user = await db.Users.Where(u => u.Id == uvm.Id).FirstOrDefaultAsync();
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
                Id = company.Id,
                Name = company.Name,
                LogotypePath = company.LogotypePath,
                TypeOfPayment = company.TypeOfPayment,
                UnlimitedOrders = company.UnlimitedOrders
            };
            return View(cvm);
        }

        // POST: Users/EditCompany/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditCompany([Bind(Include = "Id,Name,Logotype,TypeOfPayment,UnlimitedOrders")] CompanyViewModel cvm)
        {
            if (ModelState.IsValid)
            {
                var company = await db.Companies.Where(c => c.Id == cvm.Id).FirstOrDefaultAsync();
                company.Name = cvm.Name;
                company.LogotypePath = cvm.LogotypePath;
                company.TypeOfPayment = cvm.TypeOfPayment;
                company.UnlimitedOrders = cvm.UnlimitedOrders;
                db.Entry(company).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("MyDetails", "Users");
            }
            return View(cvm);
        }

        // GET: Users/Delete/5
        [Authorize(Roles = "admin, representative")]
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await db.Users.Where(c => c.Id == id).FirstOrDefaultAsync();
            if (user == null)
            {
                return HttpNotFound();
            }
            var uvm = new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                SecondName = user.SecondName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
            return View(uvm);
        }

        // POST: Users/DeleteConfirmed/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            var user = await db.Users.Where(c => c.Id == id).FirstOrDefaultAsync();
            db.Users.Remove(user);
            await db.SaveChangesAsync();
            return RedirectToAction("MyDetails", "Users");
        }

        // GET: Users/DeleteCompany/5
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> DeleteCompany(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var company = await db.Companies.Where(c => c.Id == id).FirstOrDefaultAsync();
            if (company == null)
            {
                return HttpNotFound();
            }
            var cvm = new CompanyViewModel
            {
                Id = company.Id,
                Name = company.Name,
                LogotypePath = company.LogotypePath,
                TypeOfPayment = company.TypeOfPayment,
                UnlimitedOrders = company.UnlimitedOrders
            };
            return View(cvm);
        }

        // POST: Users/DeleteCompanyConfirmed/5
        [HttpPost, ActionName("DeleteCompany")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteCompanyConfirmed(string id)
        {
            var company = await db.Companies.Where(c => c.Id == id).FirstOrDefaultAsync();
            db.Companies.Remove(company);
            await db.SaveChangesAsync();
            return RedirectToAction("MyDetails", "Users");
        }

        public string CreateRandomPassword()
        {
            return Membership.GeneratePassword(6, 2);
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
