namespace AutoPilotSimulation.Geometric
{
  public class Longitude : Angle
  {
    public Longitude(Angle angle)
      : base(angle)
    {}

    public static new Longitude FromDegrees(double degrees)
    {
      return new Longitude(Angle.FromDegrees(degrees));
    }

    public new static Longitude FromDegrees(double degrees, double minutes, double seconds)
    {
      return new Longitude(Angle.FromDegrees(degrees, minutes, seconds));
    }

    public static new Longitude FromRadians(double radians)
    {
      return new Longitude(Angle.FromRadians(radians));
    }
  }
}
