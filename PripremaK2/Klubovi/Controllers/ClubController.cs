using Klubovi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Klubovi.Controllers
{
    public class ClubController : Controller
    {
        // GET: Club
        public ActionResult Index()
        {
            Dictionary<string, Club> clubs = (Dictionary<string, Club>)HttpContext.Application["clubs"];

            ViewBag.clubs = clubs.Values;
            return View();
        }

        [HttpPost]
        public ActionResult Add(Club club)
        {
            Dictionary<string, Club> clubs = (Dictionary<string, Club>)HttpContext.Application["clubs"];
            //bool userExists = Users.users.ContainsKey(user.Username);
            clubs.Add(club.Name, club);
            return RedirectToAction("Index", "Club");


        }

        [HttpPost]
        public ActionResult Points(string club,string point)
        {
            Dictionary<string, Club> clubs = (Dictionary<string, Club>)HttpContext.Application["clubs"];

            clubs[club].Points = Int32.Parse(point);
            return RedirectToAction("Index", "Club");


        }

        public ActionResult Edit(string name)
        {
            Dictionary<string, Club> clubs = (Dictionary<string, Club>)HttpContext.Application["clubs"];
            return RedirectToAction("Edit","Club");
        }

        [HttpPost]
        public ActionResult Edit(Club club) {

            return RedirectToAction("Index", "Club");
        }
    }
}