using FPTGreenManagement.Models;
using FPTGreenManagement.ViewModels;
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
    public class TrainerCoursesController : Controller
    {
        private ApplicationDbContext _context;
        public TrainerCoursesController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: TrainerCourses
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var trainerCourse = _context.TrainerCourses
                .Where(t => t.Trainer.Id.Equals(userId))
                .Include(t => t.Trainer)
                .Include(t => t.Course)
                .ToList();
            return View(trainerCourse);
        }
        [Authorize(Roles = "Staff")]
        [HttpGet]
        public ActionResult Create()
        {
            var role = (from r in _context.Roles where r.Name.Contains("Trainer") select r).FirstOrDefault();

            var courses = _context.Courses.ToList();
            var users = _context.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(role.Id)).ToList();

            var viewModel = new TrainerCoursesViewModel()
            {
                Courses = courses,
                Trainers = users,
                TrainerCourse = new TrainerCourse()
            };
            return View(viewModel);
        }
        [Authorize(Roles = "Staff")]
        [HttpPost]
        public ActionResult Create(TrainerCoursesViewModel model)
        {
            //get trainer
            var role = (from r in _context.Roles where r.Name.Contains("Trainer") select r).FirstOrDefault();

            var users = _context.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(role.Id)).ToList();

            //get course

            var courses = _context.Courses.ToList();


            if (ModelState.IsValid)
            {
                _context.TrainerCourses.Add(model.TrainerCourse);
                _context.SaveChanges();
                return RedirectToAction("AssignTrainer", "Staffs", new { area = "Staff" });
            }

            var trainerCourseVM = new TrainerCoursesViewModel()
            {
                Courses = courses,
                Trainers = users,
                TrainerCourse = new TrainerCourse()
            };

            return View(trainerCourseVM);
        }
        [Authorize(Roles = "Staff")]
        public ActionResult Delete(int? id)
        {
            var TrainerCourseInDB = _context.TrainerCourses.SingleOrDefault(m => m.Id == id);
            if (TrainerCourseInDB == null) return HttpNotFound();
            _context.TrainerCourses.Remove(TrainerCourseInDB);
            _context.SaveChanges();
            return RedirectToAction("AssignTrainer", "Staffs", new { area = "Staff" });
        }
        [Authorize(Roles = "Staff")]
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var role = (from r in _context.Roles where r.Name.Contains("Trainer") select r).FirstOrDefault();
            var users = _context.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(role.Id)).ToList();
            var editTrainerCourse = _context.TrainerCourses
               .SingleOrDefault(t => t.Id == id);
            if (editTrainerCourse == null) return HttpNotFound();
            var TrainerCourseVM = new TrainerCoursesViewModel()
            {
                TrainerCourse = editTrainerCourse,
                Courses = _context.Courses.ToList(),
                Trainers = users,
            };
            return View(TrainerCourseVM);
        }
        [Authorize(Roles = "Staff")]
        [HttpPost]
        public ActionResult Edit(TrainerCourse trainerCourse)
        {
            var editTrainerCourse = _context.TrainerCourses
                .SingleOrDefault(t => t.Id == trainerCourse.Id);

            if (!ModelState.IsValid)
            {
                var viewModel = new TrainerCoursesViewModel
                {
                    TrainerCourse = trainerCourse,
                    Courses = _context.Courses.ToList(),
                    Trainers = _context.Users.ToList()
                };
                return View(viewModel);
            }
            if (editTrainerCourse == null) return HttpNotFound();
            editTrainerCourse.CourseId = trainerCourse.CourseId;
            editTrainerCourse.TrainerId = trainerCourse.TrainerId;
            _context.SaveChanges();
            return RedirectToAction("AssignTrainer", "Staffs", new { area = "Staff" });
        }
    }
}