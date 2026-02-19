using FluentAssertions;
using FluentValidation;
using FluentValidation.Results; 
using MemberManagement.Application.Branches;
using MemberManagement.Domain.Entities;
using MemberManagement.Domain.Interfaces;
using Moq;
using Xunit;

namespace MemberManagement.Tests.Application.Branches;

public class CreateBranchHandlerTests
{
    private readonly Mock<IBranchRepository> _repositoryMock;
    private readonly Mock<IValidator<Branch>> _validatorMock;
    private readonly CreateBranchHandler _handler;

    public CreateBranchHandlerTests()
    {
        _repositoryMock = new Mock<IBranchRepository>();
        _validatorMock = new Mock<IValidator<Branch>>();

        // 2. Setup the mock to return a successful ValidationResult
        _validatorMock
            .Setup(v => v.ValidateAsync(It.IsAny<Branch>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _handler = new CreateBranchHandler(_repositoryMock.Object, _validatorMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldAddBranchAndReturnId()
    {
        // Arrange
        var name = "ARDCI Head Office";
        var address = "Surtida Street";
        var code = "MB0001";

        // Act
        var result = await _handler.Handle(name, address, code);

        // Assert
        _repositoryMock.Verify(r => r.AddAsync(It.Is<Branch>(b =>
            b.Name == name &&
            b.Address == address &&
            b.BranchCode == code)), Times.Once);
    }
}