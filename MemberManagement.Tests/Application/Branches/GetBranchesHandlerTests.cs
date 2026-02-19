using FluentAssertions;
using MemberManagement.Application.Branches;
using MemberManagement.Application.DTOs;
using MemberManagement.Domain.Entities;
using MemberManagement.Domain.Interfaces;
using Moq;
using Xunit;

namespace MemberManagement.Tests.Application.Branches;

public class GetBranchesHandlerTests
{
    private readonly Mock<IBranchRepository> _repositoryMock;
    private readonly GetBranchesHandler _handler;

    public GetBranchesHandlerTests()
    {
        _repositoryMock = new Mock<IBranchRepository>();
        _handler = new GetBranchesHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnListOfBranchDtos()
    {
        // Arrange
        var branches = new List<Branch>
        {
            new Branch("Branch A", "Addr A", "A01"),
            new Branch("Branch B", "Addr B", "B01")
        };
        _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(branches);

        // Act
        var result = await _handler.Handle();

        // Assert
        result.Should().HaveCount(2);
        result.First().Name.Should().Be("Branch A");
        _repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
    }
}