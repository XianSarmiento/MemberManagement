using FluentAssertions;
using MemberManagement.Application.Validation;
using MemberManagement.Domain.Entities;
using MemberManagement.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace MemberManagement.Tests.Application.Validation
{
    public class BranchValidatorTests
    {
        private readonly MMSDbContext _context;
        private readonly BranchValidator _validator;

        public BranchValidatorTests()
        {
            var options = new DbContextOptionsBuilder<MMSDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new MMSDbContext(options);
            _validator = new BranchValidator(_context);
        }

        private Branch CreateBranch(string name, string address, string code)
        {
            return new Branch(name, address, code);
        }

        [Fact]
        public async Task BranchValidator_DuplicateBranch_ShouldFail()
        {
            var existing = CreateBranch("Bato Branch", "Bato, Catanduanes", "B001");
            _context.Branches.Add(existing);
            await _context.SaveChangesAsync();

            var duplicate = CreateBranch("Bato Branch", "Bato, Catanduanes", "B001");
            var result = await _validator.ValidateAsync(duplicate);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.ErrorMessage.Contains("already exists"));
        }

        [Fact]
        public async Task BranchValidator_ValidBranch_ShouldPass()
        {
            var branch = CreateBranch("Legazpi Branch", "Legazpi City, Albay", "L001");
            var result = await _validator.ValidateAsync(branch);
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public async Task BranchValidator_BranchNameTooLong_ShouldFail()
        {
            var longName = new string('A', 101);
            var branch = CreateBranch(longName, "Irosin, Sorsogon", "I001");
            var result = await _validator.ValidateAsync(branch);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Name");
        }

        [Fact]
        public async Task BranchValidator_BranchCodeTooLong_ShouldFail()
        {
            var longCode = new string('C', 21);
            var branch = CreateBranch("Daet Branch", "Daet, Camarines Norte", longCode);
            var result = await _validator.ValidateAsync(branch);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "BranchCode");
        }

        [Fact]
        public async Task BranchValidator_AllBranches_ShouldPass()
        {
            var branches = new[]
            {
                CreateBranch("ARDCI Bank Branch", "Main Office, Catanduanes", "ARDCI"),
                CreateBranch("Bato Branch", "Bato, Catanduanes", "BATO"),
                CreateBranch("Legazpi Branch", "Legazpi City, Albay", "LEG"),
                CreateBranch("Irosin Branch", "Irosin, Sorsogon", "IRO"),
                CreateBranch("Daet Branch", "Daet, Camarines Norte", "DAE")
            };

            foreach (var branch in branches)
            {
                var result = await _validator.ValidateAsync(branch);
                result.IsValid.Should().BeTrue();
            }
        }
    }
}