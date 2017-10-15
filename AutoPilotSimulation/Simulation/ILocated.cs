using Geometry;

namespace AutoPilotSimulation.Simulation
{
  public interface ILocated
  {
    Coordinate Location { get; set; }
  }
}