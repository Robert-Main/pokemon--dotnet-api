
using api.Data;
using api.Mapping;
using Microsoft.EntityFrameworkCore;
using PokemonReviewApp;
using Scalar.AspNetCore;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(PokemonProfile));
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddTransient<Seed>();
builder.Services.AddScoped<api.Interfaces.IPokemonInterface, api.Repositories.PokemonRepository>();
builder.Services.AddScoped<api.Interfaces.ICategoryInterface, api.Repositories.CategoryRepository>();
builder.Services.AddScoped<api.Interfaces.ICountryInterface, api.Repositories.CountryRepository>();
builder.Services.AddScoped<api.Interfaces.IOwnerInterface, api.Repositories.OwnerRepository>();
builder.Services.AddScoped<api.Interfaces.IReviewerInterface, api.Repositories.ReviewerRepository>();
builder.Services.AddScoped<api.Interfaces.IReviewInterface, api.Repositories.ReviewRepository>();

var app = builder.Build();
if (args.Length == 1 && args[0].ToLower() == "seeddata")
    SeedData(app);

void SeedData(IHost app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using (var scope = scopedFactory.CreateScope())
    {
        var service = scope.ServiceProvider.GetService<Seed>();
        service.SeedDataContext();
    }
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
