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
    [Route("api/linea")]    
    public class LineaController : Controller
    {
         //Inyectamos un logger
        private ILogger<LineaController> _logger;
        private ILineaInfoRepository _lineaInfoRepository;           
        //Utilizamos el constructor para inicializar el logger
        public LineaController(ILogger<LineaController> logger, ILineaInfoRepository lineaInfoRepository)
        {
        _logger = logger;
        _lineaInfoRepository = lineaInfoRepository;
        } 


//Return all Lineas of an office
    [Authorize]
    [HttpGet("allLinea/{UnidadId}")]//
    public IActionResult GetLineas(int UnidadId)
    {
      try
      {
        var LineaEntities = _lineaInfoRepository.GetLineas(UnidadId);              
        var results = Mapper.Map<IEnumerable<Linea>>(LineaEntities);                
        _logger.LogInformation("Returns Lineas OK");

        return Ok(results);
      }
      catch (Exception ex)
      {
        _logger.LogCritical($"Se recogio un error al recibir todos los datos de las Lineaas: " + ex);
        return StatusCode(500, "Un error ha ocurrido mientras se procesaba su petición.");
      }

    }
  }
}