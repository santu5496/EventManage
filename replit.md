# Event Management Pro

## Overview
This is an ASP.NET Core MVC web application for event management. It provides functionality for managing events, bookings, billing, and user administration.

## Technology Stack
- **Framework**: ASP.NET Core 8.0 (MVC)
- **Database**: SQL Server (external - sql.bsite.net)
- **ORM**: Entity Framework Core
- **Frontend**: Bootstrap, jQuery, Three.js (for visual effects)

## Project Structure
```
Assignment4/
├── Assignment4/           # Main web application
│   ├── Controllers/       # MVC Controllers
│   ├── Views/             # Razor Views
│   ├── wwwroot/           # Static files (CSS, JS, images)
│   ├── Models/            # View models
│   └── Program.cs         # Application entry point
└── DbOperation/           # Data access layer
    ├── Implementation/    # Service implementations
    ├── Interface/         # Service interfaces
    └── Models/            # Entity models (Users, Events, Bookings)
```

## Running the Application
The application runs on port 5000 and is accessible via the Replit webview.

**Workflow**: `ASP.NET Core Server`
```
cd Assignment4/Assignment4 && dotnet run
```

## Database Configuration
The app connects to an external SQL Server database at `sql.bsite.net\MSSQL2016`. 
Connection string is configured in `appsettings.json`.

**Note**: If the SQL Server is not accessible from Replit due to firewall restrictions, the app will show a database seeding error but will continue to run if the database already has data.

## Default Credentials (if database is seeded)
- Admin: username `admin`, password `admin123`
- User: username `john`, password `user123`

## Key Features
- User authentication and registration
- Event management (CRUD operations)
- Booking management
- Billing and payment tracking
- Dashboard with analytics
- Barcode generation for bills

## Deployment
Configured for autoscale deployment:
- Build: `dotnet publish -c Release -o out Assignment4/Assignment4/EventManagement.csproj`
- Run: `dotnet out/EventManagement.dll`

## Recent Changes
- 2025-12-17: Configured for Replit environment (port 5000, 0.0.0.0 binding)
- 2025-12-17: Added forwarded headers support for Replit proxy
- 2025-12-17: Connected to external SQL Server database (sql.bsite.net)
