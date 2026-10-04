using Domain.Contracts;
using Domain.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Persistance.Data;
using Persistance.Data.DataSeed;
using Persistance.Repositories;
using Presentation.Controllers;
using Presentation.Handlers;
using Service.Attachments;
using Service.MappingProfiles;
using Service.Options;
using Service.Services;
using ServiceAbstraction;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddApplicationPart(typeof(ProfilesController).Assembly);

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Attachments ---------------------------------------------------------------
// Where files land, which types are accepted, how large they may be and how they are
// addressed are all configuration, so none of it has to be sent by the client.
builder.Services.Configure<AttachmentOptions>(builder.Configuration.GetSection(AttachmentOptions.SectionName));

var attachmentOptions = builder.Configuration.GetSection(AttachmentOptions.SectionName).Get<AttachmentOptions>() ?? new AttachmentOptions();

// The transport has to tolerate the biggest file the configuration allows, otherwise the
// request is rejected before the upload service ever sees it.
var maxUploadBytes = Math.Max(
    attachmentOptions.MaxSizeInBytes,
    attachmentOptions.MaxSizePerExtensionInBytes.Values.DefaultIfEmpty(0).Max());

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = maxUploadBytes;
    options.ValueLengthLimit = int.MaxValue;
});

builder.WebHost.ConfigureKestrel(kestrel => kestrel.Limits.MaxRequestBodySize = maxUploadBytes);

builder.Services.AddSingleton<IAttachmentService, AttachementService>();

// The write path never produces a URL, so the read path needs a way to compose one. This is
// separate from AttachementService on purpose: it depends on configuration, not on the disk.
// Cross-origin access stays closed until an origin is listed under "Cors:AllowedOrigins".
builder.Services.AddCors(options =>
{
    var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];

    options.AddDefaultPolicy(policy =>
    {
        if (allowedOrigins.Length > 0)
            policy.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddExceptionHandler<AttachmentExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddDbContext<StoreDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
});

// Identity + JWT -------------------------------------------------------------
// There is a single admin account, so Identity is here for the one thing it is genuinely better at
// than anything hand-rolled: hashing the password with a salted PBKDF2 and verifying it in constant
// time. Everything else about the session is stateless — the browser keeps the token minted by
// /api/auth/login and every admin request proves who it is by presenting that token.
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.SectionName));
builder.Services.Configure<AdminSeedOptions>(builder.Configuration.GetSection(AdminSeedOptions.SectionName));

var jwtOptions = builder.Configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>() ?? new JwtOptions();

// A short HMAC key is the one mistake that makes every token forgeable, so the API refuses to start
// rather than mint tokens signed with something guessable.
if (Encoding.UTF8.GetByteCount(jwtOptions.SigningKey) < 32)
{
    throw new InvalidOperationException(
        "Jwt:SigningKey must be at least 32 bytes (256 bits) to sign tokens with HMAC-SHA256.");
}

builder.Services
    .AddIdentityCore<ApplicationUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<StoreDbContext>();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // Claims keep the names they were issued with instead of being rewritten to the legacy
        // ClaimTypes.* URIs, so "sub" and "email" mean the same thing on both sides.
        options.MapInboundClaims = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtOptions.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey)),
            // Expiry is enforced from the token itself, with no grace period, so a token that says it
            // is dead is rejected on the very next request.
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
        };
    });

builder.Services.AddAuthorization();

