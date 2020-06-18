using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using everisapi.API.Entities;
using everisapi.API.Models;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace everisapi.API.Services
{
  public class PuntuacionSectionRepository: IPuntuacionSectionRepository
  {
    private AsignacionInfoContext _context;
    //Le damos un contexto en el constructor
    public PuntuacionSectionRepository(AsignacionInfoContext context)
    {
      _context = context;
    }    
    //return all
    public IEnumerable<PuntuacionSectionEntity> GetPuntuacionSection() // probar a cambiar la entidad en el asigContext y aqui
    {
      return _context.PuntuacionSection.ToList();
    }
    
    public bool setPuntuacionSection(int evalId,int secId, int nivel, float puntuacion)
    {
        _context.PuntuacionSection.Add(new PuntuacionSectionEntity {
            EvaluacionId = evalId,
            SectionId = secId,
            NivelAlcanzado = nivel,
            Puntuacion = puntuacion
        });
        return SaveChanges();
    }

    //Este metodo nos permite persistir los cambios en las entidades
    public bool SaveChanges()
    {
      return (_context.SaveChanges() >= 0);
    }    
  }
}
