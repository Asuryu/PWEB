using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PWEB_AulasP_2223.ViewModels;
using PWEB.Models;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PWEB.Controllers
{
    public class UserRolesManagerController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRolesManagerController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            List<UserRolesViewModel> listaUtilizadores = new List<UserRolesViewModel>();

            foreach (var user in users)
            {
                UserRolesViewModel userRolesViewModel = new UserRolesViewModel();
                userRolesViewModel.PrimeiroNome = user.PrimeiroNome;
                userRolesViewModel.UltimoNome = user.UltimoNome;
                userRolesViewModel.Email = user.Email;
                userRolesViewModel.UserId = user.Id;
                userRolesViewModel.UserName = user.UserName;
                userRolesViewModel.Roles = await GetUserRoles(user);
                listaUtilizadores.Add(userRolesViewModel);
            }
            return View(listaUtilizadores);
        }

        private async Task<List<string>> GetUserRoles(ApplicationUser user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }

        public async Task<IActionResult> Details(string? userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            
            var roles = await _roleManager.Roles.ToListAsync();
            List<ManageUserRolesViewModel> manageUserRolesViewModels = new List<ManageUserRolesViewModel>();
            foreach(var role in roles)
            {
                ManageUserRolesViewModel manage = new ManageUserRolesViewModel();
                manage.RoleId = role.Id;
                manage.RoleName = role.Name;
                manage.Selected = await _userManager.IsInRoleAsync(user, role.Name);
                manageUserRolesViewModels.Add(manage);
            }
            return View(manageUserRolesViewModels);
        }

        [HttpPost]
        public async Task<IActionResult> Details(List<ManageUserRolesViewModel> model, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if(user != null)
            {
                foreach (var role in model)
                {
                    if (role.Selected) await _userManager.AddToRoleAsync(user, role.RoleName);
                    else await _userManager.RemoveFromRoleAsync(user, role.RoleName);
                }
            }
            return RedirectToAction("Index");
        }
    }
}