using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mission13.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Mission13.Controllers
{
    public class HomeController : Controller
    {
        private BowlingDbContext context { get; set; }

        //public HomeController(BowlingDbContext temp) => context = temp;
        private IBowlersRepository _repo { get; set; }
        //private ITeamsRepository _repo { get; set; }

        //constructor
        public HomeController(BowlingDbContext temp)
        {
            context = temp;
        }

        public IActionResult Index()
        {
            var blah = context.Bowlers
                //.FromSqlRaw("SELECT * FROM Recipes WHERE RecipeTitle LIKE 'a%'")
                .ToList();

            return View(blah);
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            var Bowler = context.Bowlers.Single(x => x.BowlerID == id);
            return View("BowlerInfo", Bowler);
        }
        [HttpPost]
        public IActionResult Edit(Bowler info)
        {
            if (ModelState.IsValid)
            {
                context.Update(info);
                context.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return View(info);
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var bowler = context.Bowlers.Single(x => x.BowlerID == id);

            return View(bowler);
        }
        [HttpPost]
        public IActionResult Delete(Bowler bowl)
        {

            context.Bowlers.Remove(bowl);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult NewBowler()
        {

            return View("NewBowler");
        }

        [HttpPost]
        public IActionResult NewBowler(Bowler bowl)
        {
            bowl.BowlerID = 55;
            if (ModelState.IsValid)
            {
                context.Update(bowl);
                context.SaveChanges();
                return RedirectToAction("Success");
            }
            else
            {
                return View(bowl);
            }

        }


        public IActionResult Success()
        {
            return View("Index");
        }
    }
}
