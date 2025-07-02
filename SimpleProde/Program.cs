using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SimpleProde;
using SimpleProde.Entities;
using SimpleProde.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
#region Services Area


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//builder.Services.AddIdentityCore<User>()
//    .AddSignInManager<User>()
//    .AddUserManager<User>()
//    .AddEntityFrameworkStores<ApplicationDbContext>()
//    .AddDefaultTokenProviders();

builder.Services.AddAutoMapper(typeof(Program));


builder.Services.AddIdentityCore<User>()//.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<UserManager<User>>();
builder.Services.AddScoped<SignInManager<User>>();

builder.Services.AddAuthentication().AddJwtBearer(options =>
{  
    //no se cambie los nombre de los claims
    options.MapInboundClaims = false;

    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        //no validamos el emisor
        ValidateIssuer = false,
        //
        ValidateAudience = false,
        //valida el token con fecha de vendimiento
        ValidateLifetime = true,
        // valida la llave privada
        ValidateIssuerSigningKey = true,
        //configura la llave privada
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["llavejwt"]!)),
        ClockSkew = TimeSpan.Zero
        
    };

});
builder.Services.AddAuthorization(opciones =>
{
    opciones.AddPolicy("esadmin", politica => politica.RequireClaim("esadmin"));
});

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddOutputCache(opciones =>
{
    opciones.DefaultExpirationTimeSpan = TimeSpan.FromSeconds(60);
});


//var origenesPermitidos = builder.Configuration.GetValue<string>("origenesPermitidos")!.Split(",");

//builder.Services.AddCors(opciones =>
//{
//    opciones.AddDefaultPolicy(opcionesCORS =>
//    {
//        opcionesCORS.WithOrigins(origenesPermitidos).AllowAnyMethod().AllowAnyHeader()
//        .WithExposedHeaders("cantidad-total-registros");
//    });
//});

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
        });
});


builder.Services.AddTransient<IStoreFiles, LocalStoreFiles>();
//Para poder usar el httpContext
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserService,UserService>();
builder.Services.AddScoped<ITeamService, TeamService>();

#endregion

var app = builder.Build();


#region Middelware Area

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.UseOutputCache();


#endregion

app.Run();
