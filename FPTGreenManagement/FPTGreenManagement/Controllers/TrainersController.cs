using FPTGreenManagement.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace FPTGreenManagement.Controllers
{
    [Authorize]
    public class TrainersController : Controller
    {
        private ApplicationDbContext _context;
        public TrainersController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: Trainers
        public ActionResult Index()
        {
            var trainerId = User.Identity.GetUserId();
            var trainer = _context.Trainers.SingleOrDefault(u => u.Id.Equals(trainerId));
            if (trainer == null) return HttpNotFound();
            return View(trainer);
        }

        [HttpGet]
        public ActionResult Edit()
        {

            var trainerId = User.Identity.GetUserId();
            var trainer = _context.Trainers.SingleOrDefault(u => u.Id.Equals(trainerId));
            if (trainer == null) return HttpNotFound();
            return View(trainer);
        }

        [HttpPost]
        public ActionResult Edit(Trainer trainer)
        {
            if (!ModelState.IsValid)
            {
                return View(trainer);

            }
            var editTrainer = _context.Trainers.SingleOrDefault(u => u.Id.Equals(trainer.Id));
            if (editTrainer == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            editTrainer.TrainerName = trainer.TrainerName;
            editTrainer.WorkingPlace = trainer.WorkingPlace;
            editTrainer.Telephone = trainer.Telephone;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult CourseAssign()
        {
            var trainerId = User.Identity.GetUserId();
            var courseAssign = _context.TrainerCourses
                .Where(t => t.TrainerId == trainerId)
                .Select(t => t.Course)
                .Include(t => t.Category)
                .ToList();
            return View(courseAssign);
        }
    }
}