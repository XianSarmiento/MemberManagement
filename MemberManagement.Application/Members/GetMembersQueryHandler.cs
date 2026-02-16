using MemberManagement.Application.DTOs;
using MemberManagement.Application.Interfaces;
using MemberManagement.Application.Mappers;
using MemberManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemberManagement.Application.Members;

public class GetMembersQueryHandler
{
    private readonly IMemberService _memberService;

    public GetMembersQueryHandler(IMemberService memberService)
    {
        _memberService = memberService;
    }

    public async Task<MemberIndexResult> HandleAsync(string searchLastName, string branch, string sortColumn, string sortOrder, bool getActive = true)
    {
        var memberEntities = getActive
            ? await _memberService.GetActiveMembersAsync()
            : await _memberService.GetInactiveMembersAsync();

        var members = memberEntities.Select(m => m.ToDto()).ToList();

        if (!string.IsNullOrWhiteSpace(searchLastName))
        {
            members = members.Where(m => m.LastName?.Contains(searchLastName, StringComparison.OrdinalIgnoreCase) == true).ToList();
        }

        if (!string.IsNullOrWhiteSpace(branch))
        {
            members = members.Where(m => m.Branch == branch).ToList();
        }

        members = sortColumn switch
        {
            "FirstName" => sortOrder == "desc"
                ? members.OrderByDescending(m => m.FirstName).ToList()
                : members.OrderBy(m => m.FirstName).ToList(),
            "LastName" => sortOrder == "desc"
                ? members.OrderByDescending(m => m.LastName).ToList()
                : members.OrderBy(m => m.LastName).ToList(),
            _ => members.OrderBy(m => m.MemberID).ToList()
        };

        return new MemberIndexResult
        {
            Members = members,
            TotalItems = members.Count,
            Branches = members
                .Select(m => m.Branch!)
                .Where(b => !string.IsNullOrWhiteSpace(b))
                .Distinct()
                .OrderBy(b => b)
                .ToList()
        };
    }
}