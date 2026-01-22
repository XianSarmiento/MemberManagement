using MemberManagement.Application.Services;
using MemberManagement.Domain.Entities;
using MemberManagement.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MemberManagement.Web.Controllers
{
    public class MembersController : Controller
    {
        private readonly IMemberService memberService;

        public MembersController(IMemberService memberService)
        {
            this.memberService = memberService;
        }

        public async Task<IActionResult> Index()
        {
            var members = await memberService.GetActiveMembersAsync();

            // add this, right after creating viewmodel
            var vm = members.Select(m => new MemberVM
            {
                MemberID = m.MemberID,
                FirstName = m.FirstName,
                LastName = m.LastName,
                BirthDate = m.BirthDate,
                Address = m.Address,
                Branch = m.Branch,
                ContactNo = m.ContactNo,
                EmailAddress = m.EmailAddress
            });

            return View(vm);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MemberVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var member = new Member
            {
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                BirthDate = vm.BirthDate,
                Address = vm.Address,
                Branch = vm.Branch,
                ContactNo = vm.ContactNo,
                EmailAddress = vm.EmailAddress
            };

            await this.memberService.CreateAsync(member);
            return RedirectToAction(nameof(Index));
        }

    }
}
