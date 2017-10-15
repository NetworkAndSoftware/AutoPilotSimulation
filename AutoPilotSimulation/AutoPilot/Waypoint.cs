using Geometry;

namespace AutoPilotSimulation.AutoPilot
{
  public class Waypoint
  {
    public Waypoint(Coordinate coordinate)
    {
      Location = coordinate;
      Reached = false;
    }

    public Coordinate Location { get; set; }
    public bool Reached { get; set; }
  }
}