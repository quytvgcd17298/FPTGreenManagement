using FPTGreenManagement.Models;
using FPTGreenManagement.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FPTGreenManagement.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        public AdminController()
        {
            _context = new ApplicationDbContext();
            _userManager = new UserManager<ApplicationUser>(
             new UserStore<ApplicationUser>(new ApplicationDbContext()));

        }
        // GET: Admin
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult GetStaffs()
        {
            var users = _context.Users.ToList();
            var staffs = new List<ApplicationUser>();

            foreach (var user in users)
            {
                if (_userManager.GetRoles(user.Id)[0].Equals("Staff"))
                {
                    staffs.Add(user);
                }
            }
            return View(staffs);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult GetTrainer()
        {
            var users = _context.Users.ToList();
            var trainers = new List<ApplicationUser>();

            foreach (var user in users)
            {
                if (_userManager.GetRoles(user.Id)[0].Equals("Trainer"))
                {
                    trainers.Add(user);
                }
            }
            return View(trainers);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult ChangePasswordStaff(string id)
        {
            var user = _context.Users.FirstOrDefault(model => model.Id == id);
            var changePasswordViewModel = new AdminChangePasswordViewModel()
            {
                UserId = user.Id
            };

            return View(changePasswordViewModel);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult ChangePasswordStaff(AdminChangePasswordViewModel model)
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
            return _userManager.GetRoles(user.Id).First() == "Staff" ?
                RedirectToAction("GetStaffs", "Admin") : RedirectToAction("GetStaffs", "Admin");

        }
        [Authorize(Roles = "Admin,Trainer")]
        [HttpGet]
        public ActionResult ChangePasswordTrainer(string id)
        {
            var user = _context.Users.FirstOrDefault(model => model.Id == id);
            var changePasswordViewModel = new AdminChangePasswordViewModel()
            {
                UserId = user.Id
            };

            return View(changePasswordViewModel);
        }
        [Authorize (Roles = "Admin,Trainer")]
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
                RedirectToAction("GetTrainer", "Admin") : RedirectToAction("GetTrainer", "Admin");

        }
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteAccount(string id)
        {
            var removeUser = _context.Users.SingleOrDefault(t => t.Id == id);
            if (removeUser == null) return HttpNotFound();
            _context.Users.Remove(removeUser);
            _context.SaveChanges();
            return RedirectToAction("GetStaffs");
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult RemoveTrainer(string id)
        {
            var trainerAcc = _context.Users
              .SingleOrDefault(t => t.Id == id);
            var trainerInfo = _context.Trainers
                .SingleOrDefault(t => t.Id == id);
            if (trainerAcc == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            if (trainerInfo == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            _context.Users.Remove(trainerAcc);
            _context.Trainers.Remove(trainerInfo);
            _context.SaveChanges();
            return RedirectToAction("GetTrainer");
        }
    }
}