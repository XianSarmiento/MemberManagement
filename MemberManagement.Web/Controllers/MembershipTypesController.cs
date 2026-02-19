using Microsoft.AspNetCore.Mvc;
using MemberManagement.Application.MembershipTypes;
using MemberManagement.Domain.Interfaces;
using MemberManagement.SharedKernel.Constant;
using FluentValidation; // Required for catching ValidationException

namespace MemberManagement.Web.Controllers;

public class MembershipTypesController(
    GetMembershipTypesHandler getHandler,
    CreateMembershipTypeHandler createHandler,
    UpdateMembershipTypeHandler updateHandler,
    IMembershipTypeRepository repository
) : Controller
{
    private readonly GetMembershipTypesHandler _getHandler = getHandler;
    private readonly CreateMembershipTypeHandler _createHandler = createHandler;
    private readonly UpdateMembershipTypeHandler _updateHandler = updateHandler;
    private readonly IMembershipTypeRepository _repository = repository;

    public async Task<IActionResult> Index()
    {
        var membershipTypes = await _getHandler.Handle();
        return View(membershipTypes);
    }

    [HttpPost]
    public async Task<IActionResult> Create(string name, string membershipCode, decimal fee, string description)
    {
        try
        {
            await _createHandler.Handle(name, membershipCode, fee, description);
            TempData["SuccessMessage"] = OperationMessage.Membership.Created;
        }
        catch (ValidationException ex)
        {
            TempData["ErrorMessage"] = ex.Errors.FirstOrDefault()?.ErrorMessage ?? OperationMessage.Error.InvalidInput;
        }
        catch (Exception)
        {
            TempData["ErrorMessage"] = OperationMessage.Error.SaveFailed;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Update(int id, string name, string membershipCode, decimal fee, string description)
    {
        try
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing != null)
            {
                string currentDesc = existing.Description ?? string.Empty;
                string newDesc = description ?? string.Empty;

                if (existing.Name == name &&
                    existing.MembershipCode == membershipCode &&
                    existing.MembershipFee == fee &&
                    currentDesc == newDesc)
                {
                    TempData["SuccessMessage"] = OperationMessage.Membership.NoChanges;
                    return RedirectToAction(nameof(Index));
                }
            }

            await _updateHandler.Handle(id, name, membershipCode, fee, description ?? string.Empty);
            TempData["SuccessMessage"] = OperationMessage.Membership.Updated;
        }
        catch (ValidationException ex)
        {
            TempData["ErrorMessage"] = ex.Errors.FirstOrDefault()?.ErrorMessage ?? OperationMessage.Error.InvalidInput;
        }
        catch (KeyNotFoundException)
        {
            TempData["ErrorMessage"] = OperationMessage.Error.NotFound;
        }
        catch (Exception)
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

    [HttpPost]
    public async Task<IActionResult> ToggleStatus(int id)
    {
        try
        {
            var membership = await _repository.GetByIdAsync(id);
            if (membership == null)
            {
                TempData["ErrorMessage"] = OperationMessage.Error.NotFound;
                return RedirectToAction(nameof(Index));
            }

            if (membership.IsActive)
                membership.Deactivate();
            else
                membership.Activate();

            await _repository.UpdateAsync(membership);
            TempData["SuccessMessage"] = OperationMessage.Membership.Updated;
        }
        catch
        {
            TempData["ErrorMessage"] = OperationMessage.Error.SaveFailed;
        }

        return RedirectToAction(nameof(Index));
    }
}