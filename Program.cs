using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Reflection;
using WebAPICoreDapper.Resources;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Logging.AddFile(builder.Configuration.GetSection("Logging"));

var supportedCultures = new[] { 
    new CultureInfo("vi-VN"),
    new CultureInfo("en-US") 
};
var options = new RequestLocalizationOptions()
{
    DefaultRequestCulture = new RequestCulture(culture: "vi-VN", uiCulture: "vi-VN"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
};
options.RequestCultureProviders = new[]
{
    new RouteDataRequestCultureProvider() {Options = options }
};
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddMvc()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization(options =>
    {
        options.DataAnnotationLocalizerProvider = (type, factory) =>
        {
            var assemblyName = new AssemblyName(typeof(SharedResource).GetTypeInfo().Assembly.FullName);
            return factory.Create("SharedResource", assemblyName.Name);
        };
    });

builder.Services.AddControllers()
    .AddNewtonsoftJson(opt =>
    {
        opt.SerializerSettings.ContractResolver = new DefaultContractResolver();
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c  =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Phuocnh Rest API Dapper",
        Version = "v1"
    });
});

builder.Services.AddSingleton(options);
builder.Services.AddSingleton<LocService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
