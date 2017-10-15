using System;
using AutoPilotSimulation.AutoPilot;
using Geometry;

namespace AutoPilotSimulation.Simulation
{
  public class Boat : IFleeting, ITillerable, ILocated, IMoving
  {
    public Bearing Bearing { get; private set; }
    public ICompass Compass { get; private set; }
    public Coordinate Location { get; set; }
    public IGPS GPS { get; private set; }
    public IRudder Rudder { get; private set; }
    public Velocity Speed { get; set; }
    public AutoPilot.AutoPilot Autopilot { get; private set; }
    
    public Length TurningDistance = Length.FromMeters(5);

    public Boat(Coordinate location, Velocity speed, Bearing bearing, Rudder rudder)
    {
      Location = location;
      Speed = speed;
      Bearing = bearing;
      Rudder = rudder;
      GPS = new GPSSimulator();
      Autopilot = new AutoPilot.AutoPilot(this);
    }

    public void Age(Time elapsedtime)
    { 
      UpdateLocation(elapsedtime);
      ((GPSSimulator) GPS).Age(elapsedtime, Location);
      
      Autopilot.Update();
      ((Rudder) Rudder).Update(elapsedtime);
      UpdateBearing(elapsedtime);
    }

    private void UpdateLocation(Time elapsedtime)
    {
      Length distancetravelled = Speed * elapsedtime;
      var v = new AngularVector(Bearing, Ball.EarthSurfaceApproximation.Arc(distancetravelled));
      Location = Location.GreatCircle(v);
    }

    private void UpdateBearing(Time elapsedtime)
    {
      Bearing -= Rudder.Position*(elapsedtime/(TurningDistance/Speed));
    }

  }
}