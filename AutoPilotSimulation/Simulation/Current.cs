using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Geometry;

namespace AutoPilotSimulation.Simulation
{
  public class Current : IFleeting, IMoving
  {
    public Bearing Direction { get; private set; }
    public Velocity Speed { get; private set; }

    private World _world;

    public Current(Bearing direction, Velocity speed, World world)
    {
      Direction = direction;
      Speed = speed;
      _world = world;
    }
    public void Age(Time elapsedtime)
    {
      foreach (var boat in _world._fleetingthings.Where(thing => thing is Boat).Cast<Boat>())
      {
        var distance = Speed*elapsedtime;
        boat.Location = boat.Location.GreatCircle(new AngularVector(Direction, Ball.EarthSurfaceApproximation.Arc(distance)));
      }
    }
  }
}
