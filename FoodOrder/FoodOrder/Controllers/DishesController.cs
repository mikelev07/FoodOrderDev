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

namespace FoodOrder.Controllers
{
    public class DishesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Dishes
        public async Task<ActionResult> Index()
        {
            return View(await db.Dishes.ToListAsync());
        }

        // GET: Dishes/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dish dish = await db.Dishes.FindAsync(id);
            if (dish == null)
            {
                return HttpNotFound();
            }
            return View(dish);
        }

        // GET: Dishes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Dishes/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,TypeOfDish,NutritionalValue,ImagePath,HasGarnish")] Dish dish)
        {
            if (ModelState.IsValid)
            {
                db.Dishes.Add(dish);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(dish);
        }

        public JsonResult CreateJson(string name, int selectedType, bool hasGarnish)
        {
            Dish dish = new Dish()
            {
                Id = Guid.NewGuid().ToString("N"),
                Name = name,
                TypeOfDish = (TypeOfDish)selectedType
            };
            db.Dishes.Add(dish);
            db.SaveChanges();

            var type = Helpers.HelperExtensions.GetDisplayName((TypeOfDish)selectedType);

            return Json(new { name, type, hasGarnish }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDishes()
        {
            var dishList = db.Dishes.AsEnumerable().Select(d => new
            {
                name = d.Name,
                type = Helpers.HelperExtensions.GetDisplayName((TypeOfDish) d.TypeOfDish),
                hasGarnish = d.HasGarnish
            }).ToList();
            return Json(dishList, JsonRequestBehavior.AllowGet);
        }

        // GET: Dishes/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dish dish = await db.Dishes.FindAsync(id);
            if (dish == null)
            {
                return HttpNotFound();
            }
            return View(dish);
        }

        // POST: Dishes/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,TypeOfDish,NutritionalValue,ImagePath,HasGarnish")] Dish dish)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dish).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(dish);
        }

        // GET: Dishes/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dish dish = await db.Dishes.FindAsync(id);
            if (dish == null)
            {
                return HttpNotFound();
            }
            return View(dish);
        }

        // POST: Dishes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            Dish dish = await db.Dishes.FindAsync(id);
            db.Dishes.Remove(dish);
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
