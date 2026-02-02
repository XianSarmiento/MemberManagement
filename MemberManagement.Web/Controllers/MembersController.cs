using FluentValidation;
using MemberManagement.Application.Business;
using MemberManagement.Application.Core;
using MemberManagement.Web.Mappers;
using MemberManagement.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;

public class MembersController : Controller
{
    private readonly MemberCore _memberCore;
    private readonly IValidator<MemberVM> _vmValidator;

    public MembersController(MemberCore memberCore, IValidator<MemberVM> vmValidator)
    {
        _memberCore = memberCore;
        _vmValidator = vmValidator;
    }

    public async Task<IActionResult> Index(string searchLastName = "", string branch = "", int page = 1, int pageSize = 5)
    {
        var result = await _memberCore.GetMembersForIndexAsync(searchLastName, branch);     //

        var vms = result.Members.ToViewModels();                                            // Use mapper instead of repeating mapping logic

        ViewBag.RawPageSize = pageSize;                                                     // Save the raw selection for the UI
        int actualPageSize = pageSize < 1 ? vms.Count : pageSize;                           // If pageSize < 1, show all items
        var pagedList = vms.ToPagedList(page, actualPageSize);                              // Build paged list correctly

        ViewBag.Branches = result.Branches ?? new List<string>();                           // ViewBag for filters
        ViewBag.SearchLastName = searchLastName;
        ViewBag.SelectedBranch = branch;
        ViewBag.CurrentPageSize = pageSize;

        return View(pagedList);
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MemberVM vm)
    {
        var validationResult = await _vmValidator.ValidateAsync(vm);                        // Validate VM first
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            return View(vm);
        }

        var dto = vm.ToDTO();                                                               // Use mapper from DTO

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
            return View(vm);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var dto = await _memberCore.GetMemberByIdAsync(id);
        if (dto == null) return NotFound();

        var vm = dto.ToViewModel();

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(MemberVM vm)
    {
        var validationResult = await _vmValidator.ValidateAsync(vm);                        // Validate VM first
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            return View(vm);                                                                // Use mapper from DTO
        }

        var dto = vm.ToDTO();

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
            return View(vm);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Detail(int id)
    {
        var dto = await _memberCore.GetMemberByIdAsync(id);
        if (dto == null) return NotFound();

        var vm = dto.ToViewModel();

        return View(vm);
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
