using Microsoft.AspNetCore.Mvc;
using MemberManagement.Application.Branches;
using MemberManagement.Domain.Interfaces;
using MemberManagement.SharedKernel.Constant;

namespace MemberManagement.Web.Controllers;

public class BranchesController(
    GetBranchesHandler getHandler,
    CreateBranchHandler createHandler,
    UpdateBranchHandler updateHandler,
    IBranchRepository repository
) : Controller
{
    private readonly GetBranchesHandler _getHandler = getHandler;
    private readonly CreateBranchHandler _createHandler = createHandler;
    private readonly UpdateBranchHandler _updateHandler = updateHandler;
    private readonly IBranchRepository _repository = repository;

    public async Task<IActionResult> Index()
    {
        var branches = await _getHandler.Handle();
        return View(branches);
    }

    [HttpPost]
    public async Task<IActionResult> Create(string name, string branchCode, string? address)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(branchCode))
        {
            TempData["ErrorMessage"] = OperationMessage.Error.InvalidInput;
            return RedirectToAction(nameof(Index));
        }

        address ??= string.Empty;

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
    public async Task<IActionResult> Update(int id, string name, string branchCode, string? address)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(branchCode))
        {
            TempData["ErrorMessage"] = OperationMessage.Error.InvalidInput;
            return RedirectToAction(nameof(Index));
        }

        address ??= string.Empty;

        try
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing != null)
            {
                string currentAddress = existing.Address ?? string.Empty;

                if (existing.Name == name &&
                    existing.BranchCode == branchCode &&
                    currentAddress == address)
                {
                    TempData["SuccessMessage"] = OperationMessage.Branch.NoChanges;
                    return RedirectToAction(nameof(Index));
                }
            }

            await _updateHandler.Handle(id, name, address, branchCode);
            TempData["SuccessMessage"] = OperationMessage.Branch.Updated;
        }
        catch (KeyNotFoundException)
        {
            TempData["ErrorMessage"] = OperationMessage.Error.NotFound;
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

    [HttpPost]
    public async Task<IActionResult> ToggleStatus(int id)
    {
        try
        {
            var branch = await _repository.GetByIdAsync(id);
            if (branch == null)
            {
                TempData["ErrorMessage"] = OperationMessage.Error.NotFound;
                return RedirectToAction(nameof(Index));
            }

            if (branch.IsActive)
                branch.Deactivate();
            else
                branch.Activate();

            await _repository.UpdateAsync(branch);
            TempData["SuccessMessage"] = OperationMessage.Branch.Updated;
        }
        catch
        {
            TempData["ErrorMessage"] = OperationMessage.Error.SaveFailed;
        }

        return RedirectToAction(nameof(Index));
    }
}
