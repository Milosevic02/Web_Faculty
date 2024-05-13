using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zadatak1.Models;

namespace Zadatak1.Controllers
{
    public class UsersController : Controller
    {
        // GET: Users
        public ActionResult Index()
        {
            List<User> users = new List<User>();
            users.Add(new User
            {
                Username = "pera",
                Password = "pera"
            });
            return View(users);
        }
    }
}