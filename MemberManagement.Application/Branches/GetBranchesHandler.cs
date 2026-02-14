using MemberManagement.Application.DTOs;
using MemberManagement.Domain.Interfaces;

public class GetBranchesHandler
{
    private readonly IBranchRepository _repository;

    public GetBranchesHandler(IBranchRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<BranchDto>> Handle()
    {
        var branches = await _repository.GetAllAsync();
        return branches.Select(b => new BranchDto
        {
            Id = b.Id,
            Name = b.Name,
            IsActive = b.IsActive
        });
    }
}