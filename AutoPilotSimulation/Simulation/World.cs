using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using AutoPilotSimulation.Geometric;

namespace AutoPilotSimulation.Simulation
{
  public class World
  {
    public Mesh Mesh;
    private readonly List<IFleeting> _things = new List<IFleeting>();

    private static readonly Coordinate PointRobertsMarina = new Coordinate(Latitude.FromDegrees(48, 58, 44.02),
                                                                       Longitude.FromDegrees(123, 4, 4.09));
    
    public World()
    {
      Mesh = new Mesh(PointRobertsMarina, Length.FromMeters(1000));

      var boat = new Boat(new Coordinate(Latitude.FromDegrees(0), Longitude.FromDegrees(0)), Velocity.FromMPH(6),
                          Bearing.West, Angle.FromDegrees(20));
      _things.Add(boat);
    }

    private long _lastupdateticks = DateTime.Now.Ticks;

    public void Update()
    {
      var ticks = DateTime.Now.Ticks;
      var elapsedtime = Time.FromTicks(ticks - _lastupdateticks);
      _lastupdateticks = ticks;

      foreach (var thing in _things)
        thing.Update(elapsedtime);
    }
  }
}
