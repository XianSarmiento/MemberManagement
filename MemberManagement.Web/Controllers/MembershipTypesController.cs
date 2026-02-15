using Microsoft.AspNetCore.Mvc;
using MemberManagement.Application.MembershipTypes;
using MemberManagement.Domain.Interfaces;
using MemberManagement.SharedKernel.Constant;

namespace MemberManagement.Web.Controllers
{
    public class MembershipTypesController : Controller
    {
        private readonly GetMembershipTypesHandler _getHandler;
        private readonly CreateMembershipTypeHandler _createHandler;
        private readonly IMembershipTypeRepository _repository;

        public MembershipTypesController(
            GetMembershipTypesHandler getHandler,
            CreateMembershipTypeHandler createHandler,
            IMembershipTypeRepository repository)
        {
            _getHandler = getHandler;
            _createHandler = createHandler;
            _repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            var membershipTypes = await _getHandler.Handle();
            return View(membershipTypes);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name, decimal fee)
        {
            if (string.IsNullOrWhiteSpace(name) || fee < 0)
            {
                TempData["ErrorMessage"] = OperationMessage.Error.InvalidInput;
                return RedirectToAction(nameof(Index));
            }

            try
            {
                await _createHandler.Handle(name, fee);
                TempData["SuccessMessage"] = OperationMessage.Membership.Created;
            }
            catch
            {
                TempData["ErrorMessage"] = OperationMessage.Error.SaveFailed;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _repository.DeleteAsync(id);
                TempData["SuccessMessage"] = OperationMessage.Membership.Deleted;
            }
            catch
            {
                TempData["ErrorMessage"] = OperationMessage.Error.NotFound;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}