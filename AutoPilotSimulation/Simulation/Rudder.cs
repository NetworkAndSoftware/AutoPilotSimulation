using AutoPilotSimulation.AutoPilot;
using Geometry;

namespace AutoPilotSimulation.Simulation
{
  public class Rudder : IRudder
  {
    public readonly Angle MaxPosition;

    private readonly Angle _minimumdeflection;
    private readonly AngularVelocity _velocity;
    private Angle _targetposition;

    public Angle Position { get; private set; }

    public Rudder(Angle maxposition, AngularVelocity velocity, Angle minimumdeflection)
    {
      Position = Angle.Zero;
      MaxPosition = maxposition;
      _velocity = velocity;
      _minimumdeflection = minimumdeflection;
    }

    public void MoveTo(Angle angle)
    {
      if (angle.Abs() < MaxPosition)
        _targetposition = angle;
      else
        _targetposition = angle.Sign()*MaxPosition;
    }

    public void Update(Time elapsedtime)
    {
      var deviation = _targetposition - Position;

      if (deviation.Abs() < _minimumdeflection) 
        return;

      var correction = deviation.Sign()*(_velocity*elapsedtime);

      if (deviation.Abs() < correction.Abs())
        correction = deviation;

      Position += correction;
    }
  }
}