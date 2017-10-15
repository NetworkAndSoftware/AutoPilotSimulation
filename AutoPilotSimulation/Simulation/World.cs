using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using AutoPilotSimulation.AutoPilot;
using Geometry;

namespace AutoPilotSimulation.Simulation
{
  public class World
  {
    public readonly Mesh Mesh;
    public readonly List<IFleeting> _fleetingthings = new List<IFleeting>();

    private static readonly Coordinate PointRobertsMarina = new Coordinate(Latitude.FromDegrees(48, 58, 44.02),
                                                                       Longitude.FromDegrees(123, 4, 4.09));
    private static readonly Coordinate Ourhouse = new Coordinate(Latitude.FromDegrees(49, 0, 51.55),
                                                             Longitude.FromDegrees(123, 4, 39.87));
    private static readonly Coordinate Something = new Coordinate(Latitude.FromDegrees(48, 58, 30),
                                                             Longitude.FromDegrees(123, 4, 20));


    public World()
    {
      Mesh = new Mesh(PointRobertsMarina, Length.FromMeters(1000));

      var current=new Current(Bearing.East, Velocity.FromMPH(2), this);

      var rudder = new Rudder(Angle.FromDegrees(45), AngularVelocity.FromDegreesPerSecond(90), Angle.FromDegrees(1));
      var boat = new Boat(PointRobertsMarina, Velocity.FromMPH(25), Bearing.West, rudder);
      rudder.MoveTo(Angle.FromDegrees(22));

      boat.Autopilot.WayPoint = new Waypoint(Something);

      _fleetingthings.Add(current);
      _fleetingthings.Add(boat);

    }

    private long _lastupdateticks = DateTime.Now.Ticks;
    
    public bool Paused;

    public void Age()
    {
      var ticks = DateTime.Now.Ticks;

      var elapsedtime = Time.FromTicks(ticks - _lastupdateticks);
      _lastupdateticks = ticks;

      foreach (var thing in _fleetingthings.Where(thing => !(Paused && thing is IMoving)))
        thing.Age(elapsedtime);
    }

    public IEnumerable<T> Find<T>() where T: IFleeting
    {
      return _fleetingthings.Where(thing => thing is T).Cast<T>();
    }
  }
}
