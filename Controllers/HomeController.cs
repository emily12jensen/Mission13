using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mission13.Models;
using Mission13.Models.ViewModels;
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

        public IActionResult Index( string BowlerTeam)
        {
            var x = new BowlerViewModel
            {

                Bowlers = context.Bowlers
                .Where(x => x.Team.TeamName == BowlerTeam || BowlerTeam == null)

            };
            ViewBag.BowlerTeam = BowlerTeam;
            //var blah = context.Bowlers
                //.Include(x => TeamID)
               // .FromSqlRaw("SELECT * FROM bowlers WHERE BowlerFirstName LIKE 'a%'")
                //.ToList();

             return View(x);
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
            if (ModelState.IsValid)
            {
                context.Add(bowl);
                context.SaveChanges();
                bowl.BowlerID = context.Bowlers.Count() + 1;
                var x = new BowlerViewModel
                {
                    Bowlers = context.Bowlers

                };
                return RedirectToAction("Index", x);
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

        public IActionResult ViewTeams()
        {
            return View("TeamInfo");
        }
    }
}
