using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using everisapi.API.Services;
using everisapi.API.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using everisapi.API.Entities;
using System.Text;
using System.Security.Cryptography;

namespace everisapi.API.Controllers
{     
    [Authorize]
    [Route("api/unidad")]    
    public class UnidadController : Controller
    {
         //Inyectamos un logger
        private ILogger<UnidadController> _logger;
        private IUnidadInfoRepository _unidadInfoRepository;           
        //Utilizamos el constructor para inicializar el logger
        public UnidadController(ILogger<UnidadController> logger, IUnidadInfoRepository unidadInfoRepository)
        {
        _logger = logger;
        _unidadInfoRepository = unidadInfoRepository;
        } 


//Return all Unidads of an office
    [Authorize]
    [HttpGet("allUnidad/{OficinaId}")]//
    public IActionResult GetUnidades(int OficinaId)
    {
      try
      {
        var UnidadEntities = _unidadInfoRepository.GetUnidades(OficinaId);              
        var results = Mapper.Map<IEnumerable<Unidad>>(UnidadEntities);                
        _logger.LogInformation("Returns Unidads OK");

        return Ok(results);
      }
      catch (Exception ex)
      {
        _logger.LogCritical($"Se recogio un error al recibir todos los datos de las Unidadas: " + ex);
        return StatusCode(500, "Un error ha ocurrido mientras se procesaba su petición.");
      }

    }
  }
}