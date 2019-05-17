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
    [Route("api/oficina")]    
    public class OficinaController : Controller
    {
         //Inyectamos un logger
        private ILogger<OficinaController> _logger;
        private IOficinaInfoRepository _oficinaInfoRepository;           
        //Utilizamos el constructor para inicializar el logger
        public OficinaController(ILogger<OficinaController> logger, IOficinaInfoRepository oficinaInfoRepository)
        {
        _logger = logger;
        _oficinaInfoRepository = oficinaInfoRepository;
        } 


//Return all Oficinas
    [Authorize]
    [HttpGet("allOficina")]
    public IActionResult GetOficinas()
    {
      try
      {
        var OficinaEntities = _oficinaInfoRepository.GetOficinas();
        
        var results = Mapper.Map<IEnumerable<Oficina>>(OficinaEntities);

        _logger.LogInformation("Returns Oficinas OK");

        return Ok(results);
      }
      catch (Exception ex)
      {
        _logger.LogCritical($"Se recogio un error al recibir todos los datos de las Oficinaas: " + ex);
        return StatusCode(500, "Un error ha ocurrido mientras se procesaba su petición.");
      }

    }
  }
}