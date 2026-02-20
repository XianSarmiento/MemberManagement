using FluentAssertions;
using FluentValidation;
using FluentValidation.Results; 
using MemberManagement.Application.Branches;
using MemberManagement.Domain.Entities;
using MemberManagement.Domain.Interfaces;
using Moq;
using Xunit;

namespace MemberManagement.Tests.Application.Branches;

public class UpdateBranchHandlerTests
{
    private readonly Mock<IBranchRepository> _repositoryMock;
    private readonly Mock<IValidator<Branch>> _validatorMock;
    private readonly UpdateBranchHandler _handler;

    public UpdateBranchHandlerTests()
    {
        _repositoryMock = new Mock<IBranchRepository>();
        _validatorMock = new Mock<IValidator<Branch>>();

        _validatorMock
            .Setup(v => v.ValidateAsync(It.IsAny<Branch>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _handler = new UpdateBranchHandler(_repositoryMock.Object, _validatorMock.Object);
    }

    [Fact]
    public async Task Handle_WhenBranchExists_ShouldUpdateBranch()
    {
        // Arrange
        var branchId = 1;
        var existingBranch = new Branch("Old Name", "Old Address", "OLD");
        _repositoryMock.Setup(r => r.GetByIdAsync(branchId)).ReturnsAsync(existingBranch);

        // Act
        await _handler.Handle(branchId, "New Name", "New Address", "NEW");

        // Assert
        _repositoryMock.Verify(r => r.UpdateAsync(existingBranch), Times.Once);
        existingBranch.Name.Should().Be("New Name");
    }

    [Fact]
    public async Task Handle_WhenBranchDoesNotExist_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Branch)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(99, "Name", "Addr", "Code");

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
                 .WithMessage("Branch not found.");
    }
}