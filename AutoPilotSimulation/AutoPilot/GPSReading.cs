using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geometry;

namespace AutoPilotSimulation.AutoPilot
{
  public class GPSReading
  {
    public readonly DateTime Time;
    public readonly Angle Accuracy;
    public readonly Coordinate Coordinate;

    public GPSReading(DateTime time, Angle accuracy, Coordinate coordinate)
    {
      Time = time;
      Accuracy = accuracy;
      Coordinate = coordinate;
    }
  }
}
