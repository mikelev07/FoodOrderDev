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

namespace FoodOrder.Controllers
{
    public class UsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Users
        public async Task<ActionResult> Index()
        {
            return View(await db.Users.ToListAsync());
        }

        // Get: Users/MyDetails
        [Authorize]
        public async Task<ActionResult> MyDetails()
        {
            var username = User.Identity.GetUserName();
            if (Roles.IsUserInRole(username, "admin"))
            {
                var companiesNames = Roles.GetUsersInRole("company");
                var companies = await db.Users.Where(m => companiesNames.Contains(m.UserName)).ToListAsync();
                return View("MyDetailsAdmin", companies);
            }
            if (Roles.IsUserInRole(username, "company"))
            {
                return View("MyDetailsCompany");
            }
            if (Roles.IsUserInRole(username, "cook"))
            {
                return View("MyDetailsCook");
            }
            if (Roles.IsUserInRole(username, "employee"))
            {
                return View("MyDetailsEmployee");
            }
            /*if (HttpContext.User.IsInRole("admin"))
            {
                var companiesNames = Roles.GetUsersInRole("company");
                var companies = await db.Users.Where(m => companiesNames.Contains(m.UserName)).ToListAsync();
                return View("MyDetailsAdmin", companies);
            }
            if (HttpContext.User.IsInRole("company"))
            {
                return View("MyDetailsCompany");
            }
            if (HttpContext.User.IsInRole("cook"))
            {
                return View("MyDetailsCook");
            }
            if (HttpContext.User.IsInRole("employee"))
            {
                return View("MyDetailsEmployee");
            }*/

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
            return View(user);
        }

        // GET: Users/CreateCompany
        [Authorize(Roles = "admin")]
        public ActionResult CreateCompany()
        {
            return View();
        }

        // POST: Users/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateCompany([Bind(Include = "Id,SecondName,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        [Authorize(Roles = "admin, company")]
        public async Task<ActionResult> EditCompany(string id)
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

        // POST: Users/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditCompany([Bind(Include = "Id,SecondName,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(user);
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
        [HttpPost, ActionName("Delete")]
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
