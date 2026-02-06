using FluentValidation;
using MemberManagement.Application.Core;
using MemberManagement.Application.DTOs;
using MemberManagement.Application.Interfaces;
using MemberManagement.Application.Members;
using MemberManagement.Application.Mappers;
using MemberManagement.Web.Mappers;
using MemberManagement.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using X.PagedList.Extensions;

namespace MemberManagement.Web.Controllers
{
    public class MembersController : Controller
    {
        private readonly GetMembersQueryHandler _getQueryHandler;
        private readonly CreateMemberHandler _createHandler;
        private readonly UpdateMemberHandler _updateHandler;
        private readonly IMemberService _memberService;
        private readonly IValidator<MemberVM> _vmValidator;
        private readonly IMemberExportService _exportService;

        public MembersController(
            GetMembersQueryHandler getQueryHandler,
            CreateMemberHandler createHandler,
            UpdateMemberHandler updateHandler,
            IMemberService memberService,
            IValidator<MemberVM> vmValidator,
            IMemberExportService exportService)
        {
            _getQueryHandler = getQueryHandler;
            _createHandler = createHandler;
            _updateHandler = updateHandler;
            _memberService = memberService;
            _vmValidator = vmValidator;
            _exportService = exportService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            string searchLastName = "", string branch = "",
            string sortColumn = "MemberId", string sortOrder = "asc",
            int page = 1, int pageSize = 10)
        {
            var result = await _getQueryHandler.HandleAsync(searchLastName, branch, sortColumn, sortOrder);

            // Fixed: Explicitly use MemberEntityMapper to avoid ambiguity
            var memberVMs = result.Members.ToViewModels();

            int actualPageSize = pageSize < 1 ? (memberVMs.Count > 0 ? memberVMs.Count : 10) : pageSize;
            var pagedList = memberVMs.ToPagedList(page, actualPageSize);

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

            try
            {
                await _createHandler.HandleAsync(memberVM.ToDTO());
                TempData["SuccessMessage"] = OperationMessage.User.Created;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, OperationMessage.Error.SaveFailed);
                return View(memberVM);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var member = await _memberService.GetByIdAsync(id);
            if (member == null) return NotFound();

            var dto = MemberEntityMapper.ToDto(member);
            var memberVM = dto.ToViewModel();

            return View(memberVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MemberVM memberVM)
        {
            var validationResult = await _vmValidator.ValidateAsync(memberVM);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

                return View(memberVM);
            }

            try
            {
                await _updateHandler.HandleAsync(memberVM.ToDTO());
                TempData["SuccessMessage"] = OperationMessage.User.Updated;
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError(string.Empty, OperationMessage.Error.SaveFailed);
                return View(memberVM);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var member = await _memberService.GetByIdAsync(id);
            if (member == null) return NotFound();

            var dto = MemberEntityMapper.ToDto(member);
            var memberVM = dto.ToViewModel();

            return View(memberVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _memberService.DeleteAsync(id);
                TempData["SuccessMessage"] = OperationMessage.User.Deleted;
            }
            catch
            {
                TempData["ErrorMessage"] = OperationMessage.Error.SaveFailed;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> ExportToExcel(string searchLastName = "", string branch = "")
        {
            var result = await _getQueryHandler.HandleAsync(searchLastName, branch, "MemberId", "asc");
            var fileContent = _exportService.GenerateExcel(result.Members);
            var fileName = $"Members_{DateTime.Now:yyyyMMdd}.xlsx";

            return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpGet]
        public async Task<IActionResult> ExportToPdf(string searchLastName = "", string branch = "")
        {
            var result = await _getQueryHandler.HandleAsync(searchLastName, branch, "MemberId", "asc");
            var fileContent = _exportService.GeneratePdf(result.Members);
            var fileName = $"Members_{DateTime.Now:yyyyMMdd}.pdf";

            return File(fileContent, "application/pdf", fileName);
        }
    }
}

/* HOW IT WORKS:
  This controller manages the communication between the web browser and the application layer. 
  It handles HTTP requests (GET, POST), manages UI state, and coordinates file downloads.

  1. DEPENDENCY INJECTION: The constructor receives Handlers (for writing/querying) 
     and Services (for simple tasks and exports). This keeps the controller "Thin"—it 
     doesn't contain business logic, it just delegates work.

  2. INDEX & PAGINATION (GET):
     - It calls the 'GetMembersQueryHandler' to fetch filtered/sorted data.
     - It uses 'X.PagedList' to split a large list of members into smaller, 
       manageable pages (e.g., 10 per page).
     - It prepares a 'MemberIndexVM' containing the data and the current filter 
       settings so the search bar stays populated after the page refreshes.

  3. FORM HANDLING (POST):
     - 'ValidateAntiForgeryToken': Protects against CSRF security attacks.
     - 'ModelState': If FluentValidation finds an error, the controller adds it to 
       ModelState. This allows the View to display red error messages next to 
       the specific input fields.
     - 'TempData': Stores "Success" messages that survive a redirect, so the user 
       sees "Member successfully created" after being sent back to the Index page.

  4. MAPPING LAYERS:
     - Notice the transition: ViewModel (UI) <-> DTO (Application) <-> Entity (Domain).
     - By converting 'memberVM.ToDTO()', the controller ensures the Handler 
       remains pure and doesn't know anything about Web ViewModels.

  5. FILE EXPORTS (ActionResults):
     - 'ExportToExcel' and 'ExportToPdf' call the export service to get a raw 
       byte array.
     - The 'File()' method then tells the browser: "Don't display this as text; 
       download it as a file with this specific name and type."
*/