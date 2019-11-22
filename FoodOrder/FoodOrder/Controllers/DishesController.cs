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
using System.IO;

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
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,DishCategory,NutritionalValue,ImagePath,HasGarnish")] Dish dish)
        {
            if (ModelState.IsValid)
            {
                dish.DateOfCreation = DateTime.UtcNow;
                db.Dishes.Add(dish);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(dish);
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

        //проверка, существует ли уже в бд блюдо с таким названием
        public async Task<JsonResult> DishAlreadyExists(string dishName)
        {
            var dishAlreadyExist = await db.Dishes.AnyAsync(d => d.Name == dishName);
            return Json(dishAlreadyExist, JsonRequestBehavior.AllowGet);
        }


        public async Task<ActionResult> CreatePack(string[] idDs)
        {
            Pack pack = new Pack();

            var dishes = await db.Dishes.Where(b => idDs.Contains(b.Id)).ToListAsync();
            pack.Id = Guid.NewGuid().ToString("N");
            pack.DateOfCreation = DateTime.Now;
            pack.Dishes = dishes;

            db.Packs.Add(pack);
            db.SaveChanges();

            return Json(true, JsonRequestBehavior.AllowGet);
        }

            public async Task<ActionResult> CreateJson()
        {
            string dbImagePath;
            if (Request.Files.Get(0).FileName != null)
            {
                string fileName = Path.GetFileName(Request.Files.Get(0).FileName);
                string path = Server.MapPath("~/Files/" + fileName);
                Request.Files.Get(0).SaveAs(path);
                dbImagePath = "~/Files/" + fileName;
            } else {
                dbImagePath = "~/Content/img/1.jpg";
            }

            string name = Request.Form.Get("name");
            string selectedType = Request.Form.Get("selectedType");
            bool hasGarnish = Convert.ToBoolean(Request.Form.Get("hasGarnish"));
            double proteins = Convert.ToDouble(Request.Form.Get("proteins").Replace('.', ','));
            double fats = Convert.ToDouble(Request.Form.Get("fats").Replace('.', ','));
            double carbonhydrates = Convert.ToDouble(Request.Form.Get("carbonhydrates").Replace('.', ','));
            double kilocalories = Convert.ToDouble(Request.Form.Get("kilocalories").Replace('.', ','));

            var dateOfCreation = DateTime.UtcNow;
            var category = await db.DishCategories.Where(d => d.Id == selectedType).FirstOrDefaultAsync();
            var categoryName = category.Name;

            Dish dish = new Dish()
            {
                Id = Guid.NewGuid().ToString("N"),
                Name = name,
                DateOfCreation = dateOfCreation,
                DishCategoryId = selectedType,
                ImagePath = dbImagePath,
                HasGarnish = hasGarnish,
                Proteins = proteins,
                Fats = fats,
                Carbonhydrates = carbonhydrates,
                Kilocalories = kilocalories
            };

           

            db.Dishes.Add(dish);
            await db.SaveChangesAsync();

            return Json(new
            {
                id = dish.Id,
                path = dish.ImagePath,
                dateOfCreation
            }, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> EditJson()
        {
            string dbImagePath;
            if (Request.Files.Get(0).FileName != null)
            {
                string fileName = Path.GetFileName(Request.Files.Get(0).FileName);
                string path = Server.MapPath("~/Files/" + fileName);
                Request.Files.Get(0).SaveAs(path);
                dbImagePath = "~/Files/" + fileName;
            }
            else
            {
                dbImagePath = "~/Content/img/1.jpg";
            }

            string id = Request.Form.Get("id");
            string name = Request.Form.Get("name");
            string selectedType = Request.Form.Get("selectedType");
            bool hasGarnish = Convert.ToBoolean(Request.Form.Get("hasGarnish"));
            double proteins = Convert.ToDouble(Request.Form.Get("proteins").Replace('.', ','));
            double fats = Convert.ToDouble(Request.Form.Get("fats").Replace('.', ','));
            double carbonhydrates = Convert.ToDouble(Request.Form.Get("carbonhydrates").Replace('.', ','));
            double kilocalories = Convert.ToDouble(Request.Form.Get("kilocalories").Replace('.', ','));

            var dish = await db.Dishes.Where(d => d.Id == id).FirstOrDefaultAsync();
            var category = await db.DishCategories.Where(d => d.Id == selectedType).FirstOrDefaultAsync();
            var categoryName = category.Name;

            db.Entry(dish).State = EntityState.Modified;

            dish.Name = name;
            dish.DishCategoryId = category.Id;
            dish.HasGarnish = hasGarnish;
            dish.Proteins = proteins;
            dish.Fats = fats;
            dish.Carbonhydrates = carbonhydrates;
            dish.Kilocalories = kilocalories;
            dish.ImagePath = dbImagePath;

            await db.SaveChangesAsync();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> DeleteJson(string id)
        {
            var dish = await db.Dishes.Where(d => d.Id == id).FirstOrDefaultAsync();
            db.Dishes.Remove(dish);
            await db.SaveChangesAsync();
            return Json(true, JsonRequestBehavior.AllowGet);
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
