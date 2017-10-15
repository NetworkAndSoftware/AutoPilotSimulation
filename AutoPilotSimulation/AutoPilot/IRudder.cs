using Geometry;

namespace AutoPilotSimulation.AutoPilot
{
  public interface IRudder
  {
    Angle Position { get; }
    void MoveTo(Angle angle);
  }
}