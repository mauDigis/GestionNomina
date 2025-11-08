
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Variable para idenificar mi cors(Id)
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

            var builder = WebApplication.CreateBuilder(args);

            //Configuración de CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  policy =>
                                  {
                                      policy.WithOrigins("http://localhost:5201")
                                                        .AllowAnyHeader()
                                                        .AllowAnyMethod();
                                  });
            });

            //Cadena de Conexion
            var conString = builder.Configuration.GetConnectionString("GestionNomina") ??
            throw new InvalidOperationException("Connection string 'GestionNomina'" + " not found.");
            builder.Services.AddDbContext<DL.GestionNominaContext>(options =>
                options.UseSqlServer(conString));

            //Se crea una única instancia de la clase por cada solicitud HTTP entrante.
            builder.Services.AddScoped<BL.Usuario>();

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
            });

            builder.Services.AddAuthorization();

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //Configuración para usar CORS
            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
