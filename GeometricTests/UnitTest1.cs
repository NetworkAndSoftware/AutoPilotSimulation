using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoPilotSimulation.Geometric;

namespace GeometricTests
{
  [TestClass]
  public class UnitTest1
  {
    static readonly Coordinate Ourhouse = new Coordinate(Latitude.FromDegrees(49, 0, 51.55), Longitude.FromDegrees(123,4,39.87));
    static readonly Coordinate PointRobertsMarina = new Coordinate(Latitude.FromDegrees(48,58,44.02), Longitude.FromDegrees(123,4,4.09));
    static readonly Coordinate PottsburgCrossing = new Coordinate(Latitude.FromDegrees(30,15,45.57), Longitude.FromDegrees(81,35,30.09));
    static readonly Coordinate MammasHuis = new Coordinate(Latitude.FromDegrees(51,57,49.22), Longitude.FromDegrees(-4,30,2.66));
    
  [TestMethod]
    public void ShouldCalculateDistance()
    {
      var miles = Ball.EarthSurfaceApproximation.Distance(Ourhouse, PointRobertsMarina).Miles;
      Assert.IsTrue(miles < 2.6 && miles > 2.4);

      miles = Ball.EarthSurfaceApproximation.Distance(Ourhouse, PottsburgCrossing).Miles;

      Assert.IsTrue(miles > 2500 && miles < 2600);

      miles = Ball.EarthSurfaceApproximation.Distance(Ourhouse, MammasHuis).Miles;
      Assert.IsTrue(miles > 4800 && miles < 4850);
    }

    [TestMethod]
    public void ShouldCalculateInitialCourse()
    {
      var course = Ourhouse.InitialCourse(PointRobertsMarina);

      Assert.IsTrue(course.Degrees > 169 && course.Degrees < 170);

      course = Ourhouse.InitialCourse(MammasHuis);

      Assert.IsTrue(course.Degrees > 28 && course.Degrees < 32);
    }

    [TestMethod]
    public void ShouldVectorRhumbWell()
    { var vector = new Vector(Ourhouse, PointRobertsMarina);

      var newlocation = Ourhouse.Rhumb(vector);

      var meters = Ball.EarthSurfaceApproximation.Distance(newlocation, PointRobertsMarina).Meters;

      Assert.IsTrue(meters < .5);

      vector = new Vector(Ourhouse, PottsburgCrossing);

      newlocation = Ourhouse.Rhumb(vector);

      meters = Ball.EarthSurfaceApproximation.Distance(newlocation, PottsburgCrossing).Meters;

      Assert.IsFalse(meters < .5);
    }

    [TestMethod]
    public void ShouldVectorGreatCirclesWell()
    {
      var vector = new Vector(Ourhouse, PointRobertsMarina);

      var newlocation = Ourhouse.GreatCircle(vector);

      var meters = Ball.EarthSurfaceApproximation.Distance(newlocation, PointRobertsMarina).Meters;

      Assert.IsTrue(meters < .5);

      vector = new Vector(Ourhouse, MammasHuis);

      newlocation = Ourhouse.GreatCircle(vector);

      meters = Ball.EarthSurfaceApproximation.Distance(newlocation, MammasHuis).Meters;

      Assert.IsTrue(meters < .5);
    }
  }
}
