﻿using Cemsa_BackEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Cemsa_BackEnd.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("usuario")]
    public class UsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        //var context = new ApplicationDbContext();
        //var repository = new ClienteRepository(context);
        public UsuariosController(ApplicationDbContext context, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            //_context = new ApplicationDbContext();
            _context = context;
            _configuration = configuration;

        }

        // POST: api/usuario/login
        /// <summary>
        /// Verifica que el usuario que quiere iniciar sesión sea un usuario registrado y otorga un Token de autenticación.
        /// </summary>
        /// <param name="usuario">Usuario que desea Iniciar Sesión.</param>
        /// <returns>Token Jwt</returns>
        [HttpPost]
        [Route("login")]
        public dynamic IniciarSesion(TUsuario usuario)
        {
            //int id = usuario.UsrId;
            string user = usuario.Usuario;
            string password = usuario.Password;
            string usu = "";
            int idUsuario = 0;
            int rol = 0;

            //TUsuario usuarioDB = _context.TUsuarios.Where(usuarioDB => usuarioDB.Usuario == user && usuarioDB.Password == password).FirstOrDefault();

            TUsuario usuarioDB = _context.TUsuarios
                .Include(u => u.TClientes)
                .Where(usuarioDB => usuarioDB.Usuario == user && usuarioDB.Password == password)
                .FirstOrDefault();

            if (usuarioDB == null)
            {
                return new
                {
                    idUsuario = 0,
                    rol = 0,
                    usu = "",
                    cliente = "",
                    success = false,
                    message = "Credenciales incorrectas",
                    result = ""
                };
            }

            var jwt = _configuration.GetSection("Jwt").Get<Jwt>();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("usuario", usuarioDB.Usuario)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
            var singIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                jwt.Issuer,
                jwt.Audience,
                claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: singIn
            );

            string cliApeNomDen = usuarioDB.TClientes?.FirstOrDefault()?.CliApeNomDen;

            return new
            {
                idUsuario = usuarioDB.UsrId,
                rol = usuarioDB.Rol,
                usu = usuarioDB.Usuario,
                cliente = cliApeNomDen,
                succes = true,
                message = "exito",
                result = new JwtSecurityTokenHandler().WriteToken(token)
            };
        }
    }
}
