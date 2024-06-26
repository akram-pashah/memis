﻿using MEMIS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MEMIS.Data; 
using Microsoft.AspNetCore.Mvc.Rendering;
using cloudscribe.Pagination.Models;
using MEMIS.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace MEMIS.Controllers
{
    public class LoginController : Controller
    {
        private readonly Data.AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager; 
        public LoginController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, Data.AppDbContext dbContext, IPasswordHasher<ApplicationUser> _passwordHasher, Data.AppDbContext context)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager; 
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.FYear = _context.FYears == null ? new List<FYear>() : await _context.FYears.ToListAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(LoginAuth model, string? returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                HttpContext.Session.SetString("FYEAR", model.fyear.ToString());
                var identityResult = await _signInManager.PasswordSignInAsync(model.userName, model.password, false,false);
                if (identityResult.Succeeded)
                {
                    if (returnUrl == null || returnUrl == "/")
                    {
                        return RedirectToAction("Home", "Login");
                    }
                    else
                        return Redirect(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("", "Username or password incorrect!");
                }
               
            }
            else
            {
                ModelState.AddModelError("", "Username or password incorrect!");
            }
            return View();
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
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    UserName = model.Username,
                    Email = model.Email,
                    intDept = model.intDept,
                   intDir=model.intDir                     
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    await _signInManager.SignInAsync(user, false);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }
            return RedirectToAction("Register", "Login");
        }
        public IActionResult Logout()
        {
            return View();
        }
        public async Task<IActionResult> LogoutPage()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }
        public IActionResult DoNotLogout()
        {
            return RedirectToAction("Index", "Home");
        }
        public async Task OnGetAsync(string? returnUrl = null)
        {

        }
        public IActionResult Home()
        {
            return View();
        }
        public IActionResult AdminHome()
        {
            return View();
        }
        public IActionResult GISHome()
        {
            return View();
        }
        public IActionResult MEHome()
        {
            return View();
        }
        public IActionResult PlanHome()
        {
            return View();
        }
        public IActionResult RiskHome()
        {
            return View();
        }
        public IActionResult ProjectHome()
        {
            return View();
        }
    }
}
