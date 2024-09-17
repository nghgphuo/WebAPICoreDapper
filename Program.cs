using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Reflection;
using WebAPICoreDapper.Resources;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using WebAPICoreDapper.Models;
using WebAPICoreDapper.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Logging.AddFile(builder.Configuration.GetSection("Logging"));

builder.Services.AddTransient<IUserStore<AppUser>, UserStore>();
builder.Services.AddTransient<IRoleStore<AppRole>, RoleStore>();
builder.Services.AddIdentity<AppUser, AppRole>()
                .AddDefaultTokenProviders();

#region Localization
builder.Services.AddSingleton<LocService>();
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

var supportedCultures = new[] { 
    new CultureInfo("vi-VN"),
    new CultureInfo("en-US") 
};

var options = new RequestLocalizationOptions()
{
        DefaultRequestCulture = new RequestCulture(culture: "vi-VN", uiCulture: "vi-VN"),
        SupportedCultures = supportedCultures,
        SupportedUICultures = supportedCultures,
};

options.RequestCultureProviders = new[]
{
    new RouteDataRequestCultureProvider() {Options = options }
};

builder.Services.AddSingleton(options);

#endregion

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

var app = builder.Build();

#region GlobalHandleException
app.UseExceptionHandler(options =>
{
    options.Run(async context =>
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var ex = context.Features.Get<IExceptionHandlerFeature>()?.Error;
        if (ex != null) return;

        var error = new
        {
            message = ex.Message
        };

        context.Response.ContentType = "application/json";
        context.Response.ContentType = "application/json";
        context.Response.Headers.Add("Access-Control-Allow-Credentials", new[] { "true" });
        context.Response.Headers.Add("Access-Control-Allow-Origin", new[] { builder.Configuration["AllowedHosts"] });

        using (var writer = new StreamWriter(context.Response.Body))
        {
            new JsonSerializer().Serialize(writer, error);
            await writer.FlushAsync().ConfigureAwait(false);
        }

    });
});

#endregion

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "PHUOCNH REST API V1");
    });
}

app.UseHttpsRedirection();
app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
