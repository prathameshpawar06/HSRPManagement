using HSRP_BAL.IServices;
using HSRP_BAL.Services;
using HSRP_DAL.DBContext;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddAuthorization();

builder.Services.AddDbContext<HSRPDbContext>(options =>
    //options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionStrings")));
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionStrings"), b => b.MigrationsAssembly("HSRP Management-API")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Inject services 
builder.Services.AddScoped<IApplicationUserServices, ApplicationUserServices>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty; // ✅ Makes Swagger UI load at `/`
    });
}

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization(); // ✅ This now works correctly
app.MapControllers(); // ✅ For Web APIs

app.UseDeveloperExceptionPage();
app.Run();
