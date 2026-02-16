using FluentValidation;
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
using MemberManagement.SharedKernel.Constant;

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
        private readonly IBranchService _branchService;
        private readonly IMembershipTypeService _typeService;

        public MembersController(
            GetMembersQueryHandler getQueryHandler,
            CreateMemberHandler createHandler,
            UpdateMemberHandler updateHandler,
            IMemberService memberService,
            IValidator<MemberVM> vmValidator,
            IMemberExportService exportService,
            IBranchService branchService,
            IMembershipTypeService typeService)
        {
            _getQueryHandler = getQueryHandler;
            _createHandler = createHandler;
            _updateHandler = updateHandler;
            _memberService = memberService;
            _vmValidator = vmValidator;
            _exportService = exportService;
            _branchService = branchService;
            _typeService = typeService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchLastName = "", string branch = "", string sortColumn = "MemberId", string sortOrder = "asc", int page = 1, int pageSize = 10)
        {
            var result = await _getQueryHandler.HandleAsync(searchLastName, branch, sortColumn, sortOrder);

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
        public async Task<IActionResult> Create()
        {
            await PopulateSelectionLists();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MemberVM memberVM)
        {
            var validationResult = await _vmValidator.ValidateAsync(memberVM);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

                await PopulateSelectionLists();
                return View(memberVM);
            }

            try
            {
                await _createHandler.HandleAsync(memberVM.ToDTO());
                TempData["SuccessMessage"] = OperationMessage.User.Created;
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("BirthDate", ex.Message);
                await PopulateSelectionLists();
                return View(memberVM);
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, OperationMessage.Error.SaveFailed);
                await PopulateSelectionLists();
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

            await PopulateSelectionLists();
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

                await PopulateSelectionLists();
                return View(memberVM);
            }

            try
            {
                await _updateHandler.HandleAsync(memberVM.ToDTO());
                TempData["SuccessMessage"] = OperationMessage.User.Updated;
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("BirthDate", ex.Message);
                await PopulateSelectionLists();
                return View(memberVM);
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, OperationMessage.Error.SaveFailed);
                await PopulateSelectionLists();
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

        [HttpGet]
        public async Task<IActionResult> Inactive(string searchLastName = "", string branch = "", string sortColumn = "MemberId", string sortOrder = "asc", int page = 1, int pageSize = 10)
        {
            var result = await _getQueryHandler.HandleAsync(searchLastName, branch, sortColumn, sortOrder, false);

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(int id)
        {
            try
            {
                await _memberService.RestoreAsync(id);
                TempData["SuccessMessage"] = OperationMessage.User.Restored;
            }
            catch
            {
                TempData["ErrorMessage"] = OperationMessage.Error.RestoreFailed;
            }
            return RedirectToAction(nameof(Inactive));
        }

        private async Task PopulateSelectionLists()
        {
            ViewBag.Branches = await _branchService.GetAllAsync();
            ViewBag.MembershipTypes = await _typeService.GetAllAsync();
        }
    }
}