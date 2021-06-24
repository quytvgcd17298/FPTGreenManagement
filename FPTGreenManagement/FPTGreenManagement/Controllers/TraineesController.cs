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
    public class TraineesController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext _context;
        public TraineesController()
        {
            _userManager = new UserManager<ApplicationUser>(
            new UserStore<ApplicationUser>(new ApplicationDbContext()));
            _context = new ApplicationDbContext();
        }
        // GET: trainees
        public ActionResult Index()
        {
            var traineeId = User.Identity.GetUserId();
            var trainee = _context.Trainees.SingleOrDefault(u => u.Id.Equals(traineeId));
            if (trainee == null) return HttpNotFound();
            return View(trainee);
        }

        [HttpGet]
        public ActionResult Edit()
        {

            var traineeId = User.Identity.GetUserId();
            var trainee = _context.Trainees.SingleOrDefault(u => u.Id.Equals(traineeId));
            if (trainee == null) return HttpNotFound();
            return View(trainee);
        }

        [HttpPost]
        public ActionResult Edit(Trainee trainee)
        {
            if (!ModelState.IsValid)
            {
                return View(trainee);

            }
            var editTrainee = _context.Trainees.SingleOrDefault(u => u.Id.Equals(trainee.Id));
            if (editTrainee == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            editTrainee.TraineeName = trainee.TraineeName;
            editTrainee.Age = trainee.Age;
            editTrainee.DateOfBirth = trainee.DateOfBirth;
            editTrainee.Education = trainee.Education;
            editTrainee.MainProgrammingLanguage = trainee.MainProgrammingLanguage;
            editTrainee.TOEICscore = trainee.TOEICscore;
            editTrainee.ExperienceDetails = trainee.ExperienceDetails;
            editTrainee.Department = trainee.Department;
            editTrainee.Location = trainee.Location;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult ChangePasswordTrainee(string id)
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
        public ActionResult ChangePasswordTrainee(AdminChangePasswordViewModel model)
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
            return _userManager.GetRoles(user.Id).First() == "Trainee" ?
                RedirectToAction("Index", "Trainees") : RedirectToAction("Index", "Trainees");
        }
    }
}