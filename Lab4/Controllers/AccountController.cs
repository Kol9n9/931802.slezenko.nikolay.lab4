using Lab4.Models.User;
using Lab4.Models.User.Login;
using Lab4.Models.User.ResetPassword;
using Lab4.Models.User.SignUp;
using Lab4.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Lab4.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = AccountFileStorage.LoadData(model.Email,model.Password);

                if (user != null)
                {
                    await Authenticate(model.Email);

                    return RedirectToAction("Profile", "Account");
                }

                ModelState.AddModelError("", "Incorrect email or password");
            }

            return View();
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View(new SignUpStep1Model());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(SignUpStep1Model model)
        {
            if (ModelState.IsValid)
            {
                HttpContext.Session.Set("SignUpStep1", model);

                return View("SignUpStep2");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUpStep2(SignUpStep2Model model)
        {
            if (ModelState.IsValid)
            {
                var step1Model = HttpContext.Session.Get<SignUpStep1Model>("SignUpStep1");

                if(!AccountFileStorage.IsExist(model.Email))
                {
                    AccountFileStorage.SaveData(new UserModel
                    {
                        FirstName = step1Model.FirstName,
                        LastName = step1Model.LastName,
                        Email = model.Email,
                        BirthDay = step1Model.BirthDay,
                        Gender = step1Model.Gender,
                        Password = model.Password
                    });

                    await Authenticate(model.Email);

                    return RedirectToAction("Profile", "Account");
                }

                ModelState.AddModelError("Email", "Email is already registered");
            }

            return View(model);
        }
        private async Task Authenticate(string userName)
        {
            var claims = new List<Claim> {
                new (ClaimsIdentity.DefaultNameClaimType, userName)
            };

            var id = new ClaimsIdentity(claims, "ApplicationCookie",
                ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var user = AccountFileStorage.LoadData(HttpContext.User.Identity.Name, null, false);
            if (user is null)
            {
                return RedirectToAction("Logout");
            }
            else
            {

                return View(new Profile
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    BirthDay = user.BirthDay,
                    Password = user.Password,
                    Gender = user.Gender
                });
            }
        }

        [HttpGet]
        public IActionResult Reset()
        {
            return View();
        }

        public async Task<IActionResult> Reset(ResetStep1Model model, string action)
        {
            if (ModelState.IsValid)
            {
                if (action == "Send me a code")
                {
                    var user = AccountFileStorage.LoadData(model.Email, null, false);
                    if (user is null)
                    {
                        ModelState.AddModelError("Email", "Email was not found");
                    }
                    else
                    {
                        var code = new Random().Next(0, 10000);
                        HttpContext.Session.Set("ResetCode", code);
                        HttpContext.Session.Set("ResetEmail", model.Email);
                        ModelState.AddModelError("Email", $"Code: {code:D4}");
                    }
                }
                else
                {
                    return View("ResetStep2");
                }

            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetStep2(ResetStep2Model model)
        {
            if (ModelState.IsValid)
            {
                int code = HttpContext.Session.Get<int>("ResetCode");

                if (code == int.Parse(model.Code))
                {
                    return View("ResetStep3");
                }
                else
                {
                    ModelState.AddModelError("Code", "Invalid code");
                }
            }

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ResetStep3(ResetStep3Model model)
        {
            if (ModelState.IsValid)
            {
                var email = HttpContext.Session.Get<string>("ResetEmail");
                if(email == default)
                {
                    return RedirectToAction("Reset", "Account");
                }
                var user = AccountFileStorage.LoadData(email, null, false);
                AccountFileStorage.SaveData(new UserModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    BirthDay = user.BirthDay,
                    Gender = user.Gender,
                    Password = model.Password
                });
                ModelState.AddModelError("Password", "Password changed");
            }

            return View(model);
        }
    }
}
