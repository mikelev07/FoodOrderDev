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
using Microsoft.AspNet.Identity.Owin;

namespace FoodOrder.Controllers
{
    public class OrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        public OrdersController()
        {

        }
        public OrdersController(ApplicationUserManager userManager)
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

        [Authorize(Roles = "admin,representative,cook, employee")]
        // GET: Orders
        public async Task<ActionResult> Index()
        {
            var userId = User.Identity.GetUserId();
            if (userId == null)
            {
                return new HttpNotFoundResult();
            }

            if (await UserManager.IsInRoleAsync(userId, "admin"))
            {
                var companies = await db.Companies.ToListAsync();
                return View("IndexAdmin", companies);
            }
            if (await UserManager.IsInRoleAsync(userId, "representative"))
            {
                return View("IndexCompany");
            }
            if (await UserManager.IsInRoleAsync(userId, "employee"))
            {
                var menu = db.Menus.Include(m => m.Dishes).Where(c=>c.DateOfCreation.Day == DateTime.Now.Day).FirstOrDefault();
                var categories = await db.DishCategories.Include(d => d.Dishes).ToListAsync();

                if (menu != null)
                {
                    string[] hs = menu.Dishes.Select(c => c.Id).ToArray();

                    var dishCategories = categories.Select(c => new DishCategory()
                    {
                        Id = c.Id,
                        Name = c.Name,
                        DateOfCreation = c.DateOfCreation,
                        Dishes = c.Dishes.Where(b => hs.Contains(b.Id)).ToList()
                    });

                    MenuViewModel menuViewModel = new MenuViewModel
                    {
                        Menu = menu,
                        DishCategories = dishCategories.ToList()
                    };

                    return View("IndexEmployee", menuViewModel);
                }
                else
                {
                    return View("IndexEmployee");
                }
            }
            if (await UserManager.IsInRoleAsync(userId, "cook"))
            {
                var orders = await db.Orders.Include(o => o.User).ToListAsync();
                return View("IndexCook", orders);
            }

            return new HttpUnauthorizedResult();
        }

        //метод для отображения заказов по сотрудникам
        [Authorize(Roles = "admin,representative,cook")]
        public async Task<ActionResult> Employees(string companyId)
        {
            if (companyId == null)
            {
                return new HttpNotFoundResult("Such a company not found");
            }
            var employees = await db.Users.Include(u=>u.Orders).Where(u => u.CompanyId == companyId).ToListAsync();
            ViewBag.CompanyName = (await db.Companies.FirstOrDefaultAsync(c => c.Id == companyId)).Name;
            return View("IndexCompany", employees);
        }

        // GET: Orders/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            //когда дойдут руки (и другие части тела) до создания заказа
            //нужно будет инкрементить число заказов у компании
            ViewBag.UserId = new SelectList(db.Users, "Id", "SecondName");
            return View();
        }

        // POST: Orders/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,DateOfCreation,Description,UserId")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.Users, "Id", "SecondName", order.UserId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "SecondName", order.UserId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,DateOfCreation,Description,UserId")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "SecondName", order.UserId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            Order order = await db.Orders.FindAsync(id);
            db.Orders.Remove(order);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
