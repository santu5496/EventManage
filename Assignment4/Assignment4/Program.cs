using DbOperation.Interface;
using DbOperation.Implementation;

var builder = WebApplication.CreateBuilder(args);

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
    pattern: "{controller=home}/{action=login}/{id?}");

app.Run();
