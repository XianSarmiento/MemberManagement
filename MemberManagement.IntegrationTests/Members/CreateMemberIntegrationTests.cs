using FluentAssertions;
using MemberManagement.IntegrationTests.Base;
using MemberManagement.Web.ViewModels;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic; // Added for Dictionary
using System.Net;
using System.Net.Http; // Added for FormUrlEncodedContent
using System.Threading.Tasks;
using Xunit;

namespace MemberManagement.IntegrationTests.Members;

public class CreateMemberIntegrationTests : IClassFixture<MemberApiFactory>
{
    private readonly HttpClient _client;

    public CreateMemberIntegrationTests(MemberApiFactory factory)
    {
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Fact]
    public async Task CreateMember_WithValidData_ShouldRedirectToIndex()
    {
        // 1. Arrange: Prepare the data as a Dictionary of strings
        var formData = new Dictionary<string, string>
        {
            { "FirstName", "Integration" },
            { "LastName", "User" },
            { "BirthDate", "1990-01-01" },
            { "BranchId", "1" },
            { "MembershipTypeId", "1" },
            { "EmailAddress", "integration@test.com" },
            { "Address", "Test Address" },
            { "ContactNo", "09123456789" }
        };

        var content = new FormUrlEncodedContent(formData);

        // 2. Act: Use PostAsync with FormUrlEncodedContent
        var response = await _client.PostAsync("/Members/Create", content);

        // 3. Assert
        // If this still fails with 200, it means a Validation Error occurred.
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var result = await response.Content.ReadAsStringAsync();
            // DEBUG TIP: Set a breakpoint here and look at 'result' 
            // to see the validation errors in the HTML.
        }

        response.StatusCode.Should().Be(HttpStatusCode.Redirect);
        response.Headers.Location?.ToString().Should().Contain("/Members");
    }
}