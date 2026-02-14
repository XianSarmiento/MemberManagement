using FluentValidation;
using FluentValidation.AspNetCore;
using MemberManagement.Application.Core;
using MemberManagement.Application.Interfaces;
using MemberManagement.Application.Members;
using MemberManagement.Application.Services;
using MemberManagement.Application.Validation;
using MemberManagement.Domain.Entities;
using MemberManagement.Domain.Interfaces;
using MemberManagement.Infrastructure;
using MemberManagement.Infrastructure.Repositories;
using MemberManagement.Web.ValidationsVM;
using Microsoft.EntityFrameworkCore;

namespace MemberManagement.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container or MVC
            builder.Services.AddControllersWithViews();

            // Register validators
            builder.Services.AddValidatorsFromAssemblyContaining<MemberVMValidator>();
            builder.Services.AddScoped<IValidator<Member>, MemberValidator>();

            // Enable FluentValidation integration
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddFluentValidationClientsideAdapters();

            // Add DbContext
            builder.Services.AddDbContext<MMSDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            );

            // Add DI for Repositories
            builder.Services.AddScoped<IMemberRepository, MemberRepository>();
            builder.Services.AddScoped<IBranchRepository, BranchRepository>();
            builder.Services.AddScoped<IMembershipTypeRepository, MembershipTypeRepository>();

            // Add DI for Services
            builder.Services.AddScoped<IMemberService, MemberService>();
            builder.Services.AddScoped<IMemberExportService, MemberExportService>();

            // Handlers for Commands and Queries
            builder.Services.AddScoped<CreateMemberHandler>();
            builder.Services.AddScoped<UpdateMemberHandler>();
            builder.Services.AddScoped<GetMembersQueryHandler>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}

/* HOW THIS CODE WORKS:
   1. DEPENDENCY INJECTION (DI) CONTAINER: 
      Everything registered under 'builder.Services' is added to the DI container. 
      - 'AddScoped' means a new instance of the class is created for every single 
        HTTP request (the most common lifetime for web apps).
      - This allows your Controllers to "ask" for 'IMemberService' without 
        manually creating it with the 'new' keyword.

   2. FLUENT VALIDATION: 
      - 'AddValidatorsFromAssemblyContaining' scans your project for validation rules.
      - 'AddFluentValidationAutoValidation' makes it so that when a user submits a form, 
        ASP.NET automatically checks those rules before your Controller code even runs.

   3. DATABASE CONFIGURATION: 
      'AddDbContext' connects the app to SQL Server using the "DefaultConnection" string 
      found in your 'appsettings.json' file.

   4. THE MIDDLEWARE PIPELINE (The 'app' section): 
      This is the order in which a request is handled. 
      - 'UseStaticFiles' allows the app to serve CSS and JS.
      - 'UseRouting' and 'MapControllerRoute' determine which Controller and 
        Action method should handle the incoming URL.

   5. ARCHITECTURE PATTERN: 
      By registering 'Handlers' (CreateMemberHandler, etc.), this file shows that the 
      app follows a "Command/Query" or "Clean Architecture" style, separating 
      the "How to do it" logic from the Controller.
*/
