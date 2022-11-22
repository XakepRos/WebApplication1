using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ProfileController : Controller
    {
        private readonly DbaseContext _context;
        public ProfileController(DbaseContext context)
        {
            _context = context;
        }

        public IActionResult Profile()
        {
            return View();
        }

        [Authorize]
        public IActionResult UserProfile()
        {
            return View(new UserProfile()
            {
                Name = User.Identity.Name,
                Email = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value,
                Avatar = User.FindFirst(c => c.Type == "picture")?.Value
            });
        }

        public IActionResult CreateProfile()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateProfile(ProfileViewModel profile)
        {
            if (ModelState.IsValid)
            {
                //profile.DepartmentCreatedOn = DateTime.Now;
                _context.Profile.Add(profile);
                _context.SaveChanges();
                TempData["ResultOk"] = "Profile Saved Successfully !";
                return RedirectToAction("Profile");
            }

            return View(profile);
        }

        public IActionResult EditProfile(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var profilefromdb = _context.Profile.Find(id);

            if (profilefromdb == null)
            {
                return NotFound();
            }
            return View(profilefromdb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditProfile(ProfileViewModel profile)
        {
            if (ModelState.IsValid)
            {
                _context.Profile.Update(profile);
                _context.SaveChanges();
                TempData["ResultOk"] = "Department Updated Successfully !";
                return RedirectToAction("Index");
            }

            return View(profile);
        }

        public IActionResult DeleteProfile(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var profilefromdb = _context.Profile.Find(id);

            if (profilefromdb == null)
            {
                return NotFound();
            }
            return View(profilefromdb);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteUserProfile(int? id)
        {
            var deleteProfile = _context.Profile.Find(id);
            if (deleteProfile == null)
            {
                return NotFound();
            }
            _context.Profile.Remove(deleteProfile);
            _context.SaveChanges();
            TempData["ResultOk"] = "Department Deleted Successfully !";
            return RedirectToAction("Index");
        }
    }
}
