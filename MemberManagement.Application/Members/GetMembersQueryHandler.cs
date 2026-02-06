using MemberManagement.Application.DTOs;
using MemberManagement.Application.Interfaces;
using MemberManagement.Application.Mappers;
using MemberManagement.Domain.Entities;

namespace MemberManagement.Application.Members;

public class GetMembersQueryHandler
{
    private readonly IMemberService _memberService;

    public GetMembersQueryHandler(IMemberService memberService)
    {
        _memberService = memberService;
    }

    public async Task<MemberIndexResult> HandleAsync(string searchLastName, string branch, string sortColumn, string sortOrder)
    {
        var memberEntities = await _memberService.GetActiveMembersAsync();

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

/* HOW IT WORKS:
  This handler retrieves a list of members and applies filtering, sorting, and metadata 
  extraction to prepare the data for a "List" or "Index" view.

  1. DATA RETRIEVAL & MAPPING: It fetches all active members from the database service 
     and immediately converts (maps) them from Domain Entities to DTOs. This ensures 
     the UI only sees data it is allowed to see.

  2. IN-MEMORY FILTERING: 
     - Search: If a last name is provided, it filters the list (case-insensitive).
     - Branch: If a specific branch is selected, it narrows the list further.

  3. DYNAMIC SORTING: It uses a C# 'switch' expression to decide which column to sort 
     by (FirstName, LastName, or Default/MemberID) and in which direction (Ascending 
     or Descending) based on the input parameters.

  4. METADATA AGGREGATION: Finally, it returns a 'MemberIndexResult' which includes:
     - The filtered/sorted list of members.
     - A 'TotalItems' count for UI pagination/stats.
     - A 'Branches' list: It scans the current member list to find all unique branch 
       names. This is often used to populate a "Filter by Branch" dropdown in the UI.
*/