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
        [HttpPost("actualizarCliente/")]
        public async Task<ActionResult> actualizarCliente(TCliente cliente)
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
                throw new Exception("Error al intentar actulizar un Cliente", ex);
            }
        }

        //// GET: api/<ClienteController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<ClienteController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<ClienteController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<ClienteController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<ClienteController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}