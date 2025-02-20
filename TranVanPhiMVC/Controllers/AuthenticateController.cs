using BusinessObjects;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace TranVanPhiMVC.Controllers
{
    public class AuthenticateController : Controller
    {
        private readonly ISystemAccountService _systemAccountService;

        public AuthenticateController(ISystemAccountService systemAccountService)
        {
            _systemAccountService = systemAccountService;
        }

        public IActionResult Login()
        {
            return View();
        }

        /*  [HttpPost]
          public async Task<IActionResult> Login(string email, string password)
          {
              var account = await _systemAccountService.LoginAsync(email, password);
              if (account != null)
              {

                  if (account.AccountRole == 1) // Staff
                  {
                      return RedirectToAction("Index", "Home");
                  }
                  else if (account.AccountRole == 2) // Lecturer
                  {
                      return RedirectToAction("Index", "Home");
                  }
                  else // Admin
                  {
                      return RedirectToAction("Index", "Home");
                  }
              }
              else
              {
                  ViewBag.ErrorMessage = "Invalid email or password!";
                  return View();
              }
          }*/

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var account = await _systemAccountService.LoginAsync(email, password);
            if (account != null)
            {
                // Add roles to the claims based on the account role
                var roles = account.AccountRole == 1 ? new[] { "Staff" }
                           : account.AccountRole == 2 ? new[] { "Lecturer" }
                           : new[] { "Admin" };

                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, account.AccountName),
                new Claim(ClaimTypes.NameIdentifier, account.AccountId.ToString())
            };

                claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

                // Create the claims identity and sign the user in
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                // Redirect to the Home page after successful login
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid email or password!";
                return View();  // Stay on the Login page if authentication fails
            }
        }
        // Register GET action
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(string email, string password, string confirmPassword, int role)
        {
            if (password != confirmPassword)
            {
                ViewBag.ErrorMessage = "Passwords do not match!";
                return View();
            }

            var existingAccount = await _systemAccountService.GetAccountByEmailAsync(email);
            if (existingAccount != null)
            {
                ViewBag.ErrorMessage = "An account with this email already exists!";
                return View();
            }

            var newAccount = new SystemAccount
            {
                AccountEmail = email,
                AccountPassword = password,
                AccountRole = role
            };

            await _systemAccountService.RegisterAsync(newAccount);

            // Redirect to the login page after successful registration
            return RedirectToAction("Login", "SystemAccount");
        }

      
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Profile(short accountId)
        {
            var account = await _systemAccountService.GetProfileAsync(accountId);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }
   

        [HttpGet]
        public async Task<IActionResult> EditProfile(short accountId)
        {
            var account = await _systemAccountService.GetProfileAsync(accountId);
            if (account == null)
            {
                return NotFound(); 
            }

            return View(account); 
        }

        // Edit profile 
        [HttpPost]
        public async Task<IActionResult> EditProfile(SystemAccount updatedAccount)
        {
            if (ModelState.IsValid)
            {
                // Update the profile in the service
                await _systemAccountService.EditProfileAsync(updatedAccount);
                return RedirectToAction("Profile", new { accountId = updatedAccount.AccountId }); 
            }

            return View(updatedAccount); 
        }
        // Logout POST action
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // Sign out the user by clearing the authentication cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Authenticate");
        }
    }
}
