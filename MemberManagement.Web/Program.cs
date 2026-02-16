using FluentValidation;
using FluentValidation.AspNetCore;
using MemberManagement.Application.Branches;
using MemberManagement.Application.Interfaces;
using MemberManagement.Application.Members;
using MemberManagement.Application.MembershipTypes;
using MemberManagement.Application.Services;
using MemberManagement.Application.Validation;
using MemberManagement.Domain.Entities;
using MemberManagement.Domain.Interfaces;
using MemberManagement.Infrastructure;
using MemberManagement.Infrastructure.Repositories;
using MemberManagement.Web.ValidationsVM;
using MemberManagement.Web.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace MemberManagement.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            // --- VALIDATORS ---
            builder.Services.AddValidatorsFromAssemblyContaining<MemberVMValidator>();
            builder.Services.AddScoped<IValidator<Member>, MemberValidator>();
            builder.Services.AddScoped<IValidator<MemberVM>, MemberVMValidator>();
            builder.Services.AddFluentValidationClientsideAdapters();

            // --- DATABASE ---
            builder.Services.AddDbContext<MMSDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            );

            // --- REPOSITORIES ---
            builder.Services.AddScoped<IMemberRepository, MemberRepository>();
            builder.Services.AddScoped<IBranchRepository, BranchRepository>();
            builder.Services.AddScoped<IMembershipTypeRepository, MembershipTypeRepository>();

            // --- SERVICES ---
            builder.Services.AddScoped<IMemberService, MemberService>();
            builder.Services.AddScoped<IMemberExportService, MemberExportService>();
            builder.Services.AddScoped<IBranchService, BranchService>();
            builder.Services.AddScoped<IMembershipTypeService, MembershipTypeService>();

            // --- HANDLERS ---
            builder.Services.AddScoped<CreateMemberHandler>();
            builder.Services.AddScoped<UpdateMemberHandler>();
            builder.Services.AddScoped<GetMembersQueryHandler>();
            builder.Services.AddScoped<GetBranchesHandler>();
            builder.Services.AddScoped<CreateBranchHandler>();
            builder.Services.AddScoped<GetMembershipTypesHandler>();
            builder.Services.AddScoped<CreateMembershipTypeHandler>();

            var app = builder.Build();

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