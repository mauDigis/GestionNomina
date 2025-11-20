using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Usuario
    {

        //Inyeccion de dependencias de mi DL
        private readonly DL.GestionNominaContext _context;

        public Usuario(DL.GestionNominaContext context)
        {
            _context = context;
        }

        public ML.Result GetUserByNameAndPassword(ML.Usuario usuario)
        {
            ML.Result resultUser = new ML.Result();

            try
            {
                var queryUser = (from UsuarioBD in _context.Usuarios
                                 join RolBD in _context.Rols on UsuarioBD.IdRol equals RolBD.IdPuesto
                                 where UsuarioBD.NombreUsuario == usuario.NombreUsuario && UsuarioBD.PasswordHash == usuario.PasswordHash
                                 select new { 
                                     UsuarioBD, 
                                     RolBD 
                                 }).SingleOrDefault();

                if(queryUser != null)
                {
                    ML.Usuario usuarioML = new ML.Usuario()
                    {
                        IdUsuario = queryUser.UsuarioBD.IdUsuario,
                        NombreUsuario = queryUser.UsuarioBD.NombreUsuario,

                        Rol = new ML.Rol()
                        {
                            IdPuesto = queryUser.RolBD.IdPuesto,
                            Descripcion = queryUser.RolBD.Descripcion
                        }

                    };

                    resultUser.Object = usuarioML;
                    resultUser.Correct = true;
                }

            }
            catch (Exception ex)
            {
                resultUser.Correct = false;
                resultUser.ErrorMessage = ex.Message;
            }

            return resultUser;
        }
    }
}
