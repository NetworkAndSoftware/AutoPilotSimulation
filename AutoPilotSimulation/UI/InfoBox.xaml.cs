using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using AutoPilotSimulation.Annotations;
using AutoPilotSimulation.AutoPilot;
using AutoPilotSimulation.Simulation;
using Geometry;

namespace AutoPilotSimulation.UI
{
  /// <summary>
  ///   Interaction logic for InfoBox.xaml
  /// </summary>
  public partial class InfoBox : INotifyPropertyChanged
  {
    public static readonly DependencyProperty WaypointBearingProperty = DependencyProperty.Register("WaypointBearing",
      typeof (string), typeof (InfoBox), new PropertyMetadata(default(string)));

    public static readonly DependencyProperty EstimatedCourseProperty = DependencyProperty.Register("EstimatedCourse",
      typeof (string), typeof (InfoBox), new PropertyMetadata(default(string)));

    public static readonly DependencyProperty WaypointDistanceProperty = DependencyProperty.Register(
      "WaypointDistance", typeof (string), typeof (InfoBox), new PropertyMetadata(default(string)));

    public static readonly DependencyProperty CurrentDirectionProperty = DependencyProperty.Register(
      "CurrentDirection", typeof (string), typeof (InfoBox), new PropertyMetadata(default(string)));

    public static readonly DependencyProperty CurrentSpeedProperty = DependencyProperty.Register("CurrentSpeed",
      typeof (string), typeof (InfoBox), new PropertyMetadata(default(string)));

    public static readonly DependencyProperty BoatBearingProperty = DependencyProperty.Register("BoatBearing",
      typeof (string), typeof (InfoBox), new PropertyMetadata(default(string)));

    public static readonly DependencyProperty BoatSpeedProperty = DependencyProperty.Register("BoatSpeed",
      typeof (string), typeof (InfoBox), new PropertyMetadata(default(string)));

    public static readonly DependencyProperty BoatRudderProperty = DependencyProperty.Register("BoatRudder",
      typeof (string), typeof (InfoBox), new PropertyMetadata(default(string)));

    public InfoBox()
    {
      InitializeComponent();
    }

    public string WaypointBearing
    {
      get { return (string) GetValue(WaypointBearingProperty); }
      set { SetValue(WaypointBearingProperty, value); }
    }

    public string EstimatedCourse
    {
      get { return (string) GetValue(EstimatedCourseProperty); }
      set { SetValue(EstimatedCourseProperty, value); }
    }

    public string WaypointDistance
    {
      get { return (string) GetValue(WaypointDistanceProperty); }
      set { SetValue(WaypointDistanceProperty, value); }
    }

    public string CurrentDirection
    {
      get { return (string) GetValue(CurrentDirectionProperty); }
      set { SetValue(CurrentDirectionProperty, value); }
    }

    public string CurrentSpeed
    {
      get { return (string) GetValue(CurrentSpeedProperty); }
      set { SetValue(CurrentSpeedProperty, value); }
    }

    public string BoatBearing
    {
      get { return (string) GetValue(BoatBearingProperty); }
      set { SetValue(BoatBearingProperty, value); }
    }

    public string BoatSpeed
    {
      get { return (string) GetValue(BoatSpeedProperty); }
      set { SetValue(BoatSpeedProperty, value); }
    }

    public string BoatRudder
    {
      get { return (string) GetValue(BoatRudderProperty); }
      set { SetValue(BoatRudderProperty, value); }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChangedEventHandler handler = PropertyChanged;
      if (handler != null)
        handler(this, new PropertyChangedEventArgs(propertyName));
    }

    public void Update(World world)
    {
      var current = world._fleetingthings.First(thing => thing is Current) as Current;
      var boat = world._fleetingthings.First(thing => thing is Boat) as Boat;
      Waypoint waypoint = boat.Autopilot.WayPoint;
      const string none = "(none)";

      CurrentDirection = null == current ? none : FormatBearing(current.Direction);
      CurrentSpeed = null == current ? none : FormatSpeed(current.Speed);

      BoatBearing = FormatBearing(boat.Bearing);
      BoatSpeed = FormatSpeed(boat.Speed);
      BoatRudder = FormatRudder(boat.Rudder.Position);

      WaypointBearing = FormatBearing(boat.Location.InitialCourse(waypoint.Location));
      WaypointDistance = FormatDistance(boat.Location.Distance(waypoint.Location));
      EstimatedCourse = boat.Autopilot.CurrentCourseEstimate == null
        ? none
        : FormatBearing(boat.Autopilot.CurrentCourseEstimate);
    }

    private static string FormatBearing(Angle bearing)
    {
      return bearing.Degrees.ToString("###0°");
    }

    private static string FormatSpeed(Velocity speed)
    {
      return speed.Knots().ToString("#### Kts");
    }

    private static string FormatDistance(Angle distance)
    {
      return Ball.EarthSurfaceApproximation.Distance(distance).Meters().ToString("#### m");
    }

    private static string FormatRudder(Angle angle)
    {
      var sb = new StringBuilder();
      var abs = angle.Abs();

      sb.Append(FormatBearing(abs));

      if (abs >= Angle.FromDegrees(.5))
      {
        sb.Append(" to the ");
        sb.Append(angle.Sign() > 0 ? "right" : "left");
      }

      return sb.ToString();
    }
  }
}