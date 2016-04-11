namespace AutoPilotSimulation.Geometric
{
  public class Bearing : Angle
  {
    public static readonly Bearing South = new Bearing(Angle.Pi);
    public static readonly Bearing North = new Bearing(Angle.FromRadians(0));
    public static readonly Bearing East = new Bearing(Angle.Pi * 1.5);
    public static readonly Bearing West = new Bearing(Angle.Pi / 2);

    public Bearing(Angle value) : base(Normalize(value))
    {
    }

    static private Angle Normalize(Angle angle)
    {
      return angle%(2*Angle.Pi);
    }

    public static Bearing operator -(Bearing bearing, Angle angle)
    {
      return new Bearing((Angle)bearing - angle);
    }
  }
}
