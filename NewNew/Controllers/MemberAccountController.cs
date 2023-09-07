using NewNew.Data;
using NewNew.Models.Domain;
using NewNew.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PayPalCheckoutSdk.Orders;

namespace NewNew.Controllers
{
    public class MemberAccountController : Controller
    {
        private readonly MVCDemoContext mvcDemoContext;

        public MemberAccountController(MVCDemoContext mvcDemoDbContext)
        {
            this.mvcDemoContext = mvcDemoDbContext;
        }

        public async Task<IActionResult> userlogin()
        {
            ViewData["Layout"] = "_Layout1";
            return View();
        }
        public async Task<IActionResult> usersignup()
        {
            ViewData["Layout"] = "_Layout1";
            return View();
        }

        //public IActionResult Login()
        //{
        //    ViewData["Layout"] = "_Layout1";
        //    return View();
        //}profile


        [HttpPost]
        public async Task<IActionResult> userlogin(LoginViewModel model)
        {
            ViewData["Layout"] = "_Layout1";
            if (ModelState.IsValid)
            {
                var member = mvcDemoContext.Members.FirstOrDefault(m => m.email == model.email);

                if (member != null && member.memberPassword == model.memberPassword)
                {
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, member.memberId.ToString()),
                // Add other claims if needed
            };

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity));

                    return RedirectToAction("Index", "HomeMember");
                }
                else
                {
                    var admin = mvcDemoContext.admin.FirstOrDefault(m => m.email == model.email);

                    if (admin != null && admin.password == model.memberPassword)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid email or password");
                    }
                }
            }

            return View(model);
        }


        public IActionResult SignUp()
        {
            ViewData["Layout"] = "_Layout1";
            return View();
        }

        [HttpPost]
        public IActionResult usersignup(SignupViewModel model)
        {
            ViewData["Layout"] = "_Layout1";
            if (ModelState.IsValid)
            {
                // Create a new user object and populate its properties with the form data
                var member = new Member
                {
                    memberFullName = model.memberFullName,
                    gender = model.gender,
                    email = model.email,
                    phoneNumber = model.phoneNumber,
                    memberPassword = model.memberPassword,
                    Status = MemberStatus.Blocked
                };

                // Add the new member to the database
                using (var dbContext = new MVCDemoContext())
                {
                    mvcDemoContext.Members.Add(member);
                    mvcDemoContext.SaveChanges();
                }

                // Redirect to the login page after successful signup
                return RedirectToAction("userlogin", "MemberAccount");
            }

            // If we reach this point, the signup failed or the model is invalid
            // Return the signup view with the model to display validation errors
            return View(model);
        }

        [Authorize]
        public IActionResult Profile()
        {
            ViewData["Layout"] = "_Layout1";

            // Retrieve the member ID from the authenticated user's claims
            var memberIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(memberIdClaim) || !int.TryParse(memberIdClaim, out int memberId))
            {
                // Handle the case if the member ID claim is missing or cannot be parsed
                return View("Error");
            }


            // Retrieve the member from the database using the member ID
            var member = mvcDemoContext.Members.FirstOrDefault(m => m.memberId == memberId);

            if (member == null)
            {
                // Handle the case if the member is not found in the database
                return View("Error");
            }

            // Pass the member object to the profile view
            return View("Profile", member);
        }


        //Edit Personal Information
        [HttpPost]
        public IActionResult EditProfile(Member member)
        {
            ViewData["Layout"] = "_Layout1";
            // Check if the user is authenticated
            if (!User.Identity.IsAuthenticated)
            {
                // Redirect to the login page or display an error if the user is not logged in
                return RedirectToAction("userlogin", "MemberAccount");
            }

            // Retrieve the member ID from the authenticated user's claims
            var memberIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(memberIdClaim))
            {
                // Handle the case if the member ID claim is missing
                return View("Error");
            }

            // Parse the member ID to an integer
            if (!int.TryParse(memberIdClaim, out int memberId))
            {
                // Handle the case if the member ID cannot be parsed
                return View("Error");
            }

            // Retrieve the existing member from the database using the member ID
            var existingMember = mvcDemoContext.Members.FirstOrDefault(m => m.memberId == memberId);

            if (existingMember == null)
            {
                // Handle the case if the member is not found in the database
                return View("Error");
            }

            // Update the member properties with the edited values
            existingMember.memberFullName = member.memberFullName;
            existingMember.gender = member.gender;
            existingMember.email = member.email;
            existingMember.phoneNumber = member.phoneNumber;
            existingMember.memberPassword = member.memberPassword;


            // Save the changes to the database
            mvcDemoContext.SaveChanges();

            // Redirect to the profile page after successful update
            return RedirectToAction("Profile", "MemberAccount");
        }
        // This to Reset my Password

        public ActionResult Logout()
        {
            ViewData["Layout"] = "_Layout1";
            return RedirectToAction("userlogin", "MemberAccount"); // Assuming "Login" is the login action method in the "AccountController"
        }


        //public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await Member.FindByEmailAsync(model.Email);

        //        if (user != null && await Member.IsEmailConfirmedAsync(user))
        //        {
        //            var token = await Member.GeneratePasswordResetTokenAsync(user);

        //            var passwordResetLink = Url.Action("ResetPassword", "Account",
        //                new { email = model.Email, token = token }, Request.Scheme);

        //            logger.LogWarning(passwordResetLink);

        //            return View("ResetPasswordConfirmation");
        //        }

        //        return View("ResetPasswordConfirmation");
        //    }

        //    return View(model);
        //}

        //[HttpGet]
        //[AllowAnonymous]
        //public IActionResult ResetPassword(string token, string email)
        //{
        //    if (token == null || email == null)
        //    {
        //        ModelState.AddModelError("", "Invalid Password reset token");
        //        return View();
        //    }

        //    var model = new ResetPasswordViewModel { Token = token, Email = email };
        //    return View(model);


    }
}





