using DbOperation.Interface;
using DbOperation.Implementation;
using DbOperation.Models;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel to bind to port 5000 on all interfaces
builder.WebHost.UseUrls("http://0.0.0.0:5000");

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IUserService, UsersService>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("Assignment4");
    if (string.IsNullOrWhiteSpace(connectionString))
    {
        throw new InvalidOperationException("Connection string 'Assignment4' is not configured or is empty.");
    }
    return new UsersService(connectionString);
});
builder.Services.AddSingleton<ITableViewService, TableViewService>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("Assignment4");
    if (string.IsNullOrWhiteSpace(connectionString))
    {
        throw new InvalidOperationException("Connection string 'Assignment4' is not configured or is empty.");
    }
    return new TableViewService(connectionString);
});
builder.Services.AddSingleton<IEventCrudService, EventCrudService>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("Assignment4");
    if (string.IsNullOrWhiteSpace(connectionString))
    {
        throw new InvalidOperationException("Connection string 'Assignment4' is not configured or is empty.");
    }
    return new EventCrudService(connectionString);
});
builder.Services.AddSingleton<IEventBookingSerive, EventBookingSerive>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("Assignment4");
    if (string.IsNullOrWhiteSpace(connectionString))
    {
        throw new InvalidOperationException("Connection string 'Assignment4' is not configured or is empty.");
    }
    return new EventBookingSerive(connectionString);

});
builder.Services.AddSingleton<IDashBoard, DashBoardSerivice>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("Assignment4");
    if (string.IsNullOrWhiteSpace(connectionString))
    {
        throw new InvalidOperationException("Connection string 'Assignment4' is not configured or is empty.");
    }
    return new DashBoardSerivice(connectionString);
});

var app = builder.Build();

// Seed the database with initial data
var seedConnectionString = builder.Configuration.GetConnectionString("Assignment4");
var dbOptions = new DbContextOptionsBuilder<EventContext>().UseSqlServer(seedConnectionString).Options;
try
{
    using (var db = new EventContext(dbOptions))
    {
        db.Database.EnsureCreated();
        
        if (!db.Users.Any())
    {
        db.Users.Add(new Users
        {
            fullName = "Administrator",
            email = "admin@example.com",
            phoneNumber = "1234567890",
            userRole = "Admin",
            passwordHash = "admin123",
            username = "admin",
            createdDate = DateTime.Now
        });
        
        db.Users.Add(new Users
        {
            fullName = "John Doe",
            email = "john@example.com",
            phoneNumber = "9876543210",
            userRole = "User",
            passwordHash = "user123",
            username = "john",
            createdDate = DateTime.Now
        });
        db.SaveChanges();
    }
    
    if (!db.Events.Any())
    {
        db.Events.Add(new Events
        {
            eventName = "Wedding Ceremony",
            description = "Full wedding event with band and catering",
            userId = 1,
            createdDate = DateTime.Now
        });
        
        db.Events.Add(new Events
        {
            eventName = "Birthday Party",
            description = "Birthday celebration with entertainment",
            userId = 1,
            createdDate = DateTime.Now
        });
        
        db.Events.Add(new Events
        {
            eventName = "Corporate Event",
            description = "Business meeting and corporate gathering",
            userId = 1,
            createdDate = DateTime.Now
        });
        db.SaveChanges();
    }
    
    if (!db.Bookings.Any())
    {
        db.Bookings.Add(new Bookings
        {
            userId = 1,
            eventId = 1,
            bookingDate = DateTime.Now,
            fromDate = DateTime.Now.AddDays(7),
            toDate = DateTime.Now.AddDays(8),
            shiftType = "Full Day",
            bookingStatus = "Confirmed",
            totalAmount = 50000,
            advancePayment = 25000,
            remainingPayment = 25000,
            paymentStatus = "Partial",
            customerName = "Rahul Sharma",
            phoneNumber = "9876543210",
            alternativeNumber = "9876543211",
            address = "123 Main Street, Mumbai",
            EventName = "Wedding Ceremony",
            bandType = "Full Band",
            createdDate = DateTime.Now
        });
        
        db.Bookings.Add(new Bookings
        {
            userId = 1,
            eventId = 2,
            bookingDate = DateTime.Now,
            fromDate = DateTime.Now.AddDays(14),
            toDate = DateTime.Now.AddDays(14),
            shiftType = "Evening",
            bookingStatus = "Pending",
            totalAmount = 15000,
            advancePayment = 5000,
            remainingPayment = 10000,
            paymentStatus = "Unpaid",
            customerName = "Priya Patel",
            phoneNumber = "8765432109",
            address = "456 Park Avenue, Delhi",
            EventName = "Birthday Party",
            bandType = "Sada Band",
            createdDate = DateTime.Now
        });
        db.SaveChanges();
    }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Database seeding error: {ex.Message}");
    Console.WriteLine("The application will continue without seeding. Please check your database connection.");
}

// Configure forwarded headers for Replit proxy
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=login}/{id?}");

app.Run();
