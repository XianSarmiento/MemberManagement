using FluentValidation;
using MemberManagement.Application.Business;
using MemberManagement.Application.Core;
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
        var dtos = await _memberCore.GetActiveMembersAsync();

        // FILTER: Last Name
        if (!string.IsNullOrWhiteSpace(searchLastName))
        {
            dtos = dtos.Where(d => d.LastName.Contains(searchLastName, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        // FILTER: Branch
        if (!string.IsNullOrWhiteSpace(branch))
        {
            dtos = dtos.Where(d => d.Branch.Equals(branch, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        // MAP DTO → VM
        var vms = dtos.Select(d => new MemberVM
        {
            MemberID = d.MemberID,
            FirstName = d.FirstName,
            LastName = d.LastName,
            BirthDate = d.BirthDate,
            Address = d.Address,
            Branch = d.Branch,
            ContactNo = d.ContactNo,
            EmailAddress = d.EmailAddress
        }).ToList();

        // PAGE SIZE SUPPORT
        int actualPageSize = pageSize;
        if (pageSize == -1)
          actualPageSize = vms.Count == 0 ? 1 : vms.Count;

        var pagedList = vms.ToPagedList(page, actualPageSize);

        // PAGINATION METADATA
        ViewBag.Pagination = new
        {
            items = pagedList.ToList(),
            total = vms.Count,
            page = page,
            pageSize = actualPageSize
        };

        // VIEW DATA (for Index.cshtml)
        ViewBag.SearchLastName = searchLastName;
        ViewBag.Branch = branch;
        ViewBag.CurrentPageSize = pageSize;

        // BRANCH LIST FOR DATALIST
        ViewBag.Branches = dtos
            .Select(d => d.Branch)
            .Where(b => !string.IsNullOrWhiteSpace(b))
            .Distinct()
            .OrderBy(b => b)
            .ToList();

        return View(pagedList);
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MemberVM vm)
    {
        // Validate VM first
        var validationResult = await _vmValidator.ValidateAsync(vm);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            return View(vm);
        }

        var dto = new MemberDTO
        {
            FirstName = vm.FirstName,
            LastName = vm.LastName,
            BirthDate = vm.BirthDate,
            Address = vm.Address,
            Branch = vm.Branch,
            ContactNo = vm.ContactNo,
            EmailAddress = vm.EmailAddress
        };

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

        var vm = new MemberVM
        {
            MemberID = dto.MemberID,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            BirthDate = dto.BirthDate,
            Address = dto.Address,
            Branch = dto.Branch,
            ContactNo = dto.ContactNo,
            EmailAddress = dto.EmailAddress
        };

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(MemberVM vm)
    {
        // Validate VM first
        var validationResult = await _vmValidator.ValidateAsync(vm);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            return View(vm);
        }

        var dto = new MemberDTO
        {
            MemberID = vm.MemberID,
            FirstName = vm.FirstName,
            LastName = vm.LastName,
            BirthDate = vm.BirthDate,
            Address = vm.Address,
            Branch = vm.Branch,
            ContactNo = vm.ContactNo,
            EmailAddress = vm.EmailAddress
        };
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

        var vm = new MemberVM
        {
            MemberID = dto.MemberID,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            BirthDate = dto.BirthDate,
            Address = dto.Address,
            Branch = dto.Branch,
            ContactNo = dto.ContactNo,
            EmailAddress = dto.EmailAddress
        };

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
