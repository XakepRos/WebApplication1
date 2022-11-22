using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly DbaseContext _context;
        public DepartmentController(DbaseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            IEnumerable<Department> objlist = _context.Department;
            return View(objlist);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Department dept)
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

        public IActionResult Edit(int? id)
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
        public IActionResult Edit(Department dept)
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

        public IActionResult Delete(int? id)
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
        public IActionResult DeleteDept(int? id)
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

        [HttpPost]
        public JsonResult AjaxMethod(string name)
        {
            DateTimeModel date = new DateTimeModel
            {
                DateTime = DateTime.Now.ToString()
            };
            return Json(date);
        }
    }
}
