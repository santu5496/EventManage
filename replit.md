# Event Management System

## Overview
An ASP.NET Core 8.0 MVC web application for event management and booking. Features include user authentication, event CRUD operations, booking management, billing, and dashboard.

## Project Structure
```
Assignment4/
├── Assignment4/           # Main ASP.NET Core MVC web project
│   ├── Controllers/       # MVC Controllers
│   ├── Views/            # Razor Views
│   ├── wwwroot/          # Static files (CSS, JS, images)
│   ├── Program.cs        # Application entry point
│   └── appsettings.json  # Configuration
├── DbOperation/          # Data access layer
│   ├── Implementation/   # Service implementations
│   ├── Interface/        # Service interfaces
│   └── Models/           # Entity Framework models
└── EventManagement.sln   # Solution file
```

## Technology Stack
- ASP.NET Core 8.0 MVC
- Entity Framework Core with SQLite
- Bootstrap, jQuery, SweetAlert2
- SkiaSharp for image processing
- ZXing.Net for barcode generation

## Running the Application
The application runs on port 5000 via the configured workflow:
```bash
cd Assignment4/Assignment4 && dotnet run
```

## Database
The application uses SQLite database stored locally as `EventManagement.db`. The database is created automatically on first run.

## Recent Changes
- December 17, 2025: Initial Replit setup
  - Configured Kestrel to bind to 0.0.0.0:5000
  - Added forwarded headers support for Replit proxy
  - Removed HTTPS redirection for development environment
  - Migrated from SQL Server to SQLite for local database storage
