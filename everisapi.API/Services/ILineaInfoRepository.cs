using everisapi.API.Entities;
using everisapi.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace everisapi.API.Services
{
  public interface ILineaInfoRepository
  {
    //Return all Lineas
    IEnumerable<LineaEntity> GetLineas( int UnidadId );
  }
}