using System;
using System.Windows;
using System.Windows.Media.Media3D;
using Geometry;
using HelixToolkit.Wpf;

namespace AutoPilotSimulation.UI
{
  public class Projections
  {
    private readonly Ball _earth;
    private readonly double _worldlength;
    private readonly Coordinate _modelcenter;
    private readonly Length _modellength;

    public Projections(Ball earth, Length modellength, double worldlength, Coordinate modelcenter)
    {
      _earth = earth;
      _modellength = modellength;
      _worldlength = worldlength;
      _modelcenter = modelcenter;
    }

    public double ToWorld(Length length)
    {
      return _worldlength * length / _modellength;
    }

    public Length ToModel(double d)
    {
      return _modellength*d/_worldlength;
    }

    public Coordinate ToModel(double x, double y)
    {
      var cartesian = new CartesianVector(ToModel(x), ToModel(y));
      var vector = _earth.AngularVector(cartesian);
      return _modelcenter.GreatCircle(vector);
    }

    public Tuple<double, double> ToWorld(Coordinate coordinate)
    {
      var vector = ToCartesian(coordinate);

      return new Tuple<double, double>(ToWorld(vector.X), ToWorld(vector.Y));
    }

    public Point3D ToWorld(Coordinate coordinate, Length altitude)
    {
      var cartesianposition = ToCartesian(coordinate);

      return new Point3D(ToWorld(cartesianposition.X), ToWorld(cartesianposition.Y), ToWorld(altitude));
    }

    public CartesianVector ToCartesian(Coordinate coordinate)
    {
      var vector = new AngularVector(_modelcenter, coordinate);
      return _earth.Cartesian(vector);
    }
  }
}
