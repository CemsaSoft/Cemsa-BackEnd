using Cemsa_BackEnd.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using static Cemsa_BackEnd.Controllers.ClienteController;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Cemsa_BackEnd.Controllers
{
    [Route("api/mediciones")]
    [ApiController]
    [Authorize]
    public class MedicionesController : ControllerBase
    {
        //GET: api/mediciones/obtenerMediciones
        /// <summary>
        /// Recupera el listado de Mediciones de la Base de datos de una central
        /// </summary>
        /// <returns>Lista de Mediciones de una central</returns>
        /// <exception cref="Exception"></exception>
        [HttpGet("obtenerMediciones")]
        public async Task<ActionResult<List<Tmedicion>>> obtenerMediciones([FromQuery] int medNro, [FromQuery] DateTime desde, [FromQuery] DateTime hasta)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var query = await (from tm in db.Tmedicions
                                       join s in db.TServicios on tm.MedSer equals s.SerId
                                       where tm.MedNro == medNro && tm.MedFechaHoraSms >= desde && tm.MedFechaHoraSms <= hasta
                                       select new
                                       {
                                           tm.MedId,
                                           tm.MedSer,
                                           tm.MedFechaHoraSms,
                                           tm.MedValor,
                                           s.SerDescripcion
                                       }).ToListAsync();
                    return Ok(query);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al intentar obtener Lista de Mediciones de una Central", ex);
            }
        }

        //GET: api/mediciones/obtenerUltimaMedicionesXCentral
        /// <summary>
        /// Recupera el listado de las ultimas Mediciones de la Base de datos de una central
        /// </summary>
        /// <returns>Lista de las ultimas Mediciones de la central</returns>
        /// <exception cref="Exception"></exception>
        [HttpGet("obtenerUltimaMedicionesXCentral/{medNro}")]
        public async Task<ActionResult<List<Tmedicion>>> obtenerUltimaMedicionesXCentral(int medNro)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var query = await (from ts in db.TServiciosxcentrals
                                       join ts2 in db.TServicios on ts.SxcNroServicio equals ts2.SerId
                                       join tm in db.Tmedicions on new { NroCentral = ts.SxcNroCentral, NroServicio = ts.SxcNroServicio } equals new { NroCentral = tm.MedNro, NroServicio = tm.MedSer }
                                       where ts.SxcNroCentral == medNro
                                           && ts.SxcEstado != 2
                                           && tm.MedFechaHoraSms == db.Tmedicions
                                               .Where(m => m.MedNro == ts.SxcNroCentral && m.MedSer == ts.SxcNroServicio)
                                               .Max(m => m.MedFechaHoraSms)
                                       select new
                                       {
                                           tm.MedValor,
                                           tm.MedFechaHoraSms,
                                           ts2.SerTipoGrafico
                                       }).ToListAsync();


                    return Ok(query);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al intentar obtener Lista de Ultima Mediciones de una Central", ex);
            }
        }









        ////POST: api/Mediciones/registarMediciones
        ///// <summary>
        ///// se cargan varias mediciones, API Solo para registrar mediciones
        ///// </summary>
        ///// <returns>Realizar Registro de varias mediciones</returns>
        ///// <exception cref="Exception"></exception>
        [HttpPost("registarMediciones")]
        public async Task<ActionResult> registarMedicionesTest()
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    DateTime fechaSMS = new DateTime(2023, 6, 22, 0, 0, 0); // Fecha inicial para MedFechaHoraSms
                    DateTime fechaBD = new DateTime(2023, 6, 22, 0, 0, 0); // Fecha inicial para MedFechaHoraBd

                    //Random rnd = new Random();

                    //for (int i = 0; i < 100; i++)
                    //{
                    //int valor = rnd.Next(40, 101); // Genera un número aleatorio entre 40 y 100
                    Tmedicion newMed1 = new Tmedicion
                    {
                        MedNro = 51,
                        MedSer = 1,
                        MedValor = -30,
                        MedFechaHoraSms = fechaSMS,
                        MedFechaHoraBd = fechaBD
                    };

                    Tmedicion newMed2 = new Tmedicion
                    {
                        MedNro = 51,
                        MedSer = 1,
                        MedValor = 90,
                        MedFechaHoraSms = fechaSMS,
                        MedFechaHoraBd = fechaBD
                    };

                    db.Tmedicions.Add(newMed1);
                    db.Tmedicions.Add(newMed2);
                    await db.SaveChangesAsync();

                    //fechaSMS = fechaSMS.AddMinutes(60); // Añade 5 minutos a la fecha de MedFechaHoraSms
                    //fechaBD = fechaBD.AddMinutes(60); // Añade 5 minutos a la fecha de MedFechaHoraBd
                    //}

                    return Ok();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al intentar Registrar las mediciones", ex);
            }
        }

    }
}
