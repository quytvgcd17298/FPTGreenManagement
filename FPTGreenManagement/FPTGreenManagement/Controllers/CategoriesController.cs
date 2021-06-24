using FPTGreenManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FPTGreenManagement.Controllers
{
    public class CategoriesController : Controller
    {
        private ApplicationDbContext _context;
        public CategoriesController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: Categories
        [Authorize(Roles = "Staff")]
        public ActionResult Index()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }

        [HttpGet]
        [Authorize(Roles = "Staff")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Staff")]
        public ActionResult Create(Category model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var category = new Category
            {
                CategoryName = model.CategoryName
            };

            _context.Categories.Add(category);
            try
            {
                _context.SaveChanges();

            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException)
            {
                ModelState.AddModelError("", "Category Name alreay exists");
                return View(model);
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null) return HttpNotFound();
            var delCategory = _context.Categories
                .SingleOrDefault(t => t.Id == id);
            if (delCategory == null) return HttpNotFound();
            _context.Categories.Remove(delCategory);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}