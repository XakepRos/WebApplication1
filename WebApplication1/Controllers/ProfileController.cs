using Microsoft.AspNetCore.Mvc;
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
            IEnumerable<ProfileViewModel> objlist = _context.Profile;
            return View(objlist);
        }

        public IActionResult CreateProfile()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateProfile(Department dept)
        {
            if (ModelState.IsValid)
            {
                dept.DepartmentCreatedOn = DateTime.Now;
                _context.Department.Add(dept);
                _context.SaveChanges();
                TempData["ResultOk"] = "Department Created Successfully !";
                return RedirectToAction("Index");
            }

            return View(dept);
        }

        public IActionResult EditProfile(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var deptfromdb = _context.Department.Find(id);

            if (deptfromdb == null)
            {
                return NotFound();
            }
            return View(deptfromdb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditProfile(Department dept)
        {
            if (ModelState.IsValid)
            {
                _context.Department.Update(dept);
                _context.SaveChanges();
                TempData["ResultOk"] = "Department Updated Successfully !";
                return RedirectToAction("Index");
            }

            return View(dept);
        }

        public IActionResult DeleteProfile(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var deptfromdb = _context.Department.Find(id);

            if (deptfromdb == null)
            {
                return NotFound();
            }
            return View(deptfromdb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteDeptProfile(int? id)
        {
            var deleteDepartment = _context.Department.Find(id);
            if (deleteDepartment == null)
            {
                return NotFound();
            }
            _context.Department.Remove(deleteDepartment);
            _context.SaveChanges();
            TempData["ResultOk"] = "Department Deleted Successfully !";
            return RedirectToAction("Index");
        }
    }
}
