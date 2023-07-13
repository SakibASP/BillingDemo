using BillingDemo.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//For connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//For DbContext
builder.Services.AddDbContext<BilllingDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add services to the container.
builder.Services.AddControllersWithViews();

//Use of IMemoryCache
builder.Services.AddDistributedMemoryCache();
//Use of session (For future use)
builder.Services.AddSession(options =>
{
    options.Cookie.Name = "myBilling_Session";
    options.IdleTimeout = TimeSpan.FromMinutes(5);
});
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();


// Add services to the container.
builder.Services.AddMvc();
builder.Services.AddMvc().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
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
