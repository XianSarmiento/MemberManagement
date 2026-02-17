using Microsoft.AspNetCore.Mvc;
using MemberManagement.Application.Branches;
using MemberManagement.Domain.Interfaces;
using MemberManagement.SharedKernel.Constant;

namespace MemberManagement.Web.Controllers
{
    public class BranchesController : Controller
    {
        private readonly GetBranchesHandler _getHandler;
        private readonly CreateBranchHandler _createHandler;
        private readonly UpdateBranchHandler _updateHandler;
        private readonly IBranchRepository _repository;

        public BranchesController(
            GetBranchesHandler getHandler,
            CreateBranchHandler createHandler,
            UpdateBranchHandler updateHandler,
            IBranchRepository repository)
        {
            _getHandler = getHandler;
            _createHandler = createHandler;
            _updateHandler = updateHandler;
            _repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            var branches = await _getHandler.Handle();
            return View(branches);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name, string address, string branchCode)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(branchCode))
            {
                TempData["ErrorMessage"] = OperationMessage.Error.InvalidInput;
                return RedirectToAction(nameof(Index));
            }

            try
            {
                await _createHandler.Handle(name, address, branchCode);
                TempData["SuccessMessage"] = OperationMessage.Branch.Created;
            }
            catch
            {
                TempData["ErrorMessage"] = OperationMessage.Error.SaveFailed;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, string name, string address, string branchCode)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(branchCode))
            {
                TempData["ErrorMessage"] = OperationMessage.Error.InvalidInput;
                return RedirectToAction(nameof(Index));
            }

            try
            {
                await _updateHandler.Handle(id, name, address, branchCode);
                TempData["SuccessMessage"] = OperationMessage.Branch.Updated;
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