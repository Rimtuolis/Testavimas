using PSA.Server.Services;
using PSA.Services;
using PSA.Shared;
using MySqlConnector;
using System.Text;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;

var builder = WebApplication.CreateBuilder(args);
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
// Add services to the container.
builder.Services.AddTransient(_ => new MySqlConnection(builder.Configuration["ConnectionStrings:Default"]));
builder.Services.Configure<MailOptions>(builder.Configuration.GetSection("MailConfiguration"));

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSingleton<IDatabaseOperationsService, DatabaseOperationsService>();
builder.Services.AddSingleton<ICurrentUserService, CurrentUserService>();
builder.Services.AddSingleton<IMailService, MailService>();
builder.Services.AddSingleton<ICartService, CartService>();
builder.Services.AddSingleton<IBlackJackService, BlackJackService>();
builder.Services.AddSingleton<IManualBuildingService, ManualBuildingService>();
builder.Services.AddSingleton<IAutoBuildingService, AutoBuildingService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
