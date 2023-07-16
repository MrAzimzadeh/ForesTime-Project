using ForestTime.WebUI.DTOs;
using ForestTime.WebUI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace ForestTime.WebUI.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<User> _userManager;
        //login ucun ist edilir,register olub olmadigini yoxlaya bilirsiniz
        private readonly SignInManager<User> _signInManager;
        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task <IActionResult> Login(LoginDTO loginDTO)
        {
            var findUser = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (findUser == null)
            {
                return RedirectToAction("Login");
            }
                Microsoft.AspNetCore.Identity.SignInResult result=await _signInManager.PasswordSignInAsync(findUser,loginDTO.Password,false,false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(loginDTO);
        }
        //bu html seh acir
        public IActionResult Register()
        {
            return View();
        }
        //bu register htmlde post metodunu ise salir
        [HttpPost]
        //async bir birini gozleyen method yaratmaq ucun
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            User user = new()
            {
                FirstName = registerDTO.FirstName,
                LastName = registerDTO.LastName,
                PhotoUrl="/image/avatar.png",
                //bu hisse identityden gelir doldurmaq mecburiyyetindeyik
                UserName=registerDTO.Email,
                Email = registerDTO.Email
            };

            IdentityResult result=await _userManager.CreateAsync(user,registerDTO.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("Login");
            }
            return View(registerDTO);
        }
    }
}
