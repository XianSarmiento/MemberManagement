# Member Management System

A robust member management solution developed using **Clean Architecture** principles to ensure maintainable, scalable, and well-structured code.

---

## 🏗 Project Architecture

The solution is divided into four specialized layers to maintain a clear separation of concerns:

* **MemberManagement.Web**: The presentation layer (ASP.NET Core MVC). It handles UI components, including **Controllers** (`MembersController.cs`), **ViewModels** (`MemberVM.cs`), **Mappers** (`MemberMapper.cs`), and frontend assets.
* **MemberManagement.Application**: Contains the core business logic. This includes **Services** (`MemberService.cs`), **DTOs** (`MemberDTO.cs`), and **Validation** logic using FluentValidation.
* **MemberManagement.Infrastructure**: Handles data persistence and external concerns. It contains the **MMSDbContext.cs**, EF Core **Migrations**, and **Repository** implementations (`MemberRepository.cs`).
* **MemberManagement.Domain**: The core layer containing enterprise-level entities like `Member.cs` and core repository interfaces.

---

## 🛠 Tech Stack

* **Framework**: ASP.NET Core 8.0
* **ORM**: Entity Framework Core 8.0.0
* **Database**: Microsoft SQL Server
* **Validation**: FluentValidation (12.1.1)
* **UI Helpers**: X.PagedList.Mvc.Core (10.5.9)

---

## 📋 Requirements

* **.NET SDK 8.0+**
* **SQL Server** or **LocalDB**
* **Visual Studio 2022** (recommended)

---

## 🚀 Getting Started

### 1. Database Configuration

Update the `"ConnectionStrings:DefaultConnection"` key in `MemberManagement.Web/appsettings.json` to point to your SQL Server instance.


### 2. Apply Migrations

Open your terminal in the solution root and execute the following command to build the schema:

```bash
dotnet ef database update --project MemberManagement.Infrastructure --startup-project MemberManagement.Web

```

*This will automatically create the database and apply migrations such as `InitialCreate` and `UpdateBirthDateToDate`.  
You can open SQL Server Management Studio (SSMS) to verify that the database and tables have been created.*

### 3. Run the Application

Set `MemberManagement.Web` as the startup project and run it via Visual Studio (F5) or the CLI:

```bash
cd MemberManagement.Web
dotnet run

```

---

## 📂 Project Structure Highlights

* **Custom Assets**: Dedicated CSS and JS for member pages (Create, Edit, Details, Index) are organized within `wwwroot/css/memberpg` and `wwwroot/js/memberpg`.
* **Validation**: Implements dual-layered validation with `MemberVMValidator.cs` in the Web layer and `MemberValidator.cs` in the Application layer.
* **Design Patterns**: Utilizes the **Repository Pattern** (`IMemberRepository`) and **Service Pattern** (`IMemberService`) to decouple business logic from data access.
* **Data Mapping**: Uses specialized mappers (`MemberMapper.cs`) to handle transformations between Domain Entities and ViewModels.

---
