using backen_dotnet;
using backen_dotnet.Data;
using backen_dotnet.Interfaces;
using backen_dotnet.Models;
using backen_dotnet.Repository;
using backen_dotnet.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Filters;
using Python.Runtime;
using DinkToPdf.Contracts;
using DinkToPdf;
using System.Data;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    // Customize JsonSerializerSettings here
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
});
   ;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2",new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 12;
    options.SignIn.RequireConfirmedEmail = true;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"])
                )
    };
});

builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICandidatureRepository, CandidatureRepository>();
builder.Services.AddScoped<IInterviewRepository, InterviewRepository>();

builder.Services.AddScoped<IOffreRepository, OffreRepository>();
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<ICVRepository, CVRepository>();
//builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

//builder.Services.AddAuthentication().AddFacebook(options =>
//{
//    options.ClientId = "309856692132149";
//    options.ClientSecret = "70276cdb4ad1f277a552491a1701ce49";
//});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
    // Creation des roles par defaut
    var roles = new[] { "Admin", "User", "Member" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }

    // V�rifie si aucun utilisateur n'existe dans la table
    if (!await userManager.Users.AnyAsync())
    {
        var adminUser = new AppUser
        {
            UserName = "admin",
            Email = "chrouf.est@gmail.com",
            EmailConfirmed = true // Configure comme confirm� pour faciliter la connexion
        };

        // Cr�e l'utilisateur admin par d�faut avec le mot de passe "admin"
        var result = await userManager.CreateAsync(adminUser, "Adminadmin.2020");

        if (result.Succeeded)
        {
            // Recharge l'utilisateur depuis la base de donn�es pour s'assurer qu'il a un Id
            adminUser = await userManager.FindByNameAsync(adminUser.UserName);

            // Ajoute l'utilisateur au r�le Admin
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
        else
        {
            // Affiche les erreurs si la cr�ation �choue
            foreach (var error in result.Errors)
            {
                Console.WriteLine($"Erreur : {error.Description}");
            }
        }
    }
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(options => options
.WithOrigins(new[] { "http://vue-app:8080" } )
.AllowAnyHeader()
.AllowAnyMethod()
.AllowCredentials()
);
app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

app.Run();
