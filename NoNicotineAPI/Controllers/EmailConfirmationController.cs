using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace NoNicotineAPI.Controllers
{
    public class EmailConfirmationController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        public EmailConfirmationController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IActionResult> Index(string confirmationToken, string email)
        {
            return View("ConfirmEmail");
            var user = await _userManager.FindByEmailAsync(email);
            var result = await _userManager.ConfirmEmailAsync(user, HttpUtility.UrlDecode(confirmationToken));
            if (result.Succeeded)
            {
                return View("ConfirmEmail");
            }
            return View("Error");
        }
    }
}
