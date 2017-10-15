using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoPilotSimulation.AutoPilot;
using Geometry;

namespace AutoPilotSimulation.Simulation
{
  class GPSSimulator : IGPS
  {
    public GPSSimulator()
    {
      
    }

    public void Age(Time elapsedtime, Coordinate reallocation)
    {
      var now = DateTime.Now;
      if (null == LastReading || now - LastReading.Time > new TimeSpan(0, 0, 0, 0, 300))
        LastReading = new GPSReading(DateTime.Now, Ball.EarthSurfaceApproximation.Arc(Length.FromMeters(4)), reallocation);
    }

    public GPSReading LastReading { get; private set; }
  }
}
