using FPTGreenManagement.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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
        private UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext _context;
        public TrainersController()
        {
            _userManager = new UserManager<ApplicationUser>(
         new UserStore<ApplicationUser>(new ApplicationDbContext()));
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
        [HttpGet]
        public ActionResult ChangePasswordTrainer(string id)
        {
            var userId = User.Identity.GetUserId();
            var user = _context.Users.Where(t => t.Id.Equals(userId)).FirstOrDefault(model => model.Id == id);
            var changePasswordViewModel = new AdminChangePasswordViewModel()
            {
                UserId = userId
            };

            return View(changePasswordViewModel);
        }
        [HttpPost]
        public ActionResult ChangePasswordTrainer(AdminChangePasswordViewModel model)
        {
            var user = _context.Users.SingleOrDefault(t => t.Id == model.UserId);
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Validation", "Some thing is wrong");
                return View(model);
            }
            if (user.PasswordHash != null)
            {
                _userManager.RemovePassword(user.Id);
            }
            _userManager.AddPassword(user.Id, model.NewPassword);
            return _userManager.GetRoles(user.Id).First() == "Trainer" ?
                RedirectToAction("Index", "Trainers") : RedirectToAction("Index", "Trainers");
        }
    }
}