using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zadatak2.Models;

namespace Zadatak2.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            User user = (User)Session["user"];
            if(user == null || user.Username.Equals("") ) {
                return RedirectToAction("Index", "Authetication");
            }
            Dictionary<Product, Int32> cart = (Dictionary<Product, Int32>)Session["cart"];
            if(cart == null )
            {
                cart = new Dictionary<Product, Int32>();
                Session["cart"] = cart;
            }
            ViewBag.User = user;
            List<Product> products = (List<Product>)HttpContext.Application["products"];
            return View(products);
        }
    }
}