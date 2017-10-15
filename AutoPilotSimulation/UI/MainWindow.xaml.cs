using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using AutoPilotSimulation.AutoPilot;
using AutoPilotSimulation.Simulation;
using Geometry;
using HelixToolkit.Wpf;

namespace AutoPilotSimulation.UI
{
  /// <summary>
  ///   Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow
  {
    public static RoutedCommand SwitchCameraCommand = new RoutedCommand();
    private readonly Camera _camera;
    private readonly Projections _projections;
    private readonly World _world;
    private ProjectionCamera _oldProjectionCamera;

    public MainWindow()
    {
      InitializeComponent();

      FileModelVisual3D.Source = @"Models\boat.obj";

      _world = new World();
      _projections = new Projections(Ball.EarthSurfaceApproximation, _world.Mesh.Size, Grid.Length, _world.Mesh.Center);

      Grid.MinorDistance = _projections.ToWorld(Length.FromMeters(10));
      Waypoint.Radius = _projections.ToWorld(Length.FromMeters(1));

      CameraViewsCombobox.DataContext = this;

      _camera = new Camera(_world, _projections);
      _world._fleetingthings.Add(_camera);

      InitializeTimer(Update, 100);
    }

    public Camera.ViewMode CameraViewsSelectedValue
    {
      get { return _camera.Mode; }
      set { _camera.Mode = value; }
    }

    public IEnumerable<KeyValuePair<Camera.ViewMode, string>> CameraViewsItemsSource
    {
      get
      {
        return from Enum e in Enum.GetValues(typeof (Camera.ViewMode))
          select
            new KeyValuePair<Camera.ViewMode, string>((Camera.ViewMode) e,
              (e.GetType().GetField(e.ToString())
                .GetCustomAttributes(typeof (DescriptionAttribute), false)
                .First() as DescriptionAttribute)
                .Description);
      }
    }

    private static void InitializeTimer(EventHandler handler, int rate)
    {
      var timer = new DispatcherTimer();
      timer.Tick += handler;
      timer.Interval = new TimeSpan(0, 0, 0, 0, 1000/rate);
      timer.Start();
    }

    private void Update(object sender, EventArgs e)
    {
      _world.Age();

      UpdateBoatVisuals();

      if (!IsUserMovingThings())
        UpdateCameraVisuals(_camera);

      InfoBox.Update(_world);
    }

    private bool IsUserMovingThings()
    {
      return Waypoint.HasCapture;
    }

    private void UpdateCameraVisuals(Camera camera)
    {
      switch (camera.Mode)
      {
        case Camera.ViewMode.Follow1:
        case Camera.ViewMode.Follow2:
        {
          EnableCameraController(ViewPort.CameraController, false);
          camera.UpdateProjectionCamera(ViewPort.Camera);
          ViewPort.Camera.UpDirection = new Vector3D(0, 0, 1);
        }
          break;
        case Camera.ViewMode.Default:
          EnableCameraController(ViewPort.CameraController, true);
          break;
      }
    }

    private void UpdateBoatVisuals()
    {
      foreach (Boat boat in _world._fleetingthings.Where(thing => thing is Boat))
        UpdateBoatVisual(boat);
    }

    private void UpdateBoatVisual(Boat boat)
    {
      BoatBearing.Angle = boat.Bearing.Degrees;
      CartesianVector location = _projections.ToCartesian(boat.Location);
      BoatPosition.OffsetX = _projections.ToWorld(location.X);
      BoatPosition.OffsetY = _projections.ToWorld(location.Y);

      if (null == boat.Autopilot.WayPoint)
        Waypoint.Visible = false;
      else
      {
        Waypoint.Visible = true;
        CartesianVector cartesianVector = _projections.ToCartesian(boat.Autopilot.WayPoint.Location);

        if (Waypoint.HasCapture)
          boat.Autopilot.WayPoint = new Waypoint(_projections.ToModel(Waypoint.Center.X, Waypoint.Center.Y));
        else
          Waypoint.Center = new Point3D(_projections.ToWorld(cartesianVector.X), _projections.ToWorld(cartesianVector.Y),
            Waypoint.Center.Z);
      }
    }



    private static void EnableCameraController(CameraController controller, bool enabled)
    {
      controller.Enabled = enabled;
      controller.IsPanEnabled = enabled;
      //controller.IsChangeFieldOfViewEnabled = enabled;
      controller.IsMoveEnabled = enabled;
    }

    #region Event Handlers

    private void ButtonExit_OnClick(object sender, RoutedEventArgs e)
    {
      Close();
    }
    private void ViewPort_CameraChanged(object sender, RoutedEventArgs e)
    {
      lock (ViewPort.Camera)
      {
        if (!_camera.IsUpdatingProjectionCamera)
          if (ViewPort.Camera.Position.Z <= 0)
          {
            ViewPort.Camera = _oldProjectionCamera;
            return;
          }
          else
            _camera.MoveToProjectionCamera(ViewPort.Camera);

        _oldProjectionCamera = ViewPort.Camera;
      }
    }

    private void Waypoint_OnMove(object sender, DraggableSphere.MoveEventArgs e)
    {
      if (Math.Abs(e.Point3D.X) > Grid.Length/2 || Math.Abs(e.Point3D.Y) > Grid.Length/2)
        e.Cancel = true;
    }

    private void ToggleButton_OnChanged(object sender, RoutedEventArgs e)
    {
      if (TogglePause.IsChecked != null)
        _world.Paused = TogglePause.IsChecked.Value;
    }

    private void OnPause(object sender, ExecutedRoutedEventArgs e)
    {
      TogglePause.IsChecked = !TogglePause.IsChecked;
    }

    private void SwitchCameraCommand_OnExecuted(object sender, ExecutedRoutedEventArgs e)
    {
      CameraViewsSelectedValue = (Camera.ViewMode) Enum.Parse(typeof (Camera.ViewMode), (string) e.Parameter);
    }

    private void OnHelp(object sender, ExecutedRoutedEventArgs e)
    {
      var c = TogglePause.IsChecked;
      TogglePause.IsChecked = true;
      new HelpWindow() {Owner = this}.ShowDialog();
      TogglePause.IsChecked = c;
    }

    #endregion
  }
}