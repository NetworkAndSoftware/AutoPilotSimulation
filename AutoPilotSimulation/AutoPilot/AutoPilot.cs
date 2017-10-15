using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Geometry;

namespace AutoPilotSimulation.AutoPilot
{
  public class AutoPilot
  {
    private readonly ITillerable _boat;
    private readonly GPSReader _gpsreadings;
    public Length MinimumDistance = Length.FromMeters(5);

    public Waypoint WayPoint { get; set; }
    public Bearing CurrentCourseEstimate { get; private set; }
  

    public AutoPilot(ITillerable boat)
    {
      _boat = boat;
      _gpsreadings = new GPSReader(boat.GPS);
    }

    public void Update()
    {
      _gpsreadings.UpdateGPSReadings();

      if (!HaveWaypoint()) 
        return;

      CurrentCourseEstimate = _gpsreadings.GetCurrectCourseEstimate();

      if (null == CurrentCourseEstimate)
        return;

      var currentlocationestimate = _gpsreadings.MostRecentGPSReading();

      if (null == currentlocationestimate)
        return;

      if (Ball.EarthSurfaceApproximation.Distance(currentlocationestimate.Coordinate.Distance(WayPoint.Location)) > MinimumDistance)
      {
        var desiredbearing = currentlocationestimate.Coordinate.InitialCourse(WayPoint.Location);
        var deviation = CurrentCourseEstimate - desiredbearing;

        _boat.Rudder.MoveTo(deviation);
      }
      else
      {
        WayPoint.Reached = true;
        _boat.Rudder.MoveTo(Angle.Zero);
      }
    }

  
    private bool HaveWaypoint()
    {
      return WayPoint != null && !WayPoint.Reached;
    }
  }
}