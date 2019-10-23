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
using System.IO;
using static FoodOrder.Controllers.ManageController;

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

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        // Get: Users/MyDetails
        [Authorize]
        public async Task<ActionResult> MyDetails(ManageMessageId? message)
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Where(c => c.Id == userId).SingleOrDefault();


            if (await UserManager.IsInRoleAsync(userId, "admin"))
            {
                var companies = await db.Companies.Include(c => c.Representative).ToListAsync();
                return View("MyDetailsAdmin", companies);
            }
            if (await UserManager.IsInRoleAsync(userId, "representative"))
            {
                ViewBag.StatusMessage =
                       message == ManageMessageId.ChangePasswordSuccess ? "Ваш пароль изменен."
                       : "";

                var companyId = (await db.Companies.Where(c => c.RepresentativeId == userId).FirstOrDefaultAsync()).Id;
                var employees = await db.Users.Include(u => u.Company).Where(u => u.CompanyId == companyId && u.NotActual == false).ToListAsync();

                var employeesViewModel = employees.Select(p => new UserViewModel()
                {
                    Id = p.Id,
                    UserName = p.UserName,
                    Name = p.Name,
                    Surname = p.Surname,
                    Patronymic = p.Patronymic,
                    Age = p.Age,
                    PhoneNumber = p.PhoneNumber,
                    Email = p.Email,
                    EmailConfirmed = p.EmailConfirmed,
                    HasOrderToday = IsOrderCheck(p),
                    Company = p.Company
                });

                return View("MyDetailsCompany", employeesViewModel);
            }
            if (await UserManager.IsInRoleAsync(userId, "cook"))
            {
                var dishes = await db.Dishes.Include(d=>d.Garnish).ToListAsync();
                var garnishes = dishes.Where(d => d.TypeOfDish == TypeOfDish.Garnish);
                ViewData["Dishes"] = dishes;
                ViewData["Garnishes"] = garnishes;
                return View("MyDetailsCook");
            }
            if (await UserManager.IsInRoleAsync(userId, "employee"))
            {
                return View("MyDetailsEmployee");
            }

            return RedirectToAction("Index", "Home");
        }

        private bool IsOrderCheck(User p)
        {
            return db.Orders.Where(u => u.UserId == p.Id).Any(CheckDate());
        }

        private static System.Linq.Expressions.Expression<Func<Order, bool>> CheckDate()
        {
            return u => u.DateOfCreation == DateTime.Today;
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
                    TypeOfPayment = company.TypeOfPayment,
                    UnlimitedOrders = company.UnlimitedOrders,
                    Description = company.Description,
                    Whatsapp = company.Whatsapp,
                    Telegram = company.Telegram,
                    CompanyImagePath = company.CompanyImagePath,
                    FullName = company.FullName,
                    BIN_IIN = company.BIN_IIN,
                    IIK = company.IIK,
                    RNN = company.RNN,
                    BIK = company.BIK,
                    Bank = company.Bank,
                    LegalAddress = company.LegalAddress,
                    ActualAddress = company.ActualAddress,
                    Director = company.Director,
                    PhoneNumber = company.PhoneNumber,
                    Email = company.Email
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
                    Name = uvm.Name,
                    Surname = uvm.Surname,
                    Patronymic = uvm.Patronymic,
                    Age = uvm.Age,
                    RegistrationDate = DateTime.Now,
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

            string fileName = Path.GetFileName(cvm.CompanyImageFile?.FileName);
            string path = Server.MapPath("~/Files/" + fileName);

            cvm.Id = Guid.NewGuid().ToString();
            var newRandomPassword = CreateRandomPassword();
            cvm.GeneratedPassword = newRandomPassword;
            if (ModelState.IsValid)
            {
                var dateTimeNow = DateTime.Now;
                var company = new Company
                {
                    Id = cvm.Id,
                    Name = cvm.Name,
                    TypeOfPayment = cvm.TypeOfPayment,
                    UnlimitedOrders = cvm.UnlimitedOrders,
                    RegistrationDate = dateTimeNow,
                    Whatsapp = cvm?.Whatsapp,
                    Telegram = cvm?.Telegram,
                    Description = cvm?.Description,
                    GeneratedPassword = cvm.GeneratedPassword,
                    CompanyImagePath = path,
                    FullName = cvm.FullName,
                    BIN_IIN = cvm.BIN_IIN,
                    IIK = cvm.IIK,
                    RNN = cvm.RNN,
                    BIK = cvm.BIK,
                    Bank = cvm.Bank,
                    LegalAddress = cvm.LegalAddress,
                    ActualAddress = cvm.ActualAddress,
                    Director = cvm.Director,
                    PhoneNumber = cvm.PhoneNumber,
                    Email = cvm.Email
                };

                cvm.CompanyImageFile.SaveAs(path);

                var newRepresentative = new User
                {
                    Email = cvm.RepresentativeEmail,
                    UserName = cvm.RepresentativeEmail,
                    // CompanyId = cvm.Id,
                    RegistrationDate = dateTimeNow
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

            var euvm = new EditUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Name = user.Name,
                Surname = user.Surname,
                Patronymic = user.Patronymic,
                Age = user.Age,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
            return View(euvm);
        }

        // POST: Users/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,UserName,Name,SecondName,Patronymic,Age,Email,PhoneNumber")] EditUserViewModel uvm)
        {
            if (ModelState.IsValid)
            {
                var user = await db.Users.Where(u => u.Id == uvm.Id).FirstOrDefaultAsync();
                user.UserName = uvm.UserName;
                user.Name = uvm.Name;
                user.Surname = uvm.Surname;
                user.Patronymic = uvm.Patronymic;
                user.Age = uvm.Age;
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
                TypeOfPayment = company.TypeOfPayment,
                UnlimitedOrders = company.UnlimitedOrders,
                Description = company.Description,
                Whatsapp = company.Whatsapp,
                Telegram = company.Telegram,
                CompanyImagePath = company.CompanyImagePath,
                FullName = company.FullName,
                BIN_IIN = company.BIN_IIN,
                IIK = company.IIK,
                RNN = company.RNN,
                BIK = company.BIK,
                Bank = company.Bank,
                LegalAddress = company.LegalAddress,
                ActualAddress = company.ActualAddress,
                Director = company.Director,
                PhoneNumber = company.PhoneNumber,
                Email = company.Email
            };
            return View(cvm);
        }

        // POST: Users/EditCompany/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditCompany([Bind(Include = 
            "Id,Name,TypeOfPayment,UnlimitedOrders,Description,Whatsapp,Telegram,CompanyImagePath,CompanyImageFile," +
            "FullName,BIN_IIN, IIK, RNN,BIK,Bank,LegalAddress,ActualAddress,Director,PhoneNumber,Email")] CompanyViewModel cvm)
        {
            string fileName = Path.GetFileName(cvm.CompanyImageFile?.FileName);
            string path = Server.MapPath("~/Files/" + fileName);

            if (ModelState.IsValid)
            {
                var company = await db.Companies.Where(c => c.Id == cvm.Id).FirstOrDefaultAsync();
                company.Name = cvm.Name;
                company.TypeOfPayment = cvm.TypeOfPayment;
                company.UnlimitedOrders = cvm.UnlimitedOrders;
                company.Description = cvm.Description;
                company.Whatsapp = cvm.Whatsapp;
                company.Telegram = cvm.Telegram;
                company.CompanyImagePath = path;
                company.FullName = cvm.FullName;
                company.BIN_IIN = cvm.BIN_IIN;
                company.IIK = cvm.IIK;
                company.RNN = cvm.RNN;
                company.BIK = cvm.BIK;
                company.Bank = cvm.Bank;
                company.LegalAddress = cvm.LegalAddress;
                company.ActualAddress = cvm.ActualAddress;
                company.Director = cvm.Director;
                company.PhoneNumber = cvm.PhoneNumber;
                company.Email = cvm.Email;
                db.Entry(company).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("MyDetails", "Users");
            }

            cvm.CompanyImageFile.SaveAs(path);

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
                Name = user.Name,
                Surname = user.Surname,
                Patronymic = user.Patronymic,
                Email = user.Email
            };
            return View(uvm);
        }

        // POST: Users/DeleteConfirmed/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            var user = await db.Users.Where(c => c.Id == id).FirstOrDefaultAsync();
            user.NotActual = true;
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
                RegistrationDate = company.RegistrationDate,
                CompanyImagePath = company.CompanyImagePath,
                FullName = company.FullName,
                BIN_IIN = company.BIN_IIN,
                IIK = company.IIK,
                RNN = company.RNN,
                BIK = company.BIK,
                Bank = company.Bank,
                LegalAddress = company.LegalAddress,
                ActualAddress = company.ActualAddress,
                Director = company.Director,
                PhoneNumber = company.PhoneNumber,
                Email = company.Email
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
