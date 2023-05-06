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

namespace Cemsa_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
                using (var db = new CemsaContext())
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
