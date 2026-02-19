using FluentAssertions;
using MemberManagement.Application.Validation;
using MemberManagement.Domain.Entities;
using MemberManagement.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace MemberManagement.Test.Validation;

public class MemberValidatorTests
{
    private readonly MMSDbContext _context;
    private readonly MemberValidator _validator;

    public MemberValidatorTests()
    {
        var options = new DbContextOptionsBuilder<MMSDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new MMSDbContext(options);
        _validator = new MemberValidator(_context);
    }

    [Fact]
    public async Task Should_Fail_When_FullName_And_BirthDate_Is_Duplicate()
    {
        // Arrange: Add an existing member
        var birthDate = new DateOnly(1990, 1, 1);
        var existing = new Member("John", "Doe", birthDate, 1, 1);
        _context.Members.Add(existing);
        await _context.SaveChangesAsync();

        // New member with same name and birthdate
        var duplicate = new Member("john", "DOE", birthDate, 1, 1);

        // Act
        var result = await _validator.ValidateAsync(duplicate);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == "A member with this name and birthdate already exists.");
    }

    [Theory]
    [InlineData("1234567890")]  // Too short
    [InlineData("08123456789")] // Doesn't start with 09
    [InlineData("0912345678A")] // Contains letters
    public async Task Should_Fail_When_PH_ContactNo_Is_Invalid(string invalidNumber)
    {
        // Arrange
        var member = new Member("Alice", "Smith", new DateOnly(1995, 1, 1), 1, 1, contactNo: invalidNumber);

        // Act
        var result = await _validator.ValidateAsync(member);

        // Assert
        result.Errors.Should().Contain(e => e.ErrorMessage == "Invalid PH number. Format: 09XXXXXXXXX");
    }

    [Fact]
    public async Task Should_Fail_When_Age_Is_Exactly_Under_18()
    {
        // 1. Create a VALID member first (so the constructor doesn't throw)
        var birthDate = DateOnly.FromDateTime(DateTime.Today).AddYears(-20);
        var member = new Member("Young", "User", birthDate, 1, 1);

        // 2. Force an INVALID birthdate (17 years old) into the property via Reflection
        var invalidBirthDate = DateOnly.FromDateTime(DateTime.Today).AddYears(-17);

        var property = typeof(Member).GetProperty(nameof(Member.BirthDate));
        property?.SetValue(member, invalidBirthDate);

        // 3. Now the Validator can run and find the error
        // Act
        var result = await _validator.ValidateAsync(member);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "BirthDate");
    }

    [Fact]
    public async Task Should_Fail_When_Age_Exceeds_65_And_Six_Months()
    {
        // Arrange: 
        // 1. Create a valid 30-year-old so the constructor doesn't throw InvalidOperationException
        var validBirthDate = DateOnly.FromDateTime(DateTime.Today).AddYears(-30);
        var member = new Member("Senior", "User", validBirthDate, 1, 1);

        // 2. Force the "too old" birthdate (66 years) via Reflection
        var tooOldBirthDate = DateOnly.FromDateTime(DateTime.Today).AddYears(-66);

        var property = typeof(Member).GetProperty(nameof(Member.BirthDate));
        property?.SetValue(member, tooOldBirthDate);

        // Act
        var result = await _validator.ValidateAsync(member);

        // Assert
        result.IsValid.Should().BeFalse();
        // This now tests that the Validator (Application layer) catches what the Constructor (Domain layer) also protects.
        result.Errors.Should().Contain(e => e.PropertyName == "BirthDate");
    }

    [Fact]
    public async Task Should_Pass_When_Updating_Same_Member_With_Same_Email()
    {
        // Arrange: Add a member with an email
        var member = new Member("John", "Doe", new DateOnly(1990, 1, 1), 1, 1, emailAddress: "john@example.com"); _context.Members.Add(member);
        await _context.SaveChangesAsync();

        // Act: Validate the SAME member instance (simulating an update)
        var result = await _validator.ValidateAsync(member);

        // Assert: Should NOT flag "Email exists" because it belongs to this MemberID
        result.IsValid.Should().BeTrue();
    }
}