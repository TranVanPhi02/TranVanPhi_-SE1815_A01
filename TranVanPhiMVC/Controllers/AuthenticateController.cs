using BusinessObjects;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace TranVanPhiMVC.Controllers
{
    public class AuthenticateController : Controller
    {
        private readonly ISystemAccountService _systemAccountService;

        public AuthenticateController(ISystemAccountService systemAccountService)
        {
            _systemAccountService = systemAccountService;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var accounts = await _systemAccountService.GetAllAccountsAsync();

            if (accounts == null || !accounts.Any())  
            {
                ViewBag.ErrorMessage = "No accounts found.";
            }

            return View(accounts);
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ToggleActivation(short id)
        {
            var isDisabled = await _systemAccountService.IsUserDisabledAsync(id);
            if (isDisabled)
            {
                await _systemAccountService.EnableUserAsync(id);
                TempData["SuccessMessage"] = "User has been enabled.";
            }
            else
            {
                await _systemAccountService.DisableUserAsync(id);
                TempData["SuccessMessage"] = "User has been disabled.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            try
            {
                var account = await _systemAccountService.LoginAsync(email, password);

                if (account != null)
                {
                    // Xác định role dựa trên AccountRole
                    string role = account.AccountRole switch
                    {
                        1 => "Staff",
                        2 => "Lecturer",
                        _ => "Admin"
                    };

                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, account.AccountName),
                new Claim(ClaimTypes.NameIdentifier, account.AccountId.ToString()),
                new Claim(ClaimTypes.Role, role)
            };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return RedirectToAction("Index", "Home");
                }
            }
            catch (UnauthorizedAccessException) 
            {
                ViewBag.ErrorMessage = "Your account has been disabled. Please contact the administrator.";
                return View();
            }

            ViewBag.ErrorMessage = "Invalid email or password!";
            return View();
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
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Authenticate");
        }
    }
}
