using AutoPilotSimulation.Simulation;

namespace AutoPilotSimulation.AutoPilot
{
  public interface ITillerable
  { 
    IGPS GPS { get; }
    IRudder Rudder { get; }
    ICompass Compass { get; }
  }
}