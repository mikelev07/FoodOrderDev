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


        public async Task<ActionResult> MyOrders()
        {
            string userId = User.Identity.GetUserId();
          
            var companies = await db.Companies.Include(c => c.Employees.Select(b => b.Orders)).ToListAsync();
            var company = companies.Where(c => c.RepresentativeId == userId).FirstOrDefault();

            return View(company.Employees);
        }

        [Authorize(Roles = "employee")]
        public async Task<ActionResult> OrdersHistory()
        {
            string userId = User.Identity.GetUserId();
            List<Order> orders = await db.Orders.Include(c=>c.ChoosenDishes).Where(c => c.UserId == userId).ToListAsync();
            
            return View(orders);
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
                var companies = await db.Companies.Include(c=>c.Employees).ToListAsync();
                var company = companies.Where(c => c.RepresentativeId == userId).FirstOrDefault();

                return View("IndexCompany", company);
            }
            if (await UserManager.IsInRoleAsync(userId, "employee"))
            {
                var menu = db.Menus.Include(c=>c.Packs.Select(y => y.Dishes)).Include(m => m.Dishes.Select(y => y.Garnishes)).Where(c=>c.DateOfCreation.Day == DateTime.Now.Day && c.DateOfCreation.Month == DateTime.Now.Month).FirstOrDefault();
                var categories = await db.DishCategories.Include(d => d.Dishes).ToListAsync();

                var user = db.Users.Include(c=>c.Company).Where(c => c.Id == userId).FirstOrDefault();

             
                if (user.Company.SingleChoice)
                {
                    ViewBag.Unlimited = user.Company.UnlimitedOrders;

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

                        return View("IndexEmployeeSingle", menuViewModel);
                    }
                    else
                    {
                        return View("IndexEmployeeSingle");
                    }

                }

                if (user.Company.PacksPicker)
                {
                    ViewBag.Unlimited = user.Company.UnlimitedOrders;

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

                        return View("IndexEmployeePacks", menuViewModel);
                    }
                    else
                    {
                        return View("IndexEmployeePacks");
                    }

                }


            }
            if (await UserManager.IsInRoleAsync(userId, "cook"))
            {
                var orders = await db.Orders.Include(o => o.User).ToListAsync();

                var st = db.DishStatistics.ToList();
                CookStatisticViewModel cookStatisticViewModel = new CookStatisticViewModel();
                cookStatisticViewModel.DishStatistics = st;

                return View("IndexCook", cookStatisticViewModel);
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

        public ActionResult SucessOrder(string id)
        {
            ViewBag.Id = id;
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

        public async Task<ActionResult> CreatePackOrderJson(string[] IDs, string descr)
        {
            List<Pack> packs = db.Packs.Include(c => c.Dishes).Where(c => IDs.Contains(c.Id)).ToList();
            
            var dateOfCreation = DateTime.UtcNow;
            string uid = User.Identity.GetUserId();
            User user = db.Users.Include(c => c.Company).Where(c => c.Id == uid).FirstOrDefault();
            user.Company.OrdersCount++;

            Random random = new Random();
            int i = random.Next();

            Order order = new Order()
            {
                Id = Guid.NewGuid().ToString("N"),
                Packs = packs,
                DateOfCreation = dateOfCreation,
                UserId = uid,
                Description = descr,
                OrderSpecId = user.Company.Name.Substring(0, 2).ToUpper() + i.ToString(), 
                Status = OrderStatus.Done
            };

            db.Orders.Add(order);
            await db.SaveChangesAsync();
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("SucessOrder", "Orders", new { id = order.OrderSpecId });
            return Json(new { Url = redirectUrl });
        }

        public async Task<ActionResult> CreateJson(string[] IDs, string[] GarIds, string descr)
        {
            List<Dish> dishes = db.Dishes.Include(c=>c.DishCategory).Where(c => IDs.Contains(c.Id)).ToList();
            List<Garnish> garnishes = db.Garnishes.Where(c => GarIds.Contains(c.Id)).ToList();
            List<ChoosenDish> choosenDishes = new List<ChoosenDish>();
            
            for (var i =0; i < dishes.Count;)
            {
                if (garnishes.Count != 0)
                {
                    for (var a = 0; a < garnishes.Count; a++)
                    {
                        var d = dishes[i];
                        var g = garnishes[a];
                        ChoosenDish obj = CreateChoosenDish(d, g);

                        DishStatistic dishStatistic = new DishStatistic();
                        if (db.DishStatistics.Where(c => c.DishName == d.Name).FirstOrDefault() == null)
                        {
                            SetValue(d, dishStatistic);
                            db.DishStatistics.Add(dishStatistic);
                            db.SaveChanges();
                        }
                        else
                        {
                            var st = db.DishStatistics.Where(c => c.DishName == d.Name).FirstOrDefault();
                            db.Entry(st).State = EntityState.Modified;
                            st.Count++;
                            db.SaveChanges();
                        }

                        if (d.HasGarnish)
                        {
                            SetStatisticGarnish(g);
                        }

                        db.SaveChanges();
                        choosenDishes.Add(obj);
                        i++;
                    }
                }
                else
                {
                    var d = dishes[i];
                    ChoosenDish obj = CreateChoosenDishWithoutGarnish(d);

                    DishStatistic dishStatistic = new DishStatistic();
                    if (db.DishStatistics.Where(c => c.DishName == d.Name).FirstOrDefault() == null)
                    {
                        SetValue(d, dishStatistic);
                        db.DishStatistics.Add(dishStatistic);
                        db.SaveChanges();
                    }
                    else
                    {
                        var st = db.DishStatistics.Where(c => c.DishName == d.Name).FirstOrDefault();
                        db.Entry(st).State = EntityState.Modified;
                        st.Count++;
                        db.SaveChanges();
                    }

                    db.SaveChanges();
                    choosenDishes.Add(obj);
                    i++;

                }
            }

            var dateOfCreation = DateTime.UtcNow;
            string uid = User.Identity.GetUserId();
            User user = db.Users.Include(c=>c.Company).Where(c => c.Id == uid).FirstOrDefault();
            user.Company.OrdersCount++;

            Random random = new Random();
            int b = random.Next();

            Order order = new Order()
            {
                Id = Guid.NewGuid().ToString("N"),
                ChoosenDishes = choosenDishes,
                DateOfCreation = dateOfCreation,
                UserId = uid,
                Description = descr,
                OrderSpecId = user.Company.Name.Substring(0, 2).ToUpper() + b.ToString(),
                Status = OrderStatus.Done
            };

            db.Orders.Add(order);
            await db.SaveChangesAsync();
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("SucessOrder", "Orders", new { id = order.OrderSpecId });
            return Json(new { Url = redirectUrl });
        }

        private ChoosenDish CreateChoosenDish(Dish d, Garnish g)
        {
            var obj = new ChoosenDish();
            obj.Id = Guid.NewGuid().ToString();
            obj.DishName = d.Name;
            obj.DishCategoryName = d.DishCategory.Name;
            obj.Kilocalories = d.Kilocalories;
            obj.ImagePath = d.ImagePath;
            obj.Fats = d.Fats;
            obj.Carbonhydrates = d.Carbonhydrates;
            obj.GarinshName = g.Name;
            db.ChoosenDishes.Add(obj);
            return obj;
        }

        private ChoosenDish CreateChoosenDishWithoutGarnish(Dish d)
        {
            var obj = new ChoosenDish();
            obj.Id = Guid.NewGuid().ToString();
            obj.DishName = d.Name;
            obj.DishCategoryName = d.DishCategory.Name;
            obj.Kilocalories = d.Kilocalories;
            obj.ImagePath = d.ImagePath;
            obj.Fats = d.Fats;
            obj.Carbonhydrates = d.Carbonhydrates;
            obj.GarinshName = null;
            db.ChoosenDishes.Add(obj);
            return obj;
        }


        private static void SetValue(Dish d, DishStatistic dishStatistic)
        {
            dishStatistic.Id = Guid.NewGuid().ToString();
            dishStatistic.DishName = d.Name;
            dishStatistic.Count = 1;
            dishStatistic.DishCategoryName = d.DishCategory.Name;
            dishStatistic.IsReady = false;
        }

        private void SetStatisticGarnish(Garnish g)
        {
            DishStatistic garnStatistic = new DishStatistic();
            if (db.DishStatistics.Where(c => c.DishName == g.Name).FirstOrDefault() == null)
            {
                garnStatistic.Id = Guid.NewGuid().ToString();
                garnStatistic.DishName = g.Name;
                garnStatistic.Count = 1;
                garnStatistic.DishCategoryName = "Гарнир";
                garnStatistic.IsReady = false;
                db.DishStatistics.Add(garnStatistic);
                db.SaveChanges();
            }
            else
            {
                var st = db.DishStatistics.Where(c => c.DishName == g.Name).FirstOrDefault();
                db.Entry(st).State = EntityState.Modified;
                st.Count++;
                db.SaveChanges();
            }
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
