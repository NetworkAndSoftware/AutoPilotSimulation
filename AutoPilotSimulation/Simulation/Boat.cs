using AutoPilotSimulation.Geometric;

namespace AutoPilotSimulation.Simulation
{
  internal class Boat : IFleeting
  {
    public Bearing Course;
    public Coordinate Location;
    public Angle Rudderposition;
    public Velocity Speed;

    /// <summary>
    ///   How far the boat travels before it turns by the same amount as the rudder is turned
    /// </summary>
    public Length TurningDistance = Length.FromMeters(5);

    public Boat(Coordinate location, Velocity speed, Bearing course, Angle rudderposition)
    {
      Location = location;
      Speed = speed;
      Course = course;
      Rudderposition = rudderposition;
    }

    public void Update(Time elapsedtime)
    {
      UpdateLocation(elapsedtime);
      UpdateCourse(elapsedtime);
    }

    private void UpdateLocation(Time elapsedtime)
    {
      Length distancetravelled = Speed/elapsedtime;
      var v = new Vector(Course, Ball.EarthSurfaceApproximation.Arc(distancetravelled));
      Location = Location.GreatCircle(v);
    }

    private void UpdateCourse(Time elapsedtime)
    {
      Course -= Rudderposition*(elapsedtime/(TurningDistance/Speed));
    }
  }
}