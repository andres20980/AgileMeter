using everisapi.API.Entities;
using everisapi.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace everisapi.API.Services
{
  public interface IUnidadInfoRepository
  {
    //Return all Unidads
    IEnumerable<UnidadEntity> GetUnidades( int OficinaId );
  }
}