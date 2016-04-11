using System;

namespace AutoPilotSimulation.Geometric
{
  public class Ball
  {
    public static readonly Ball EarthSurfaceApproximation = new Ball(Length.FromMeters(6366710));
    private readonly Length _radius;

    public Ball(Length radius)
    {
      _radius = radius;
    }

    public Length Circumference
    {
      get { return 2*Math.PI*_radius; }
    }

    public Length Distance(Angle arc)
    {
      return Circumference*(arc/Angle.FullCircle);
    }

    public Length Distance(Coordinate coordinate1, Coordinate coordinate2)
    {
      return Distance(coordinate1.Distance(coordinate2));
    }

    public Angle Arc(Length distance)
    {
      return Angle.FullCircle*(distance/Circumference);
    }
  }
}