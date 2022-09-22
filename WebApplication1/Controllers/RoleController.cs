using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<ApplicationRole> roleManager;

        public RoleController(RoleManager<ApplicationRole> roleManager)
        {
            this.roleManager = roleManager;
        }


        [HttpGet]
        public IActionResult Index()
        {
            List<RoleListViewModel> model = new List<RoleListViewModel>();
            model = roleManager.Roles.Select(r => new RoleListViewModel
            {
                RoleName = r.Name,
                Id = r.Id,
                Description = r.Description,
                NumberOfUsers = r.Users.Count()
            }).ToList();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddEditApplicationRole(string id)
        {
            RoleListViewModel model = new RoleListViewModel();
            if (!String.IsNullOrEmpty(id))
            {
                ApplicationRole applicationRole = await roleManager.FindByIdAsync(id);
                if (applicationRole != null)
                {
                    model.Id = applicationRole.Id;
                    model.RoleName = applicationRole.Name;
                    model.Description = applicationRole.Description;
                }
            }
            return PartialView("_AddEditApplicationRole", model);
        }

        [HttpPost]
        public async Task<IActionResult> AddEditApplicationRole(string id, RoleListViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool isExist = !String.IsNullOrEmpty(id);
                ApplicationRole applicationRole = isExist ? await roleManager.FindByIdAsync(id) :
               new ApplicationRole
               {
                   CreatedDate = DateTime.UtcNow
               };
                applicationRole.Name = model.RoleName;
                applicationRole.Description = model.Description;
                applicationRole.IPAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                IdentityResult roleRuslt = isExist ? await roleManager.UpdateAsync(applicationRole)
                                                    : await roleManager.CreateAsync(applicationRole);
                if (roleRuslt.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }
    }
}
