using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class CategoryController : Controller
    {
        private readonly DbaseContext _context;
        public CategoryController(DbaseContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            IEnumerable<CategoryModel> objlist = _context.Categories;
            return View(objlist);
        }

        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCategory(CategoryModel category)
        {
            if (ModelState.IsValid)
            {
                category.CategoryCreatedOn = DateTime.Now;
                _context.Categories.Add(category);
                _context.SaveChanges();
                TempData["ResultOk"] = "Category Created Successfully !";
                return RedirectToAction("Index");
            }

            return View(category);
        }

        public IActionResult EditCategory(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var categoryfromdb = _context.Categories.Find(id);

            if (categoryfromdb == null)
            {
                return NotFound();
            }
            return View(categoryfromdb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditCategory(CategoryModel category)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Update(category);
                _context.SaveChanges();
                TempData["ResultOk"] = "Category Updated Successfully !";
                return RedirectToAction("Index");
            }

            return View(category);
        }

        public IActionResult DeleteCategory(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var categoryfromdb = _context.Categories.Find(id);

            if (categoryfromdb == null)
            {
                return NotFound();
            }
            return View(categoryfromdb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int? id)
        {
            var deleteCategory = _context.Categories.Find(id);
            if (deleteCategory == null)
            {
                return NotFound();
            }
            _context.Categories.Remove(deleteCategory);
            _context.SaveChanges();
            TempData["ResultOk"] = "Category Deleted Successfully !";
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
