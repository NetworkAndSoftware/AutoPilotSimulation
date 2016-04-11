using AutoPilotSimulation.Geometric;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeometricTests
{
  [TestClass]
  public class UnitTest1
  {
    private static readonly Coordinate Ourhouse = new Coordinate(Latitude.FromDegrees(49, 0, 51.55),
                                                                 Longitude.FromDegrees(123, 4, 39.87));

    private static readonly Coordinate PointRobertsMarina = new Coordinate(Latitude.FromDegrees(48, 58, 44.02),
                                                                           Longitude.FromDegrees(123, 4, 4.09));

    private static readonly Coordinate PottsburgCrossing = new Coordinate(Latitude.FromDegrees(30, 15, 45.57),
                                                                          Longitude.FromDegrees(81, 35, 30.09));

    private static readonly Coordinate MammasHuis = new Coordinate(Latitude.FromDegrees(51, 57, 49.22),
                                                                   Longitude.FromDegrees(-4, 30, 2.66));

    [TestMethod]
    public void ShouldCalculateDistance()
    {
      Length distance = Ball.EarthSurfaceApproximation.Distance(Ourhouse, PointRobertsMarina);
      Assert.IsTrue(distance < Length.FromMiles(2.6) && distance > Length.FromMiles(2.4));

      distance = Ball.EarthSurfaceApproximation.Distance(Ourhouse, PottsburgCrossing);

      Assert.IsTrue(distance > Length.FromMiles(2500) && distance < Length.FromMiles(2600));

      distance = Ball.EarthSurfaceApproximation.Distance(Ourhouse, MammasHuis);
      Assert.IsTrue(distance > Length.FromMiles(4800) && distance < Length.FromMiles(4850));
    }

    [TestMethod]
    public void ShouldCalculateInitialCourse()
    {
      Bearing course = Ourhouse.InitialCourse(PointRobertsMarina);

      Assert.IsTrue(course.Degrees > 169 && course.Degrees < 170);

      course = Ourhouse.InitialCourse(MammasHuis);

      Assert.IsTrue(course.Degrees > 28 && course.Degrees < 32);
    }

    [TestMethod]
    public void ShouldVectorRhumbWell()
    {
      var vector = new Vector(Ourhouse, PointRobertsMarina);

      Coordinate newlocation = Ourhouse.Rhumb(vector);

      var distance = Ball.EarthSurfaceApproximation.Distance(newlocation, PointRobertsMarina);

      Assert.IsTrue(distance < Length.FromMeters(.5));

      vector = new Vector(Ourhouse, PottsburgCrossing);

      newlocation = Ourhouse.Rhumb(vector);

      distance = Ball.EarthSurfaceApproximation.Distance(newlocation, PottsburgCrossing);

      Assert.IsFalse(distance < Length.FromMeters(.5));
    }

    [TestMethod]
    public void ShouldVectorGreatCirclesWell()
    {
      var vector = new Vector(Ourhouse, PointRobertsMarina);

      Coordinate newlocation = Ourhouse.GreatCircle(vector);

      var distance = Ball.EarthSurfaceApproximation.Distance(newlocation, PointRobertsMarina);

      Assert.IsTrue(distance < Length.FromMeters(.5));

      vector = new Vector(Ourhouse, MammasHuis);

      newlocation = Ourhouse.GreatCircle(vector);

      distance = Ball.EarthSurfaceApproximation.Distance(newlocation, MammasHuis);

      Assert.IsTrue(distance < Length.FromMeters(.5));
    }
  }
}