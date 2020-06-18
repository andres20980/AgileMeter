using everisapi.API.Entities;
using everisapi.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace everisapi.API.Services
{
  public interface IPuntuacionSectionRepository
  {
    //Return all puntuaciones sections
    IEnumerable<PuntuacionSectionEntity> GetPuntuacionSection();
    bool setPuntuacionSection(int secId, int evalId, int nivel, float puntuacion);
  }
}