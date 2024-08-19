using BookStore.Data;
using BookStore.MappingProfiles;
using BookStore.Repositories.IRepositories;
using BookStore.Repositories.Repositories;
using BookStore.Services.IServices;
using BookStore.Services.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog.Sinks.MSSqlServer;
using Serilog;
using System.Collections.ObjectModel;
using System.Text;
using BookStore.Middlewares.Log;
using BookStore.Middlewares.ExceptionHandler;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext and Identity
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Configure JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"];
var key = Encoding.ASCII.GetBytes(jwtKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddHttpContextAccessor();

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();


builder.Host.UseSerilog();

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddScoped<IBookRepository, BookRepository>();

builder.Services.AddScoped<IBookService, BookService>();

builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();

builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();

builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.AddControllers();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}
var columnOptions = new ColumnOptions
{
    AdditionalColumns = new Collection<SqlColumn>
    {
        new SqlColumn("RequestPath", System.Data.SqlDbType.NVarChar, true),
        new SqlColumn("UserName", System.Data.SqlDbType.NVarChar, true),
        new SqlColumn("ResponseBody", System.Data.SqlDbType.NVarChar, true) // -1 for max length
    }
};

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.MSSqlServer(
        connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
        sinkOptions: new MSSqlServerSinkOptions { TableName = "Logs", AutoCreateSqlTable = true },
        columnOptions: columnOptions
    )
    .CreateLogger();
app.UseCsp(csp =>
    csp.DefaultSources(s => s.Self())
       .ScriptSources(s => s.Self().UnsafeInline())
       .StyleSources(s => s.Self()));

app.UseMiddleware<RequestLoggingMiddleware>();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
