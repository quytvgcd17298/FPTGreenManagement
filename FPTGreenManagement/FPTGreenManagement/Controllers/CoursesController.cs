using FPTGreenManagement.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using FPTGreenManagement.ViewModels;
using System.Net;
using Microsoft.AspNet.Identity;

namespace FPTGreenManagement.Controllers
{
    [Authorize]
    public class CoursesController : Controller
    {
        private ApplicationDbContext _context;
        public CoursesController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: Courses
        [HttpGet]
        public ActionResult Index(string searchString)
        {
            var userId = User.Identity.GetUserId();
            var inCourses = _context.Courses
                .Include(t => t.Category)
                .Where(t => t.UserId.Equals(userId))
                .ToList();

            if (!searchString.IsNullOrWhiteSpace())
            {
                inCourses = _context.Courses.Where(t => t.Description.Contains(searchString)).ToList();
            }
            return View(inCourses);
        }

        public ActionResult Detail(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var userId = User.Identity.GetUserId();
            var deCourse = _context.Courses
                .Include(t => t.Category)
                .Where(t => t.UserId.Equals(userId))
                .SingleOrDefault(t => t.Id == id);
            if (deCourse == null) return HttpNotFound();
            return View(deCourse);
        }
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            var userId = User.Identity.GetUserId();
            if (id == null) return HttpNotFound();
            var delCourse = _context.Courses
                .Where(t => t.UserId.Equals(userId))
                .SingleOrDefault(t => t.Id == id);
            if (delCourse == null) return HttpNotFound();
            _context.Courses.Remove(delCourse);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Create()
        {

            var viewModel = new CourseCategoriesViewModel()
            {
                Categories = _context.Categories.ToList()
            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult Create(Course course)
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
            var userId = User.Identity.GetUserId();

            var newCourse = new Course()
            {
                CourseName = course.CourseName,
                Description = course.Description,
                CategoryId = course.CategoryId,
                UserId = userId
            };
            _context.Courses.Add(newCourse);
            _context.SaveChanges(); 
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            var userId = User.Identity.GetUserId();

            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var editCourse = _context.Courses
                .Where(t =>t.UserId.Equals(userId))
                .SingleOrDefault(t => t.Id == id);
            if (editCourse == null) return HttpNotFound();
            var viewModel = new CourseCategoriesViewModel
            {
                Course = editCourse,
                Categories = _context.Categories.ToList()
            };

            return View(viewModel);

        }
        [HttpPost]
        public ActionResult Edit(Course course)
        {
            var userId = User.Identity.GetUserId();
            var editCourse = _context.Courses
                .Where(t => t.UserId.Equals(userId))
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
            return RedirectToAction("Index");


        }

    }
}