# 🧭 Morshed – Tour Guide Web Application

Morshed is a tour guide web application built with ASP.NET MVC that helps users explore Egyptian provinces, discover popular places, and create personalized tour plans.

The project is designed using a clean layered architecture and follows common enterprise patterns such as Repository and Unit of Work, making it easy to extend and maintain.

---

## 🚀 Features

### 👤 User Management
- User registration and authentication
- Profile and account settings
- Role-based authorization (Admin / User)
- ASP.NET Identity integration

### 🌍 Provinces & Places
- Browse provinces
- View province details
- Explore places with images and descriptions
- Search functionality

### 🧳 Tours
- Create custom tour plans
- View and manage user tours
- Tour details pages

### 🔖 Bookmarks
- Save favorite places
- User-specific bookmarked content

### 🛠 Admin Dashboard
- Create and manage provinces
- Create and manage places
- Manage reviews
- Manage users and roles

---

## 🛠 Tech Stack

Backend
- ASP.NET MVC (.NET 9)
- C#
- Entity Framework Core
- ASP.NET Identity

Frontend
- Razor Views
- HTML, CSS, JavaScript
- Bootstrap
- jQuery

Database
- SQL Server
- EF Core (Code First)

---

## 🧱 Architecture Overview

The solution follows a layered architecture:

Morshed.sln
│
├── Morshed.Core
│   ├── Entities        Domain models
│   ├── Interfaces     Repository & service contracts
│   └── Constants      Shared constants (roles, etc.)
│
├── Morshed.Infrastructure
│   ├── Data            DbContext, repositories, UnitOfWork
│   ├── Services        Business logic
│   └── Migrations      Database migrations
│
└── Morshed.Web
    ├── Controllers     MVC controllers
    ├── Models          ViewModels
    ├── Views           Razor views
    ├── wwwroot         Static assets
    └── Program.cs      App configuration

---

## 🗄 Database Design

The database is implemented using Entity Framework Core with a code-first approach.

Main entities include:
- ApplicationUser
- Province
- Place
- Review
- Tour
- TourStop
- Bookmark
- TransportOption

Migrations are maintained inside the Infrastructure layer.

---

## ⚙️ Getting Started

Prerequisites
- .NET SDK 9
- SQL Server
- Visual Studio 2022 or later

Run Locally

1. Clone the repository
   git clone <repository-url>

2. Open Morshed.sln in Visual Studio

3. Update the database connection string in:
   Morshed.Web/appsettings.json

4. Apply migrations
   Update-Database

5. Run the project
   Ctrl + F5

---

## 🧩 Future Enhancements
- Static maps per province
- Advanced search and filtering
- Multilingual support (Arabic and English)
- Export tour plans
- UI and UX improvements

---

## 👨‍💻 Author

Karim Hamed  
Computer Science Student  
ASP.NET MVC Developer
