using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geometry;

namespace AutoPilotSimulation.Simulation
{
  public class Mesh
  {
    public Coordinate Center;
    public Length Size;

    public Mesh(Coordinate center, Length size)
    {
      Center = center;
      Size = size;
    }
  }
}
