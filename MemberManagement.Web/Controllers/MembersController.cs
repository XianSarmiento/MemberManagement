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

    public async Task<IActionResult> Index(string searchLastName = "", int page = 1, int pageSize = 5)
    {
        var dtos = await _memberCore.GetActiveMembersAsync();

        // Filter by Last Name 
        if (!string.IsNullOrWhiteSpace(searchLastName))
        {
            dtos = dtos
                .Where(d => d.LastName.Contains(searchLastName, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        // Map DTOs → VM
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

        // Support "All" items
        int actualPageSize = pageSize;
        if (pageSize == -1) // Use -1 for "All"
            actualPageSize = vms.Count;

        var pagedList = vms.ToPagedList(page, actualPageSize);

        // Response metadata for API or frontend use
        ViewBag.Pagination = new
        {
            items = pagedList.ToList(),
            total = vms.Count,
            page = page,
            pageSize = actualPageSize
        };

        ViewBag.SearchLastName = searchLastName;
        ViewBag.CurrentPageSize = pageSize;

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
            return RedirectToAction(nameof(Index));
        }
        catch (ValidationException ex)
        {
            foreach (var error in ex.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
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
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(vm);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _memberCore.DeleteMemberAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
