using MemberManagement.Application.Services;
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
            return View();
        }
    }
}
