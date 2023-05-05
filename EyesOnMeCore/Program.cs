using EyesOnMeCore;
using EyesOnMeCore.Data;
using EyesOnMeCore.Pages;
using Google.Apis.Auth.AspNetCore3;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using System.IO;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
connectionString = "Server=tcp:cbennettsdevserver.database.windows.net,1433;Initial Catalog=EOUTesting;Persist Security Info=False;User ID=CBennetts;Password=azureVenice2013!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=60;";
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});

builder.Services

        .AddAuthentication(o =>
        {
            //o.DefaultChallengeScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;
            //o.DefaultForbidScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;
            //o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        })
        .AddCookie()
        .AddGoogleOpenIdConnect(options =>
        {
            options.ClientId = "596717765407-52qi3h1otn9pdpfaeam1j9uluh90rthi.apps.googleusercontent.com";
            options.ClientSecret = "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";
        });


builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});

//builder.Services.AddAuthentication().AddGoogle(googleOptions =>
//{
//    //googleOptions.AuthorizationEndpoint = "https://localhost:7147/signin-google";
//    googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
//    googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
//});


//builder.Services.AddAuthentication()
//   .AddGoogle(options =>
//   {
//       IConfigurationSection googleAuthNSection =
//       builder.Configuration.GetSection("Authentication:Google");
//       options.ClientId = googleAuthNSection["ClientId"];
//       options.ClientSecret = googleAuthNSection["ClientSecret"];
//   });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.MapPost("/PostRequest", async (IConfiguration config, HttpContext context) =>
{
    var filePath = "UserText/" + Path.GetRandomFileName();

    await using var writeStream = File.Create(filePath);
    await context.Request.Body.CopyToAsync(writeStream);
    writeStream.Close();
    string readText = File.ReadAllText(filePath);
    LegalRequestModel legalRequest = new LegalRequestModel();
    bool result = await legalRequest.RunRequests(readText);

    return result;
});

//app.MapPost("/EmailRequest", async (IConfiguration config, HttpContext context) =>
//{
//    var filePath = "UserText/" + Path.GetRandomFileName();

//    await using var writeStream = File.Create(filePath);
//    await context.Request.Body.CopyToAsync(writeStream);
//    writeStream.Close();
//    string readText = File.ReadAllText(filePath);
//    EmailCheckModel emailRequest = new EmailCheckModel();
//    await emailRequest.RunScanAndSend(readText);

//});

app.MapPost("/DelRequest", async (IConfiguration config, HttpContext context) =>
{
    var filePath = "UserText/" + Path.GetRandomFileName();

    await using var writeStream = File.Create(filePath);
    await context.Request.Body.CopyToAsync(writeStream);
    writeStream.Close();
    string readText = File.ReadAllText(filePath);
    DatabaseAccess database = new DatabaseAccess();
    if (!string.IsNullOrEmpty(readText))
    {
        database.SetData($"DELETE FROM dbo.DataManagementRequest WHERE RequestUserID = '{readText}';");
    }
});

app.Run();
