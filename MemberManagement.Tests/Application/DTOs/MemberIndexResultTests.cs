using FluentAssertions;
using MemberManagement.Application.DTOs;
using Xunit;

namespace MemberManagement.Tests.Application.DTOs;

public class MemberIndexResultTests
{
    [Fact]
    public void MemberIndexResult_ShouldInitializeListsByDefault()
    {
        // Act
        var result = new MemberIndexResult();

        // Assert
        result.Members.Should().NotBeNull();
        result.Branches.Should().NotBeNull();
        result.Members.Should().BeEmpty();
    }
}