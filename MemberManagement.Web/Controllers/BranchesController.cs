using Microsoft.AspNetCore.Mvc;
using MemberManagement.Application.Branches;
using MemberManagement.Domain.Interfaces;
using MemberManagement.SharedKernel.Constant; // Added this

namespace MemberManagement.Web.Controllers
{
    public class BranchesController : Controller
    {
        private readonly GetBranchesHandler _getHandler;
        private readonly CreateBranchHandler _createHandler;
        private readonly IBranchRepository _repository;

        public BranchesController(
            GetBranchesHandler getHandler,
            CreateBranchHandler createHandler,
            IBranchRepository repository)
        {
            _getHandler = getHandler;
            _createHandler = createHandler;
            _repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            var branches = await _getHandler.Handle();
            return View(branches);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                TempData["ErrorMessage"] = OperationMessage.Error.InvalidInput;
                return RedirectToAction(nameof(Index));
            }

            try
            {
                await _createHandler.Handle(name);
                TempData["SuccessMessage"] = OperationMessage.Branch.Created;
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
                TempData["SuccessMessage"] = OperationMessage.Branch.Deleted;
            }
            catch
            {
                TempData["ErrorMessage"] = OperationMessage.Error.NotFound;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}