// Unit Of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Services
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IProjectImageService, ProjectImageService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<ITechnologyService, TechnologyService>();
builder.Services.AddScoped<ISkillService, SkillService>();
builder.Services.AddScoped<ITypeService, TypeService>();
builder.Services.AddScoped<IServiceService, ServiceService>();
builder.Services.AddScoped<IExperienceService, ExperienceService>();
builder.Services.AddScoped<IEducationService, EducationService>();
builder.Services.AddScoped<ICertificationService, CertificationService>();
builder.Services.AddScoped<IAchievementService, AchievementService>();
builder.Services.AddScoped<ISocialLinkService, SocialLinkService>();
builder.Services.AddScoped<IContactMessageService, ContactMessageService>();
builder.Services.AddScoped<IResumeService, ResumeService>();
builder.Services.AddScoped<IBlogPostService, BlogPostService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// AutoMapper builds profiles with a parameterless constructor, so the attachment-aware ones are
// registered explicitly with the URL composer rather than discovered by scanning the assembly.
var attachmentUrls = new AttachmentUrls(builder.Configuration, Options.Create(attachmentOptions));

builder.Services.AddSingleton(attachmentUrls);

builder.Services.AddAutoMapper(cfg => cfg.AddProfiles(
[
    new AchievementProfile(attachmentUrls),
    new BlogPostProfile(attachmentUrls),
    new CategoryProfile(),
    new CertificationProfile(attachmentUrls),
    new ContactMessageProfile(),
    new EducationProfile(attachmentUrls),
    new ExperienceProfile(attachmentUrls),
    new ProfileProfile(attachmentUrls),
    new ProjectImageProfile(attachmentUrls),
    new ProjectProfile(attachmentUrls),
    new ResumeProfile(attachmentUrls),
    new ServiceProfile(attachmentUrls),
    new SkillProfile(attachmentUrls),
    new SocialLinkProfile(attachmentUrls),
    new TagProfile(),
    new TechnologyProfile(attachmentUrls),
    new TypeProfile()
]));

var app = builder.Build();

// Creates the admin account from the AdminSeed configuration the first time the API runs against a
// database that does not have one yet. It never touches an account that already exists.
try
{
    await app.Services.SeedAdminUserAsync();
}
catch (Exception exception)
{
    throw new InvalidOperationException(
        "Admin seeding failed. The AspNetUsers table arrives with the AddIdentityAuth migration. " +
        "Apply it from the solution root with: dotnet ef database update " +
        "--project Infrastructure\\Persistance --startup-project Portofolio",
        exception);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Portofolio v1");
        options.RoutePrefix = "swagger";
        options.DocumentTitle = "Portofolio API";
    });
}

// The HTTP development profile is the target of the Vite proxy. Production deployments should
// enforce HTTPS, while local HTTP development must not redirect the proxied multipart requests to
// an HTTPS port that may not be running.
if (!app.Environment.IsDevelopment())
    app.UseHttpsRedirection();

// Serves the attachment folder and nothing else, so no other part of the web root is exposed.
var attachmentsRoot = Path.Combine(
    string.IsNullOrEmpty(builder.Environment.WebRootPath)
        ? Path.Combine(builder.Environment.ContentRootPath, "wwwroot")
        : builder.Environment.WebRootPath,
    attachmentOptions.RootPath);

Directory.CreateDirectory(attachmentsRoot);

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(attachmentsRoot),
    RequestPath = attachmentOptions.RequestPath,
    ServeUnknownFileTypes = false,
    OnPrepareResponse = context =>
    {
        var headers = context.Context.Response.Headers;

        // Uploaded bytes are never trusted, so the browser is told not to sniff their type.
        headers["X-Content-Type-Options"] = "nosniff";

        // SVG is accepted for logos and diagrams but can carry script, so it is neutralised.
        // Other types (notably the PDF behind the resume) are left alone so viewers still work.
        if (context.File.Name.EndsWith(".svg", StringComparison.OrdinalIgnoreCase))
            headers["Content-Security-Policy"] = "default-src 'none'; style-src 'unsafe-inline'; sandbox";
    },
});

app.UseExceptionHandler();
app.UseCors();

// Authentication has to run before authorization: [Authorize] is answered by the authorization
// middleware, which asks the authentication middleware who the caller is. Without a token the
// challenge turns into a 401 before the action is ever invoked.
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();