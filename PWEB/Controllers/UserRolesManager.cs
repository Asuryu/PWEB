using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PWEB.Models;
using PWEB.ViewModels;

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
                userRolesViewModel.Roles = await GetUserRoles(user);
                listaUtilizadores.Add(userRolesViewModel);
            }
            return View(listaUtilizadores);
        }

        private async Task<List<string>> GetUserRoles(ApplicationUser user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }
    }
}

