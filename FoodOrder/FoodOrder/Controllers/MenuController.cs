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
    public class MenuController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Menu
        public async Task<ActionResult> Index()
        {
            return View(await db.Menus.ToListAsync());
        }

        // GET: Menu/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = await db.Menus.FindAsync(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        // GET: Menu/Create
        public ActionResult Create()
        {
            ViewBag.DishCategory = db.DishCategories.Include(d => d.Dishes).ToList();
            DishCategory Granishlist = db.DishCategories.Include(d => d.Dishes).Where(c => c.Id == "ZdesDolzhenBitGarnir").FirstOrDefault();
            ViewBag.Garnishes = Granishlist.Dishes.ToList();
            var l = db.Packs.Include(c => c.Dishes).ToList();
            ViewBag.Packs = l;
            return View();
        }

        // POST: Menu/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,DateOfCreation,Dishes,Packs")] Menu menu, ICollection<string> ints, ICollection<string> intsGar)
        {
            if (ModelState.IsValid)
            {

               var keyValuePairs = ints.Zip(intsGar, (s, i) => new { s, i })
                          .ToDictionary(item => item.s, item => item.i);


                List<Dish> dishes = db.Dishes.Include(c =>c.Garnishes).Where(c => keyValuePairs.Keys.Contains(c.Id)).ToList();


               
                for (var i = 0; i < dishes.Count; i++)
                {
                    var d = dishes[i]; 
                    var garn = keyValuePairs[d.Id]; // цепочка гарниров для блюда
                    var tValues = garn.Split(','); // делаем массив
                    var changeList = db.Dishes.Where(c => tValues.Contains(c.Id)).ToList();
                    var clist = new List<Garnish>();
                    for (var a = 0; a < changeList.Count; a++)
                    {
                        var ret = changeList[a].Id;
                        if (db.Garnishes.Where(c => c.Id == ret).FirstOrDefault()!=null)
                        {
                            clist.Add(db.Garnishes.Where(c => ret == c.Id).FirstOrDefault());
                        }
                        else
                        {
                            var obj = new Garnish();
                            obj.Id = changeList[a].Id;
                            obj.Name = changeList[a].Name;
                            obj.Dishes.Add(d);
                            db.Garnishes.Add(obj);
                            db.SaveChanges();
                            clist.Add(obj);
                        }
                    }
                   // db.Entry(d).State = EntityState.Modified;
                        //d.Garnishes.Clear();
                    d.Garnishes = clist; 
                }
               
                var dishesFix = dishes;


               // List<Dish> garns = db.Dishes.Where(c => intsGar.Contains(c.Id)).ToList();

                /*  var withGarnishes = dishes.Select(y => new Dish
                  {
                      Id = y.Id,
                      Name= y.Name,
                      DateOfCreation = y.DateOfCreation,
                      DishCategory = y.DishCategory,
                      DishCategoryId = y.DishCategoryId,
                      HasGarnish = y.HasGarnish,
                      GarnishId = y.GarnishId,
                      Garnish = y.Garnish,
                      Proteins = y.Proteins,
                      Fats = y.Fats,
                      Carbonhydrates = y.Carbonhydrates,
                      Kilocalories = y.Kilocalories,
                      ImagePath = y.ImagePath,
                      Price = y.Price,
                      Garnishes = 
                  }); 
                  */
                Menu menuObject = new Menu
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = menu.Name,
                    DateOfCreation = DateTime.Now,
                    Dishes = dishesFix
                };
                //await db.SaveChangesAsync();
                db.Menus.Add(menuObject);
                await db.SaveChangesAsync();
                return RedirectToAction("MyDetails", "Users");
            }

            return View(menu);
        }

        // GET: Menu/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = await db.Menus.FindAsync(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        // POST: Menu/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,DateOfCreation")] Menu menu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(menu).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(menu);
        }

        // GET: Menu/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = await db.Menus.FindAsync(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        // POST: Menu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            Menu menu = await db.Menus.FindAsync(id);
            db.Menus.Remove(menu);
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
