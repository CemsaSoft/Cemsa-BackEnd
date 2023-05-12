using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cemsa_BackEnd.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace Cemsa_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClienteController : ControllerBase
    {
        //GET: api/cliente/obtenerClientes
        /// <summary>
        /// Recupera el listado de Clientes de la Base de datos
        /// </summary>
        /// <returns>Lista de Clientes</returns>
        /// <exception cref="Exception"></exception>
        [HttpGet("listaClientes")]
        public async Task<ActionResult<List<TCliente>>> obtenerClientes()
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var query = await (from tc in db.TClientes
                                       join tu in db.TUsuarios on tc.CliIdUsuario equals tu.UsrId
                                       join ttd in db.TTipoDocumentos on tc.CliTipoDoc equals ttd.TdId
                                       select new
                                       {
                                           tc.CliTipoDoc,
                                           tc.CliNroDoc,
                                           tc.CliIdUsuario,
                                           tc.CliFechaAlta,
                                           tc.FechaBaja,
                                           tc.CliApeNomDen,
                                           tc.CliEmail,
                                           tc.CliTelefono,
                                           tu.Usuario,
                                           ttd.TdDescripcion
                                       }).ToListAsync();
                    return Ok(query);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al intentar obtener Lista de Clientes", ex);
            }
        }

        //GET: api/cliente/obtenerClientes
        /// <summary>
        /// Recupera el listado de Clientes de la Base de datos
        /// </summary>
        /// <returns>Lista de Clientes</returns>
        /// <exception cref="Exception"></exception>
        [HttpGet("obtenerCuenta/{idUsuario}")]
        public async Task<ActionResult<List<TCliente>>> obtenerCuenta(int idUsuario)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var query = await (from tc in db.TClientes
                                       join tu in db.TUsuarios on tc.CliIdUsuario equals tu.UsrId
                                       join ttd in db.TTipoDocumentos on tc.CliTipoDoc equals ttd.TdId
                                       where tc.CliIdUsuario == idUsuario
                                       select new
                                       {
                                           tc.CliTipoDoc,
                                           tc.CliNroDoc,
                                           tc.CliIdUsuario,
                                           tc.CliFechaAlta,
                                           tc.FechaBaja,
                                           tc.CliApeNomDen,
                                           tc.CliEmail,
                                           tc.CliTelefono,
                                           tu.Usuario,
                                           ttd.TdDescripcion
                                       }).ToListAsync();
                    return Ok(query);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al intentar obtener Cuenta de Cliente", ex);
            }
        }

        //GET: api/Cliente/obtenerTipoDoc
        /// <summary>
        /// Recupera el listado de Tipo de Docuemntos de la Base de datos
        /// </summary>
        /// <returns>Lista de Tipo de Documentosl</returns>
        /// <exception cref="Exception"></exception>
        [HttpGet("listaTipoDoc")]
        public async Task<List<TTipoDocumento>> obtenerTipoDoc()
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    return await db.TTipoDocumentos.ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al intentar obtener Lista de Tipo de Documentos", ex);
            }
        }

        //POST: api/Cliente/modificarEstado 
        /// <summary>
        /// Modifica estado de un Cliente
        /// Si la accion 0, fechaBaja = NULL - Si la accion 0, fechaBaja = NOW()
        /// </summary>
        /// <returns>Modifica Estado de un Cliente</returns>
        /// <exception cref="Exception"></exception>        
        [HttpPost("modificarEstado")]
        public async Task<ActionResult> modificarEstado([FromBody] ModificarEstadoRequest request)
        {
            try
            {
                int accion = request.accion;
                int tipoDoc = request.tipoDoc;
                string nroDoc = request.nroDoc;
                using (var db = new ApplicationDbContext())
                {
                    var cliente = await db.TClientes.FirstOrDefaultAsync(c => c.CliTipoDoc == tipoDoc && c.CliNroDoc == nroDoc);
                    if (cliente != null)
                    {
                        cliente.FechaBaja = accion == 1 ? DateTime.Now : null;
                        await db.SaveChangesAsync();
                        return Ok();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al intentar actualizar los datos del Cliente", ex);
            }
        }

        public class ModificarEstadoRequest
        {
            public int accion { get; set; }
            public int tipoDoc { get; set; }
            public string nroDoc { get; set; }
        }

        //POST: api/Cliente/blanquearPassword 
        /// <summary>
        /// Realiza un Blanqueo del Password de un Usuario
        /// </summary>
        /// <returns>Realizar Blanqueo de Password</returns>
        /// <exception cref="Exception"></exception>
        [HttpPost("blanquearPassword/{usrID}")]
        public async Task<ActionResult> blanquearPassword(int usrID)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var usuario = await db.TUsuarios.FirstOrDefaultAsync(u => u.UsrId == usrID);
                    if (usuario != null)
                    {
                        usuario.Password = "123456";
                        await db.SaveChangesAsync();
                        return Ok();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al intentar blanquear Password de un Usuario", ex);
            }
        }

        //POST: api/Cliente/actualizarCliente 
        /// <summary>
        /// Realiza la actualización de un Cliente en la Base de Datos
        /// </summary>
        /// <returns>Realizar Actlizacion de un Cliente</returns>
        /// <exception cref="Exception"></exception>
        [HttpPost("actualizarCliente/{usuario}")]
        public async Task<ActionResult> actualizarCliente(TCliente cliente, string usuario)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var c = await db.TClientes.FirstOrDefaultAsync(u => u.CliTipoDoc == cliente.CliTipoDoc && u.CliNroDoc == cliente.CliNroDoc);
                    if (c != null)
                    {
                        c.CliApeNomDen = cliente.CliApeNomDen;  
                        c.CliEmail = cliente.CliEmail;
                        c.CliTelefono = cliente.CliTelefono;

                        // Actualizar el nombre de usuario en la tabla TUsuario
                        var u = await db.TUsuarios.FirstOrDefaultAsync(u => u.UsrId == c.CliIdUsuario);
                        if (u != null)
                        {
                            u.Usuario = usuario;
                        }
                        await db.SaveChangesAsync();               
                        return Ok();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al intentar actualizar un Cliente", ex);
            }
        }

        //POST: api/Cliente/registarCliente 
        /// <summary>
        /// Realiza el registro de un Cliente e Usuario en la Base de Datos
        /// </summary>
        /// <returns>Realizar Registro de un Cliente e Usuario</returns>
        /// <exception cref="Exception"></exception>
        [HttpPost("registarCliente/{usuario}")]
        public async Task<ActionResult> registarCliente(TCliente cliente, string usuario)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var u = await db.TUsuarios.FirstOrDefaultAsync(u => u.Usuario == usuario);
                    if (u != null)
                    {
                        return BadRequest("El nombre de usuario ya está en uso.");
                    }
                    else
                    {
                        var cc = await db.TClientes.FirstOrDefaultAsync(cc => cc.CliNroDoc == cliente.CliNroDoc && cc.CliTipoDoc == cliente.CliTipoDoc) ;
                        if (cc != null)
                        {
                            return BadRequest("El tipo y número de documento ya está en uso.");
                        }
                        else
                        {
                            // Crea un nuevo usuario con los valores especificados
                            TUsuario nuevoUsuario = new TUsuario
                            {
                                Usuario = usuario,
                                Password = "123456"
                            };

                            // Agrega el usuario a la tabla de usuarios
                            db.TUsuarios.Add(nuevoUsuario);
                            await db.SaveChangesAsync();


                            // Usa el ID generado para el nuevo usuario en el registro de cliente
                            cliente.CliIdUsuario = nuevoUsuario.UsrId;
                            cliente.CliFechaAlta = DateTime.Now;
                            cliente.FechaBaja = null;

                            // Agrega el cliente a la tabla de clientes
                            db.TClientes.Add(cliente);
                            await db.SaveChangesAsync();

                            // Retorna el cliente registrado
                            return Ok();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al intentar Registrar un Cliente", ex);
            }
        }      

        //POST: api/Cliente/verificarUsuarioMod 
        /// <summary>
        /// Realiza una consulta y ve si esta en uso el nommbre de Usuario en la Base de Datos y no sea el que quiero modificar
        /// </summary>
        /// <returns>Entraga si es posible usar el nombre de usuario</returns>
        /// <exception cref="Exception"></exception>
        [HttpPost("verificarUsuarioMod/{usuario}/{idUsuario}")]
        public async Task<ActionResult> verificarUsuarioMod(string usuario, int idUsuario)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var u = await db.TUsuarios.FirstOrDefaultAsync(u => u.Usuario == usuario && u.UsrId != idUsuario);
                    if (u != null)
                    {
                        return BadRequest("El nombre de usuario ya está en uso.");
                    }
                    else
                    {
                        return Ok();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al intentar buscar nombre de Usuario", ex);
            }
        }

        //POST: api/Cliente/obtenerCliente 
        /// <summary>
        /// Realiza una consulta en la BD y entrega el nombre del Cliente
        /// </summary>
        /// <returns>Entrega el nombre del Cliente</returns>
        /// <exception cref="Exception"></exception>
        [HttpPost("obtenerCliente/{idUsuario}")]
        public async Task<ActionResult> obtenerCliente(int idUsuario)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var c = await db.TClientes.FirstOrDefaultAsync(c => c.CliIdUsuario == idUsuario);
                    if (c != null)
                    {
                        return Ok(c.CliApeNomDen);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al intentar buscar nombre del Cliente", ex);
            }
        }

        //POST: api/Cliente/actualizarPassword 
        /// <summary>
        /// Realiza una consulta y verifica contraseña actual y si esta bien modifica la contraseña
        /// </summary>
        /// <returns>Realiza modificación de contraseña</returns>
        /// <exception cref="Exception"></exception>
        [HttpPost("actualizarPassword")]
        public async Task<ActionResult> actualizarPassword([FromBody] ModificarPasswordRequest request)
        {
            try
            {
                int idUsuario = request.idUsuario;
                string password = request.password;
                string newPassword = request.newPassword;

                if (string.IsNullOrWhiteSpace(newPassword))
                {
                    return BadRequest("La nueva contraseña es inválida");
                }

                using (var db = new ApplicationDbContext())
                {
                    var usuario = await db.TUsuarios.FirstOrDefaultAsync(u => u.UsrId == idUsuario && u.Password == password);
                    if (usuario == null)
                    {
                        return BadRequest("La contraseña no es válida");
                    }

                    usuario.Password = newPassword;
                    db.Entry(usuario).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al intentar actualizar contraseña del usuario", ex);
            }
        }

        public class ModificarPasswordRequest
        {
            public int idUsuario { get; set; }
            public string password { get; set; }
            public string newPassword { get; set; }
        }
    }
}