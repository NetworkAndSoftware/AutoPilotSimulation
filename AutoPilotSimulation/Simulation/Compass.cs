using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoPilotSimulation.AutoPilot;
using Geometry;

namespace AutoPilotSimulation.Simulation
{
  class Compass : ICompass
  {
    public Bearing Bearing { get; set; }
  }
}
