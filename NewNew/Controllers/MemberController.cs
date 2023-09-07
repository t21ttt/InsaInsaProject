using NewNew.Data;
using NewNew.Models;
using NewNew.Models.Domain;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace NewNew.Controllers
{
    public class MemberController : Controller
    {
        private readonly MVCDemoContext _mvcDemoContext;

        public MemberController(MVCDemoContext mvcDemoDbContext)
        {
            _mvcDemoContext = mvcDemoDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var members = await _mvcDemoContext.Members.ToListAsync();
            return View(members);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddMemberViewModel model)
        {
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
                _mvcDemoContext.Members.Add(member);
                _mvcDemoContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> View(int id)
        {
            var member = await _mvcDemoContext.Members.FindAsync(id);

            if (member == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new UpdateMemberViewModel()
            {
                memberId = member.memberId,
                memberFullName = member.memberFullName,
                gender = member.gender,
                email = member.email,
                phoneNumber = member.phoneNumber,
                memberPassword = member.memberPassword,
            };

            return await Task.Run(() => View("View", viewModel));
        }

        [HttpPost]
        public async Task<IActionResult> View( UpdateMemberViewModel model)
        {
            var member = await _mvcDemoContext.Members.FindAsync(model.memberId);

            if (member == null)
            {
                return RedirectToAction(nameof(Index));
            }

            member.memberFullName = model.memberFullName;
            member.gender = model.gender;
            member.email = model.email;
            member.phoneNumber = model.phoneNumber;
            member.memberPassword = model.memberPassword;


            _mvcDemoContext.Members.Update(member);
            await _mvcDemoContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public async Task<IActionResult> Delete(UpdateMemberViewModel model)
        {
            var member = await _mvcDemoContext.Members.FindAsync(model.memberId);
            if (member != null)
            {
                _mvcDemoContext.Members.Remove(member);
                await _mvcDemoContext.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}