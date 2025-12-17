using DbOperation.Interface;
using DbOperation.Implementation;
using Microsoft.AspNetCore.HttpOverrides;

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
