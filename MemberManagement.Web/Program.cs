using FluentValidation;
using FluentValidation.AspNetCore;
using MemberManagement.Application.Core;
using MemberManagement.Application.Interfaces;
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

            // Add Dependency Injection
            builder.Services.AddScoped<IMemberRepository, MemberRepository>();
            builder.Services.AddScoped<IMemberService, MemberService>();
            builder.Services.AddScoped<MemberCore>();

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
