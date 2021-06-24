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
    public class TraineeCoursesController : Controller
    {
        private ApplicationDbContext _context;
        public TraineeCoursesController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: TrainerCourses
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var traineeCourse = _context.TraineeCourses
                .Where(t => t.Trainee.Id.Equals(userId))
                .Include(t => t.Trainee)
                .Include(t => t.Course)
                .ToList();
            return View(traineeCourse);
        }
        [Authorize(Roles = "Staff")]
        [HttpGet]
        public ActionResult Create()
        {
            var role = (from r in _context.Roles where r.Name.Contains("Trainee") select r).FirstOrDefault();

            var courses = _context.Courses.ToList();
            var users = _context.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(role.Id)).ToList();

            var viewModel = new TraineeCoursesViewModel()
            {
                Courses = courses,
                Trainees = users,
                TraineeCourse = new TraineeCourse()
            };
            return View(viewModel);
        }
        [Authorize(Roles = "Staff")]
        [HttpPost]
        public ActionResult Create(TraineeCoursesViewModel model)
        {
            //get trainer
            var role = (from r in _context.Roles where r.Name.Contains("Trainee") select r).FirstOrDefault();

            var users = _context.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(role.Id)).ToList();

            //get course

            var courses = _context.Courses.ToList();


            if (ModelState.IsValid)
            {
                _context.TraineeCourses.Add(model.TraineeCourse);
                _context.SaveChanges();
                return RedirectToAction("AssignTrainee", "Staffs", new { area = "Staff" });
            }

            var traineeCourseVM = new TraineeCoursesViewModel()
            {
                Courses = courses,
                Trainees = users,
                TraineeCourse = new TraineeCourse()
            };

            return View(traineeCourseVM);
        }
        [Authorize(Roles = "Staff")]
        public ActionResult Delete(int? id)
        {
            var TraineeCourseInDB = _context.TraineeCourses.SingleOrDefault(m => m.Id == id);
            if (TraineeCourseInDB == null) return HttpNotFound();
            _context.TraineeCourses.Remove(TraineeCourseInDB);
            _context.SaveChanges();
            return RedirectToAction("AssignTrainee", "Staffs", new { area = "Staff" });
        }
        [Authorize(Roles = "Staff")]
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var role = (from r in _context.Roles where r.Name.Contains("Trainee") select r).FirstOrDefault();
            var users = _context.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(role.Id)).ToList();
            var editTraineeCourse = _context.TraineeCourses
               .SingleOrDefault(t => t.Id == id);
            if (editTraineeCourse == null) return HttpNotFound();
            var TraineeCourseVM = new TraineeCoursesViewModel()
            {
                TraineeCourse = editTraineeCourse,
                Courses = _context.Courses.ToList(),
                Trainees = users,
            };
            return View(TraineeCourseVM);
        }
        [Authorize(Roles = "Staff")]
        [HttpPost]
        public ActionResult Edit(TraineeCourse traineeCourse)
        {
            var role = (from r in _context.Roles where r.Name.Contains("Trainee") select r).FirstOrDefault();
            var users = _context.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(role.Id)).ToList();
            var editTraineeCourse = _context.TraineeCourses
                .SingleOrDefault(t => t.Id == traineeCourse.Id);

            if (!ModelState.IsValid)
            {
                var viewModel = new TraineeCoursesViewModel
                {
                    TraineeCourse = traineeCourse,
                    Courses = _context.Courses.ToList(),
                    Trainees = users,
                };
                return View(viewModel);
            }
            if (editTraineeCourse == null) return HttpNotFound();
            editTraineeCourse.CourseId = traineeCourse.CourseId;
            editTraineeCourse.TraineeId = traineeCourse.TraineeId;
            _context.SaveChanges();
            return RedirectToAction("AssignTrainee", "Staffs", new { area = "Staff" });
        }
    }
}