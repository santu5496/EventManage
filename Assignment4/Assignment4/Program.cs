using DbOperation.Interface;
using DbOperation.Implementation;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IConfigurationService, ConfigurationService>(provider =>
{
    return new ConfigurationService(builder.Configuration.GetConnectionString("Assignment4"));
});


builder.Services.AddSingleton<IInventoryMaterialLog, InventoryMaterialLogService>(provider =>
{
    return new InventoryMaterialLogService(builder.Configuration.GetConnectionString("Assignment4"));
});
builder.Services.AddSingleton<IFinishedGoodsService, FinishedGoodsService>(provider =>
{
    return new FinishedGoodsService(builder.Configuration.GetConnectionString("Assignment4"));
});
builder.Services.AddSingleton<IBillingService, BillingService>(provider =>
{
    return new BillingService(builder.Configuration.GetConnectionString("Assignment4"));
});

builder.Services.AddSingleton<IRecipeService,RecipeService>(provider =>
{
    return new RecipeService(builder.Configuration.GetConnectionString("Assignment4"));
});
builder.Services.AddSingleton<IReturnmanagementService, ReturnManagementService>(provider =>
{
    return new ReturnManagementService(builder.Configuration.GetConnectionString("Assignment4"));
});
builder.Services.AddSingleton<IOrderManagement, OrderManagementSerivice>(provider =>
{
    return new OrderManagementSerivice(builder.Configuration.GetConnectionString("Assignment4"));
});
builder.Services.AddSingleton<IReportService, ReportService>(provider =>
{
    return new ReportService(builder.Configuration.GetConnectionString("Assignment4"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
