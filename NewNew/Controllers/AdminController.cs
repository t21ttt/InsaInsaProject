using NewNew.Data;
using NewNew.Models.Domain;
using NewNew.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace NewNew.Controllers
{
    public class AdminController : Controller
    {
        private readonly MVCDemoContext mvcDemoContext;

        public AdminController(MVCDemoContext mvcDemoDbContext)
        {
            this.mvcDemoContext = mvcDemoDbContext;
        }

        public async Task<IActionResult>Home()
        {
            return View();

        }
        public IActionResult about()
        {
            return View();
        }
       
        
        public async Task<IActionResult> adminlogin()
        {
            return View();
        }
        
        public async Task<IActionResult> adminsignup()
        {
            return View();
        }
       
        public async Task<IActionResult> Index()
        {
            var admin = await mvcDemoContext.admin.ToListAsync();
            return View(admin);
        }

 //This code is to activate  the member account when they register
        public ActionResult ActivateMember(int memberId)
        {
            var member = mvcDemoContext.Members.Find(memberId);
       
            if (member != null)
            {
                member.Status = MemberStatus.Active;
                mvcDemoContext.SaveChanges();
            }
            return RedirectToAction("Index","Member");
        }

        public ActionResult BlockMember(int memberId)
        {
            var member = mvcDemoContext.Members.Find(memberId);
            if (member != null)
            {
                member.Status = MemberStatus.Blocked;
                mvcDemoContext.SaveChanges();
            }
            return RedirectToAction("Index", "Member");
        }

        [HttpPost]
        public IActionResult adminlogin(AdminLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var dbContext = new MVCDemoContext())
                {
                    // Retrieve the member from the database using the provided email
                    var admin = mvcDemoContext.admin.FirstOrDefault(m => m.email == model.email);

                    if (admin != null && admin.password == model.Password)
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
            return View();
        }
        [HttpPost]
        public IActionResult adminsignup(AddAdminViewModel model)
        {
            if (ModelState.IsValid)
            {
        
        // Create a new user object and populate its properties with the form data
                var admin = new Admin
                {
                    //adminId=model.adminId,
                    adminFullName = model.adminFullName,
                    email = model.email,
                    phonNumber = model.phonNumber,
                    gender= model.gender,
                    password = model.password
                };

                // Add the new member to the database
                using (var dbContext = new MVCDemoContext())
                {
                    mvcDemoContext.admin.Add(admin);
                    mvcDemoContext.SaveChanges();
                }

                // Redirect to the login page after successful signup
                return RedirectToAction("userlogin", "MemberAccount");
            }

            // If we reach this point, the signup failed or the model is invalid
            // Return the signup view with the model to display validation errors
            return View(model);
        }
        public ActionResult Logout()
        {
            ViewData["Layout"] = "_Layout1";
            return RedirectToAction("userlogin", "MemberAccount"); // Assuming "Login" is the login action method in the "AccountController"
        }

    }
}
