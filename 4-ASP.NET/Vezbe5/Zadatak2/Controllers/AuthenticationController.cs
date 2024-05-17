using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zadatak2.Models;

namespace Zadatak2.Controllers
{
    public class AuthenticationController : Controller
    {
        // GET: Authentication
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            User user = new User();
            Session["user"] = user;
            return View(user);
        }

        [HttpPost]
        public ActionResult Register(User user)
        {
            List<User> users = (List<User>)HttpContext.Application["Users"];
            if(users.Contains(user))
            {
                ViewBag.Message = $"User with {user.Username} already exists!";
                return View();
            }

            users.Add(user);
            Data.SaveUser(user);
            Session["user"] = user;
            return RedirectToAction("Index", "Home");

        }
    }
}