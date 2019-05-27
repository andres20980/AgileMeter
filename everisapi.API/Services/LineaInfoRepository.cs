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
  public class LineaInfoRepository : ILineaInfoRepository
  {
    private AsignacionInfoContext _context;
    //Le damos un contexto en el constructor
    public LineaInfoRepository(AsignacionInfoContext context)
    {
      _context = context;
    }    
    //return all lineaes of an office
    public IEnumerable<LineaEntity> GetLineas( int UnidadId)
    {
      return _context.Linea.Where(l => l.UnidadId == UnidadId).ToList();
    }
    
    //Este metodo nos permite persistir los cambios en las entidades
    public bool SaveChanges()
    {
      return (_context.SaveChanges() >= 0);
    }    
  }
}
