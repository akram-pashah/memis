using MEMIS.Data;
using MEMIS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MEMIS.Controllers
{
  public class UserManagementController : Controller
  {
    private readonly AppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserManagementController(AppDbContext context, UserManager<ApplicationUser> userManager)
    {
      _context = context;
      _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
      var users = await _userManager.Users.ToListAsync();
      return View(users);
    }

    public IActionResult Register()
    {
      ViewData["intDept"] = new SelectList(_context.Departments, "intDept", "deptName");
      ViewData["intDir"] = new SelectList(_context.Directorates, "intDir", "dirName");
      ViewData["intRegion"] = new SelectList(_context.Region, "intRegion", "regionName");
      return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(Register model)
    {
      if (!ModelState.IsValid)
      {
        return Json(new
        {
          success = false,
          errors = ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
        });
      }

      var user = new ApplicationUser
      {
        UserName = model.Username,
        Email = model.Email,
        intDept = model.intDept,
        intDir = model.intDir,
        intRegion = model.intRegion
      };

      var result = await _userManager.CreateAsync(user, model.Password);

      if (result.Succeeded)
      {
        return Json(new { success = true });
      }
      else
      {
        foreach (var error in result.Errors)
        {
          ModelState.AddModelError(string.Empty, error.Description);
        }
        return Json(new
        {
          success = false,
          errors = ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
        });
      }
    }


    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
      var user = await _userManager.FindByIdAsync(id);
      if (user == null)
      {
        return NotFound();
      }

      var model = new EditUserViewModel
      {
        Id = user.Id,
        Username = user.UserName,
        Email = user.Email,
        intDept = user.intDept,
        intDir = user.intDir,
        intRegion = user.intRegion
      };

      ViewData["intDept"] = new SelectList(_context.Departments, "intDept", "deptName", model.intDept);
      ViewData["intDir"] = new SelectList(_context.Directorates, "intDir", "dirName", model.intDir);
      ViewData["intRegion"] = new SelectList(_context.Region, "intRegion", "regionName", model.intRegion);

      return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditUserViewModel model)
    {
      if (!ModelState.IsValid)
      {
        return Json(new
        {
          success = false,
          errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
        });
      }

      try
      {
        var user = await _userManager.FindByIdAsync(model.Id);
        if (user == null)
        {
          return Json(new { success = false, message = "User not found." });
        }

        user.UserName = model.Username;
        user.Email = model.Email;
        user.intDept = model.intDept;
        user.intDir = model.intDir;
        user.intRegion = model.intRegion;

        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
          return Json(new
          {
            success = false,
            errors = updateResult.Errors.Select(e => e.Description)
          });
        }

        if (!string.IsNullOrEmpty(model.Password))
        {
          var removePasswordResult = await _userManager.RemovePasswordAsync(user);
          if (!removePasswordResult.Succeeded)
          {
            return Json(new
            {
              success = false,
              errors = removePasswordResult.Errors.Select(e => e.Description)
            });
          }

          var addPasswordResult = await _userManager.AddPasswordAsync(user, model.Password);
          if (!addPasswordResult.Succeeded)
          {
            return Json(new
            {
              success = false,
              errors = addPasswordResult.Errors.Select(e => e.Description)
            });
          }
        }

        return Json(new { success = true });

      }
      catch (Exception ex)
      {
        return Json(new { success = false, message = "Failed to change password." });
      }
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
      var user = await _userManager.FindByIdAsync(id);
      if (user == null)
      {
        return NotFound();
      }
      var result = await _userManager.DeleteAsync(user);
      if (result.Succeeded)
      {
        return Json(new { success = true });
      }
      return Json(new { success = false });
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword)
    {
      var user = await _userManager.GetUserAsync(User);

      if (user == null)
      {
        return Json(new { success = false, message = "User is not logged in." });
      }

      if (string.IsNullOrEmpty(currentPassword) || string.IsNullOrEmpty(newPassword))
      {
        return Json(new { success = false, message = "Invalid request. Both current and new passwords are required." });
      }

      var passwordCheck = await _userManager.CheckPasswordAsync(user, currentPassword);
      if (!passwordCheck)
      {
        return Json(new { success = false, message = "Current password is incorrect." });
      }

      var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
      if (result.Succeeded)
      {
        return Json(new { success = true, message = "Password changed successfully." });
      }
      else
      {
        return Json(new
        {
          success = false,
          errors = result.Errors.Select(e => e.Description)
        });
      }
    }

  }

  public static class HttpRequestExtensions
  {
    public static bool IsAjaxRequest(this HttpRequest request)
    {
      if (request == null)
      {
        throw new ArgumentNullException(nameof(request));
      }
      if (request.Headers != null)
      {
        return request.Headers["X-Requested-With"] == "XMLHttpRequest";
      }
      return false;
    }
  }
}
