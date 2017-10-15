using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using AutoPilotSimulation.Simulation;
using Geometry;

namespace AutoPilotSimulation.UI
{
  public class Camera : IFleeting
  {
    public enum ViewMode
    { [Description("Default")] Default,
      [Description("Follow 1")] Follow1,
      [Description("Follow 2")] Follow2
    }

    private ViewMode _mode;
    public ViewMode Mode 
    { get { return _mode; } 
      set
      {
        _mode = value;
        Offset = new Tuple<AngularVector, Length>(AngularVector.Zero, Length.Zero);
      }
    }

    public Tuple<Coordinate, Length> Location;
    public Tuple<AngularVector, Length> Offset;

    public Coordinate LookAt { get; set; }
    
    private readonly World _world;
    private readonly Projections _projections;


    public Camera(World world, Projections projections)
    {
      _world = world;
      _projections = projections;
      Offset = new Tuple<AngularVector, Length>(AngularVector.Zero, Length.Zero);
    }

    public void Age(Time elapsedtime)
    {
      switch (Mode)
      {
        case ViewMode.Default:
          return;
        case ViewMode.Follow1:
          UpdateForFollow1();
          break;
        case ViewMode.Follow2:
          UpdateForFollow2();
          break;
      }
    }

    private void UpdateForFollow1()
    {
      var firstboat = _world.Find<Boat>().First();
      if (null == firstboat)
        return;


      var waypoint = firstboat.Autopilot.WayPoint;
      if (null == waypoint || waypoint.Reached)
        return;
      
      var vector =  new AngularVector(firstboat.Location, waypoint.Location);
      var boattocamera = new AngularVector(new Bearing(vector.Bearing + Angle.Pi), new Bearing(Ball.EarthSurfaceApproximation.Arc(Length.FromMeters(15))));

      var boattohalfway = new AngularVector(vector.Bearing, vector.Distance/2);
      LookAt = firstboat.Location.GreatCircle(boattohalfway);

      Location = new Tuple<Coordinate, Length>(firstboat.Location.GreatCircle(boattocamera), Length.FromMeters(3));
    }

    private void UpdateForFollow2()
    {
      var firstboat = _world.Find<Boat>().First();
      if (null == firstboat)
        return;
      
      var waypoint = firstboat.Autopilot.WayPoint;
      if (null == waypoint || waypoint.Reached)
        return;

      var vector = new AngularVector(firstboat.Location, waypoint.Location);

      var boattohalfway = new AngularVector(vector.Bearing, vector.Distance / 2);
      LookAt = firstboat.Location.GreatCircle(boattohalfway);

      var lookattocamera = new AngularVector(new Bearing(vector.Bearing + Angle.Pi), 3 * boattohalfway.Distance);

      var location = LookAt.GreatCircle(lookattocamera);
      var altitude = 3 * Ball.EarthSurfaceApproximation.Distance(lookattocamera.Distance / (2 * Angle.FromDegrees(45).Cos()) * Angle.FromDegrees(45).Sin());

      Location = new Tuple<Coordinate, Length>(location, altitude);
    }

    public bool IsUpdatingProjectionCamera { get; private set; }

    public void UpdateProjectionCamera(ProjectionCamera projectionCamera)
    {
      IsUpdatingProjectionCamera = true;

      var coordinate = Location.Item1.GreatCircle(Offset.Item1);
      var altitude = Location.Item2 + Offset.Item2;
      
      var position = _projections.ToWorld(coordinate, altitude);

      var cartesian = _projections.ToCartesian(LookAt);
      var direction = new Vector3D
        ( _projections.ToWorld(cartesian.X) - position.X,
          _projections.ToWorld(cartesian.Y) - position.Y,
          -position.Z);
      
      projectionCamera.LookDirection = direction;
      projectionCamera.Position = position;

      IsUpdatingProjectionCamera = false;
    }

    public void MoveToProjectionCamera(ProjectionCamera projectionCamera)
    {
      var altitude = _projections.ToModel(projectionCamera.Position.Z);
      var location = _projections.ToModel(projectionCamera.Position.X, projectionCamera.Position.Y);

      if (Mode == ViewMode.Default)
      {
        Location = new Tuple<Coordinate, Length>(location, altitude);
        LookAt = _projections.ToModel(projectionCamera.LookDirection.X, projectionCamera.LookDirection.Y);
        Offset = new Tuple<AngularVector, Length>(AngularVector.Zero, Length.Zero);
      }
      else
      {
        Offset = new Tuple<AngularVector, Length>(new AngularVector(Location.Item1, location), altitude - Location.Item2);
      }
    }
  }
}
