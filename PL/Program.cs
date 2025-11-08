using Microsoft.EntityFrameworkCore;

namespace PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            //Cadena de Conexion
            var conString = builder.Configuration.GetConnectionString("GestionNomina") ??
            throw new InvalidOperationException("Connection string 'GestionNomina'" + " not found.");
            builder.Services.AddDbContext<DL.GestionNominaContext>(options =>
                options.UseSqlServer(conString));

            //Se crea una única instancia de la clase por cada solicitud HTTP entrante.
            builder.Services.AddScoped<BL.Usuario>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
