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

namespace FoodOrder.Controllers
{
    public class DishCategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DishCategories
        public async Task<ActionResult> Index()
        {
            return View(await db.DishCategories.ToListAsync());
        }

        // GET: DishCategories/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DishCategory dishCategory = await db.DishCategories.FindAsync(id);
            if (dishCategory == null)
            {
                return HttpNotFound();
            }
            return View(dishCategory);
        }

        // GET: DishCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DishCategories/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name")] DishCategory dishCategory)
        {
            if (ModelState.IsValid)
            {
                db.DishCategories.Add(dishCategory);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(dishCategory);
        }

        // GET: DishCategories/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DishCategory dishCategory = await db.DishCategories.FindAsync(id);
            if (dishCategory == null)
            {
                return HttpNotFound();
            }
            return View(dishCategory);
        }

        // POST: DishCategories/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name")] DishCategory dishCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dishCategory).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(dishCategory);
        }

        // GET: DishCategories/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DishCategory dishCategory = await db.DishCategories.FindAsync(id);
            if (dishCategory == null)
            {
                return HttpNotFound();
            }
            return View(dishCategory);
        }

        // POST: DishCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            DishCategory dishCategory = await db.DishCategories.FindAsync(id);
            db.DishCategories.Remove(dishCategory);
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
