using PlayBaldursGate3.Models;
using PlayBaldursGate3.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddRazorPages();

builder.Services.Configure<GoogleSettings>(builder.Configuration.GetSection("Google"));

builder.Services
    .AddScoped<IPlayListService, PlayListService>()
    .AddScoped<IVideoService, VideoService>()
    .AddRazorPages()
    .AddRazorRuntimeCompilation();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();