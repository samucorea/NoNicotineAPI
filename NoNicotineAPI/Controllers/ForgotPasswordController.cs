using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace NoNicotineAPI.Controllers
{
    public class ForgotPasswordController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public ForgotPasswordController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Reset(string token, string email, string password1, string password2)
        {
            if(password1 != password2 || !IsPasswordValid(password1))
            {
                return View("Error");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return View("Error");
            }
            var result = await _userManager.ResetPasswordAsync(user, HttpUtility.UrlDecode(token), password1);
            if (!result.Succeeded)
            {
                return View("Error");
            }

            return View("Success");
        }

        private static bool IsPasswordValid(string password)
        {
            return password.Length > 5 && password.Any(c => char.IsDigit(c));
        }
    }
}
