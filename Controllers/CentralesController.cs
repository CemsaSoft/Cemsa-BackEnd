using Cemsa_BackEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cemsa_BackEnd.Controllers
{
    [Route("api/central")]
    [ApiController]
    public class CentralesController : ControllerBase
    {
        //private readonly ApplicationDbContext _context;

        //GET: api/servicios
        [HttpGet]
        public async Task<ActionResult<List<TCentral>>> obtenerCentrales()
        {
            try
            {
                using (var db = new CemsaContext())
                { 
                    var query = await( from tc in db.TCentrals
                               join tec in db.TEstadoCentrals on tc.CenIdEstadoCentral equals tec.EstId
                               join tc2 in db.TClientes on new { tipoDoc = tc.CenTipoDoc, nroDoc = tc.CenNroDoc } equals new { tipoDoc = tc2.CliTipoDoc, nroDoc = tc2.CliNroDoc }
                               join tu in db.TUsuarios on tc2.CliIdUsuario equals tu.UsrId
                               select new
                               {
                                   tc.CenNro,
                                   tc2.CliApeNomDen,
                                   tu.Usuario,
                                   tec.EstDescripcion
                               }).ToListAsync();
                    return Ok(query);
                }                  
            }
            catch (Exception ex)
            {
                throw new Exception("Error al intentar obtener Lista de Centrales", ex);
            }
        }
    }
}
