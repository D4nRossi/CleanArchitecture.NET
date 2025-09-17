using CleanArchMvc.Domain.Account;
using CleanArchMvc.Infra.IoC;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// ===== Services =====
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllersWithViews();

var app = builder.Build();

// ===== Pipeline =====
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
} else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // padrão 30 dias
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Identity / Auth
app.UseAuthentication();
app.UseAuthorization();

// ===== Seeding (roles/usuários) =====
using (var scope = app.Services.CreateScope())
{
    var seed = scope.ServiceProvider.GetRequiredService<ISeedUserRoleInitial>();
    seed.SeedRoles();
    seed.SeedUsers();
}

// ===== Endpoints =====
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
