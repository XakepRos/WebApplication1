using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using WebApplication1.Models;
using System.Security.Claims;
using Microsoft.Win32;
using WebApplication1.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Authentication;

namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly DbaseContext _userContext;
        
        public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, DbaseContext dbaseContext)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _userContext = dbaseContext;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Register register)
        {
            if (ModelState.IsValid)
            {
                if ((await _userManager.FindByEmailAsync(register.Email)) == null)
                {
                    var user = new IdentityUser
                    {
                        Email = register.Email,
                        UserName = register.UserName,
                    };
                    var result = await _userManager.CreateAsync(user, register.Password);

                    if (result.Succeeded == false)
                    {
                        ModelState.AddModelError("Register", String.Join(" ", result.Errors.Select(x => x.Description)));
                        return View(register);
                    }

                    if (!await _roleManager.RoleExistsAsync("Admin"))
                    {
                        var roleResult = await _roleManager.CreateAsync(new IdentityRole() { Name = "Admin" });
                    }

                    user = await _userManager.FindByEmailAsync(register.Email);
                    await _userManager.AddToRoleAsync(user, "Admin");
                    //var claim = new Claim("Department", register.Department);
                    //await _userManager.AddClaimAsync(user, claim);
                    //if (result.Succeeded)
                    //{
                    //    return RedirectToAction("Login", "Account");
                    //}
                    /*
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    if (result.Succeeded)
                    {
                        //var confirmationLink = Url.ActionLink("Confirm Email", "Identity", new { userId = user.Id, @token = token });
                        //await _emailSender.SendEmailAsync("nabin.kaucha@gmail.com", user.Email, "Confirm your email address", confirmationLink);
                        return RedirectToAction("MFASetup");
                    }
                    */
                    ModelState.AddModelError("Register", String.Join(" ", result.Errors.Select(x => x.Description)));
                    //return View(register);
                    return RedirectToAction("Login");
                }
            }
            return View();
        }

       

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var user = await _userManager.FindByEmailAsync(userId);

            var confirmResult = await _userManager.ConfirmEmailAsync(user, token);
            if (confirmResult.Succeeded)
            {
                return RedirectToAction("Login");
            }
            return new NotFoundResult();
        }

        public IActionResult Login()
        {
            return View(new Login());
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(login.UserName, login.Password, login.RememberMe, false);
                if (result.Succeeded)
                {
                    //var user = await _userManager.FindByEmailAsync(login.UserName);
                    //if (await _userManager.IsInRoleAsync(user, "DepartmentUser"))
                    //{
                    //    return RedirectToAction("Index", "Department");
                    //}
                    //else if (await _userManager.IsInRoleAsync(user, "Admin"))
                    //{
                    //    return RedirectToAction("Index", "Dashboard");
                    //}
                    return RedirectToAction("Dashboard", "Home");
                }
                else
                {
                    ModelState.AddModelError("Login", "Cannot login.");

                }
            }
            return View(login);
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
       // [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePassword model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login");
                }

                // ChangePasswordAsync changes the user password
                var result = await _userManager.ChangePasswordAsync(user,
                    model.CurrentPassword, model.NewPassword);

                // The new password did not meet the complexity rules or
                // the current password is incorrect. Add these errors to
                // the ModelState and rerender ChangePassword view
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View();
                }
               // TempData["ResultOk"] = "Password changed successfully !";

                // Upon successfully changing the password refresh sign-in cookie
                await _signInManager.RefreshSignInAsync(user);
              
                return View("Login");
            }

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult RoleListView()
        {
            var roles = _userContext.Roles.ToList();
            return View(roles);           
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AddRole()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddRole(Role role)
        {
            if (ModelState.IsValid)
            {
                if (await _roleManager.RoleExistsAsync(role.RoleName))
                {
                    ModelState.AddModelError("RoleName", "Already Exists");
                    return View(role);
                }
                var result = await _roleManager.CreateAsync(new IdentityRole() { Name = role.RoleName });
                if (result.Succeeded)
                {
                    return RedirectToAction("RoleListView", "Account");
                }
                ModelState.AddModelError("Register", String.Join(" ", result.Errors.Select(x => x.Description)));
                return View(role);
            }
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult EditRole(string id)
       {
            if(String.IsNullOrEmpty(id))
            {
                return View();
            }
            else
            {
                //update
                var objFromDb = _userContext.Roles.FirstOrDefault(u => u.Id == id);
                return View(objFromDb);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRole(IdentityRole roleObj)
        {
            if (await _roleManager.RoleExistsAsync(roleObj.Name))
            {
                //error
                TempData[SuccessMessageViewModel.Error] = "Role already exists.";
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("EditRole");
            }
            if (string.IsNullOrEmpty(roleObj.Id))
            {
                //create
                await _roleManager.CreateAsync(new IdentityRole() { Name = roleObj.Name });
                TempData[SuccessMessageViewModel.Success] = "Role created successfully";
            }
            else
            {
                //update
                var objRoleFromDb = _userContext.Roles.FirstOrDefault(u => u.Id == roleObj.Id);
                if (objRoleFromDb == null)
                {
                    TempData[SuccessMessageViewModel.Error] = "Role not found.";
                    //return RedirectToAction(nameof(Index));
                    return RedirectToAction("RoleListView");
                }
                objRoleFromDb.Name = roleObj.Name;
                objRoleFromDb.NormalizedName = roleObj.Name.ToUpper();
                var result = await _roleManager.UpdateAsync(objRoleFromDb);
                TempData[SuccessMessageViewModel.Success] = "Role updated successfully";
            }
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("RoleListView");
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var objFromDb = _userContext.Roles.FirstOrDefault(u => u.Id == id);
            if (objFromDb == null)
            {
                TempData[SuccessMessageViewModel.Error] = "Role not found.";
                return RedirectToAction("RoleListView");
            }
            var userRolesForThisRole = _userContext.UserRoles.Where(u => u.RoleId == id).Count();
            if (userRolesForThisRole > 0)
            {
                TempData[SuccessMessageViewModel.Error] = "Cannot delete this role, since there are users assigned to this role.";
                return RedirectToAction("RoleListView");
            }
            await _roleManager.DeleteAsync(objFromDb);
            TempData[SuccessMessageViewModel.Success] = "Role deleted successfully.";
            return RedirectToAction("RoleListView");

        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateRoles()
        {
            return View();
        }

            [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task CreateRoles(IServiceProvider serviceProvider)
        {
            //initializing custom roles 
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            string[] roleNames = { "Admin", "Department-Head", "Department-User", "Normal-User" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                // ensure that the role does not exist
                if (!roleExist)
                {
                    //create the roles and seed them to the database: 
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // find the user with the admin email 
            var _user = await UserManager.FindByEmailAsync("admin@email.com");

            // check if the user exists
            if (_user == null)
            {
                //Here you could create the super admin who will maintain the web app
                var poweruser = new ApplicationUser
                {
                    UserName = "Admin",
                    Email = "admin@email.com",
                };
                string adminPassword = "p@$$w0rd";

                var createPowerUser = await UserManager.CreateAsync(poweruser, adminPassword);
                if (createPowerUser.Succeeded)
                {
                    //here we tie the new user to the role
                    await UserManager.AddToRoleAsync(poweruser, "Admin");

                }
            }
        }

        /*
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AddDepartment()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddDepartment(Department dept)
        {
            if (ModelState.IsValid)
            {
                var test = await _userContext.Department.FirstOrDefaultAsync(u => u.DepartmentName == dept.DepartmentName);
                if (test != null)
                {
                    ModelState.AddModelError("DepartmentName", "Already Exists");
                    return View(dept);
                }
                var result = await _userContext.Department.AddAsync(dept);
                _userContext.SaveChanges();
                return RedirectToAction("Dashboard", "Home");
            }
            return View();
        }
        */

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AddRoleToUser()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddRoleToUser(string email)
        {
            return View();
        }

        public async Task<IActionResult> AccessDenied()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> MFASetup(MultiFactorAuthenticator mfa)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                bool succeeded = await _userManager.VerifyTwoFactorTokenAsync(user, _userManager.Options.Tokens.AuthenticatorTokenProvider, mfa.Code);
                if (succeeded)
                {
                    await _userManager.SetTwoFactorEnabledAsync(user, true);
                }
                else
                {
                    ModelState.AddModelError("Verify", "Two Factor Authenicator could not be verified.");
                }
            }
            return View(mfa);
        }

        [HttpPost]
        public IActionResult ExternalLogin(string provider, string redirectUrl = null)
        {
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            var callBackUrl = Url.Action("ExternalLoginCallback");
            properties.RedirectUri = callBackUrl;
            return Challenge(properties, provider);
        }

        public async Task<IActionResult> ExternalLoginCallback()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            var emailClaim = info.Principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
            var user = new IdentityUser { Email = emailClaim.Value, UserName = emailClaim.Value };
            if (await _userManager.FindByEmailAsync(user.Email) == null)
            {
                await _userManager.CreateAsync(user);
            }
            await _userManager.AddLoginAsync(user, info);
            await _signInManager.SignInAsync(user, false);

            return RedirectToAction("Index", "Home");
        }
    }
}
