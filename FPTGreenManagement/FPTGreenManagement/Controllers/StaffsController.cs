using FPTGreenManagement.Models;
using FPTGreenManagement.ViewModels;
using Microsoft.Ajax.Utilities;
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
    public class StaffsController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext _context;
        public StaffsController()
        {
            _userManager = new UserManager<ApplicationUser>(
            new UserStore<ApplicationUser>(new ApplicationDbContext()));
            _context = new ApplicationDbContext();
        }
        // GET: Staff
        [Authorize(Roles = "Staff,Trainee")]
        public ActionResult IndexCourse(string searchString)
        {
            var courses = _context.Courses.Include(t => t.Category).ToList();
            if (!String.IsNullOrWhiteSpace(searchString))
            {
                courses = _context.Courses
                .Where(t => t.CourseName.Contains(searchString))
                .Include(t => t.Category)
                .ToList();
            }
            return View(courses);
        }
        [Authorize(Roles = "Staff")]
        [HttpGet]
        public ActionResult CreateCourse()
        {
            var viewModel = new CourseCategoriesViewModel()
            {
                Categories = _context.Categories.ToList(),
            };
            return View(viewModel);
        }
        [Authorize(Roles = "Staff")]
        [HttpPost]
        public ActionResult CreateCourse(Course course)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new CourseCategoriesViewModel()
                {
                    Course = course,
                    Categories = _context.Categories.ToList()
                };

                return View(viewModel);
            }
            var newCourse = new Course()
            {
                CourseName = course.CourseName,
                Description = course.Description,
                CategoryId = course.CategoryId,
            };
            _context.Courses.Add(newCourse);
            _context.SaveChanges();
            return RedirectToAction("IndexCourse");
        }
        [Authorize(Roles = "Staff")]

        public ActionResult DeleteCourse(int id)
        {
            var removeCourse = _context.Courses.SingleOrDefault(t => t.Id == id);
            _context.Courses.Remove(removeCourse);
            _context.SaveChanges();
            return RedirectToAction("IndexCourse");
        }
        [Authorize(Roles = "Staff")]
        [HttpGet]
        public ActionResult EditCourse(int id)
        {
            var fcourse = _context.Courses.SingleOrDefault(t => t.Id == id);
            var dcourse = new CourseCategoriesViewModel()
            {
                Course = fcourse,
                Categories = _context.Categories.ToList()
            };
            return View(dcourse);
        }
        [Authorize(Roles = "Staff")]
        [HttpPost]
        public ActionResult EditCourse(Course course)
        {
            var editCourse = _context.Courses
                .SingleOrDefault(t => t.Id == course.Id);

            if (!ModelState.IsValid)
            {
                var viewModel = new CourseCategoriesViewModel
                {
                    Course = course,
                    Categories = _context.Categories.ToList()
                };
                return View(viewModel);
            }
            if (editCourse == null) return HttpNotFound();
            editCourse.CourseName = course.CourseName;
            editCourse.Description = course.Description;
            editCourse.CategoryId = course.CategoryId;

            _context.SaveChanges();
            return RedirectToAction("IndexCourse");
        }
        [Authorize(Roles = "Staff,Trainee")]
        public ActionResult DetailCourse(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var deCourse = _context.Courses
                .Include(t => t.Category)
                .SingleOrDefault(t => t.Id == id);
            if (deCourse == null) return HttpNotFound();
            return View(deCourse);
        }
        [Authorize(Roles = "Staff")]
        public ActionResult AssignTrainer()
        {
            var trainerCourse = _context.TrainerCourses
                .Include(t => t.Trainer)
                .Include(t => t.Course)
                .ToList();
            return View(trainerCourse);
        }
        [Authorize(Roles = "Staff")]
        public ActionResult AssignTrainee()
        {
            var traineeCourse = _context.TraineeCourses
                .Include(t => t.Trainee)
                .Include(t => t.Course)
                .ToList();
            return View(traineeCourse);
        }
        [Authorize(Roles = "Staff")]
        public ActionResult TrainerList(string searchString)
        {
            var trainerInDB = _context.Trainers
            .ToList();

            if (!searchString.IsNullOrWhiteSpace())
            {
                trainerInDB = _context.Trainers.Where(t => t.TrainerName.Contains(searchString)).ToList();
            }
            return View(trainerInDB);
        }
        [Authorize(Roles = "Staff")]
        [HttpGet]
        public ActionResult UpdateTrainer(string id)
        {
            var trainer = _context.Trainers.SingleOrDefault(u => u.Id == id);
            if (trainer == null) return HttpNotFound();
            return View(trainer);
        }
        [HttpPost]
        [Authorize(Roles = "Staff")]
        public ActionResult UpdateTrainer(Trainer trainer)
        {
            var trainerInDb = _context.Trainers.SingleOrDefault(t => t.Id == trainer.Id);

            if (!ModelState.IsValid)
            {
                return View(trainer);
            }

            if (trainerInDb == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            trainerInDb.TrainerName = trainer.TrainerName;
            trainerInDb.WorkingPlace = trainer.WorkingPlace;
            trainerInDb.Telephone = trainer.Telephone;
            _context.SaveChanges();

            return RedirectToAction("TrainerList");
        }
        [Authorize(Roles = "Staff")]
        public ActionResult DetailsTrainer(string id)
        {
            var trainer = _context.Trainers.SingleOrDefault(t => t.Id == id);

            if (trainer == null) return HttpNotFound();

            return View(trainer);
        }
        [Authorize(Roles = "Staff")]
        public ActionResult TraineeList(string searchString)
        {
            var traineeInDB = _context.Trainees
            .ToList();

            if (!searchString.IsNullOrWhiteSpace())
            {
                traineeInDB = _context.Trainees.Where(t => t.TraineeName.Contains(searchString)).ToList();
            }
            return View(traineeInDB);
        }
        [Authorize(Roles = "Staff")]
        [HttpGet]
        public ActionResult UpdateTrainee(string id)
        {
            var trainee = _context.Trainees.SingleOrDefault(u => u.Id == id);
            if (trainee == null) return HttpNotFound();
            return View(trainee);
        }
        [Authorize(Roles = "Staff")]
        [HttpPost]
        public ActionResult UpdateTrainee(Trainee trainee)
        {
            var traineeInDb = _context.Trainees.SingleOrDefault(t => t.Id == trainee.Id);

            if (!ModelState.IsValid)
            {
                return View(trainee);
            }

            if (traineeInDb == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            traineeInDb.TraineeName = trainee.TraineeName;
            traineeInDb.Age = trainee.Age;
            traineeInDb.DateOfBirth = trainee.DateOfBirth;
            traineeInDb.Education = trainee.Education;
            traineeInDb.MainProgrammingLanguage = trainee.MainProgrammingLanguage;
            traineeInDb.TOEICscore = trainee.TOEICscore;
            traineeInDb.ExperienceDetails = trainee.ExperienceDetails;
            traineeInDb.Department = trainee.Department;
            traineeInDb.Location = trainee.Location;
            _context.SaveChanges();

            return RedirectToAction("TraineeList");
        }
        [Authorize(Roles = "Staff")]
        public ActionResult DetailsTrainee(string id)
        {
            var trainee = _context.Trainees.SingleOrDefault(t => t.Id == id);

            if (trainee == null) return HttpNotFound();

            return View(trainee);
        }
        [Authorize(Roles = "Staff")]
        public ActionResult GetTrainee()
        {
            var users = _context.Users.ToList();
            var trainees = new List<ApplicationUser>();

            foreach (var user in users)
            {
                if (_userManager.GetRoles(user.Id)[0].Equals("Trainee"))
                {
                    trainees.Add(user);
                }
            }
            return View(trainees);
        }
        [Authorize(Roles = "Staff,Trainee")]
        [HttpGet]
        public ActionResult ChangePasswordTrainee(string id)
        {
            var user = _context.Users.FirstOrDefault(model => model.Id == id);
            var changePasswordViewModel = new AdminChangePasswordViewModel()
            {
                UserId = user.Id
            };

            return View(changePasswordViewModel);
        }
        [Authorize(Roles = "Staff,Trainee")]
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
                RedirectToAction("GetTrainee", "Staffs") : RedirectToAction("GetTrainee", "Staffs");
        }
        [Authorize(Roles = "Staff")]
        [HttpGet]
        public ActionResult RemoveTrainee(string id)
        {
            var traineeAcc = _context.Users
              .SingleOrDefault(t => t.Id == id);
            var traineeInfo = _context.Trainees
                .SingleOrDefault(t => t.Id == id);
            if (traineeAcc == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            if (traineeInfo == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            _context.Users.Remove(traineeAcc);
            _context.Trainees.Remove(traineeInfo);
            _context.SaveChanges();
            return RedirectToAction("GetTrainee");
        }
    }
}