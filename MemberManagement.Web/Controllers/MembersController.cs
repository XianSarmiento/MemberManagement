using FluentValidation;
using MemberManagement.Application.Business;
using MemberManagement.Application.Core;
using MemberManagement.Web.Mappers;
using MemberManagement.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using X.PagedList.Extensions;

/// <summary>
/// Controller responsible for managing Member entities in the application.
/// It provides CRUD operations (Create, Read, Update, Delete) and supports
/// paging, sorting, filtering, and validation using FluentValidation.
/// </summary>
public class MembersController(MemberCore memberCore, IValidator<MemberVM> vmValidator) : Controller
{
    private readonly MemberCore _memberCore = memberCore;
    private readonly IValidator<MemberVM> _vmValidator = vmValidator;

    public async Task<IActionResult> Index(
        string searchLastName = "", string branch = "", 
        string sortColumn = "MemberId", string sortOrder = "asc",
        int page = 1, int pageSize = 10)
    {
        var result = await _memberCore.GetMembersForIndexAsync(searchLastName, branch, sortColumn, sortOrder);

        var memberVMs = result.Members.ToViewModels();

        ViewBag.rawPageSize = pageSize;
        int actualPageSize = pageSize < 1 ? memberVMs.Count : pageSize;
        var pagedList = memberVMs.ToPagedList(page, actualPageSize);

        ViewBag.branches = result.Branches ?? new List<string>();
        ViewBag.searchLastName = searchLastName;
        ViewBag.selectedBranch = branch;
        ViewBag.currentPageSize = pageSize;

        ViewBag.sortColumn = sortColumn;
        ViewBag.sortOrder = sortOrder;

        return View(new MemberIndexVM
        {
            Members = pagedList,
            SearchLastName = searchLastName,
            SelectedBranch = branch,
            PageSize = pageSize,
            SortColumn = sortColumn,
            SortOrder = sortOrder,
            Branches = result.Branches ?? new List<string>()
        });
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MemberVM memberVM)
    {
        var validationResult = await _vmValidator.ValidateAsync(memberVM); 
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            return View(memberVM);
        }

        var dto = memberVM.ToDTO();

        try
        {
            await _memberCore.CreateMemberAsync(dto);
            TempData["SuccessMessage"] = OperationMessage.User.Created;
            Console.WriteLine(OperationMessage.System.Created);
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            ModelState.AddModelError(string.Empty, OperationMessage.Error.SaveFailed);
            Console.WriteLine(OperationMessage.Error.SaveFailed);
            return View(memberVM);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var dto = await _memberCore.GetMemberByIdAsync(id);
        if (dto == null) return NotFound();

        var memberVM = dto.ToViewModel();

        return View(memberVM);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(MemberVM memberVM)
    {
        var validationResult = await _vmValidator.ValidateAsync(memberVM);                        // Validate VM first
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            return View(memberVM);                                                                // Use mapper from DTO
        }

        var dto = memberVM.ToDTO();

        try
        {
            await _memberCore.UpdateMemberAsync(dto);

            TempData["SuccessMessage"] = OperationMessage.User.Updated;
            Console.WriteLine(OperationMessage.System.Updated);

            return RedirectToAction(nameof(Index));
        }
        catch
        {
            ModelState.AddModelError(string.Empty, OperationMessage.Error.SaveFailed);
            Console.WriteLine(OperationMessage.Error.SaveFailed);
            return View(memberVM);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Detail(int id)
    {
        var dto = await _memberCore.GetMemberByIdAsync(id);
        if (dto == null) return NotFound();

        var memberVM = dto.ToViewModel();

        return View(memberVM);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _memberCore.DeleteMemberAsync(id);

            TempData["SuccessMessage"] = OperationMessage.User.Deleted;
            Console.WriteLine(OperationMessage.System.Deleted);
        }
        catch
        {
            TempData["ErrorMessage"] = OperationMessage.Error.SaveFailed;
            Console.WriteLine(OperationMessage.Error.SaveFailed);
        }

        return RedirectToAction(nameof(Index));
    }
}
