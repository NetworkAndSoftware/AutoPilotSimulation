using System.Security.Cryptography.X509Certificates;
using Geometry;

namespace AutoPilotSimulation.AutoPilot
{
  public interface ICompass
  {
    Bearing Bearing { get; set; } 
  }
}