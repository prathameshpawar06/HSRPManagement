using System.Text;
using HSRP_BAL.IServices;
using HSRP_BAL.Services;
using HSRP_DAL.DBContext;
using HSRP_DAL.Domains;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddAuthorization();

builder.Services.AddDbContext<HSRPDbContext>(options =>
    //options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionStrings")));
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionStrings"), b => b.MigrationsAssembly("HSRP Management-API")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//For identity (Registration and Login) services
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<HSRPDbContext>()
    .AddDefaultTokenProviders();

//Inject services 
builder.Services.AddScoped<IApplicationUserServices, ApplicationUserServices>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<IBranchesService, BranchesService>();

//Jwt Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwtSettings = builder.Configuration.GetSection("JwtSettings");
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings["Key"]))
    };
});


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
