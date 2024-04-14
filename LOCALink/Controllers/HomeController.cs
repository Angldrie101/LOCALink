using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace LOCALink.Controllers
{
    [Authorize(Roles = "User, Worker, Admin")]
    public class HomeController : BaseController
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(_userRepo.GetAll());
        }
        [AllowAnonymous]
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index");

            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(User_Account u)
        {
            var user = _userRepo._table.Where(m => m.user_name == u.user_name).FirstOrDefault();

            if (user != null)
            {
                FormsAuthentication.SetAuthCookie(u.user_name, false);
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "User does not Exist or Incorrect Password");

            return View(u);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
        [AllowAnonymous]
        public ActionResult Create()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Create(User_Account u)
        {
            // Check if the model state is valid
            if (ModelState.IsValid)
            {
                try
                {
                    switch (u.user_type)
                    {
                        case 1:
                            break;
                        case 2:
                            break;
                        default:
                            throw new InvalidOperationException("Unknown user type.");
                    }
                    
                    _userRepo.Create(u);

                    TempData["Msg"] = $"User {u.user_name} added as {u.user_type}!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error: {ex.Message}");
                }
            }
            
            return View(u);
        }
        public ActionResult Details(int id)
        {
            return View(_userRepo.Get(id));
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            return View(_userRepo.Get(id));
        }
        [HttpPost]
        public ActionResult Edit(User_Account u)
        {
            _userRepo.Update(u.user_id, u);
            TempData["Msg"] = $"User {u.user_name} updated!";
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            _userRepo.Delete(id);
            TempData["Msg"] = $"User deleted!";
            return RedirectToAction("Index");
        }
        [AllowAnonymous]
        public ActionResult BookService()
        {
            return View();
        }

        [HttpPost]
        public ActionResult BookService(Booking b)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    using (var dbContext = new LOCALinkEntities())
                    {
                        var catID = new SqlParameter("@category_id", b.category_id);
                        var date = new SqlParameter("@booking_date", b.booking_date);
                        var price = new SqlParameter("@total_price", b.total_price);

                        dbContext.Database.ExecuteSqlCommand("sp_bookingshesh @category_id, @booking_date, @total_price", catID, date, price);
                    }

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["Msg"] = $"Error: {ex.Message}";
                }
            }
            return View();

        }
    }
}
