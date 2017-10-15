using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPilotSimulation.AutoPilot
{
  public interface IGPS
  {
    GPSReading LastReading { get; }
  }
}
