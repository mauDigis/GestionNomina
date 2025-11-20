using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ML;
using System.Drawing;
using System.Text;

//Importo mi modelo para mapear las propiedades
using static ML.SMTPSettings;

namespace PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region CADENA DE CONEXIÓN

            //Cadena de Conexion
            var conString = builder.Configuration.GetConnectionString("GestionNomina") ??
            throw new InvalidOperationException("Connection string 'GestionNomina'" + " not found.");
            builder.Services.AddDbContext<DL.GestionNominaContext>(options =>
                options.UseSqlServer(conString));

            #endregion

            #region INYECCION DE DEPENDENCIAS

            //Se crea una única instancia de la clase por cada solicitud HTTP entrante.
            builder.Services.AddScoped<BL.Usuario>();

            // Agregar el servicio HttpClient de manera estándar
            builder.Services.AddHttpClient();

            // Registra el servicio de envío de correo
            builder.Services.AddTransient<BL.Correo>();

            // Mapear la sección de configuración "SmtpSettings" a la clase SmtpSettings
            builder.Services.Configure<SMTPSettings>(builder.Configuration.GetSection("SmtpSettings"));

            #endregion

            #region CONFIGURACION Json Web Tokens

            // Obtengo los valores apartir de mi archivo appsettings.json
            var jwtKey = builder.Configuration["Jwt:Key"];
            var jwtIssuer = builder.Configuration["Jwt:Issuer"];
            var jwtAudience = builder.Configuration["Jwt:Audience"];


            //Configura el esquema de autenticación basado en JSON Web Tokens (JWT).
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtIssuer, //Emisor -> Es la entidad(la URL o el nombre del servidor) que generó y firmó el token.
                    ValidAudience = jwtAudience, //Audiencia -> Es la entidad o servicio que está destinado a consumir el token.
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)) // Clave Secreta -> Se utiliza para crear y verificar la firma digital del token JWT.
                };

                //Permite leer mi token desde la cookie

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var token = context.Request.Cookies["JwtToken"];
                        if (!string.IsNullOrEmpty(token))
                        {
                            context.Token = token;
                        }

                        return Task.CompletedTask;
                    }
                };
            });

            #endregion

            //Servcio de 
            builder.Services.AddAuthorization();

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

            //Middleware
            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
