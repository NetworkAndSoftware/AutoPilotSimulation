using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace AutoPilotSimulation.UI
{
  /// <summary>
  ///   Represents a visual element that contains a manipulator that can translate along an axis.
  /// </summary>
  public class DraggableSphere : UIElement3D
  {
    /// <summary>
    ///   Identifies the <see cref="Radius" /> dependency property.
    /// </summary>
    public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register(
      "Radius", typeof (double), typeof (DraggableSphere), new UIPropertyMetadata(0.2, UpdateGeometry));

    /// <summary>
    ///   Identifies the <see cref="Normal" /> dependency property.
    /// </summary>
    public static readonly DependencyProperty NormalProperty = DependencyProperty.Register(
      "Normal",
      typeof (Vector3D),
      typeof (DraggableSphere),
      new UIPropertyMetadata(UpdateGeometry));

    /// <summary>
    ///   Identifies the <see cref="Color" /> dependency property.
    /// </summary>
    public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
      "Color", typeof (Color), typeof (DraggableSphere),
      new UIPropertyMetadata((s, e) => ((DraggableSphere) s).ColorChanged()));

    /// <summary>
    ///   Identifies the <see cref="Center" /> dependency property.
    /// </summary>
    public static readonly DependencyProperty CenterProperty = DependencyProperty.Register(
      "Center",
      typeof (Point3D),
      typeof (DraggableSphere),
      new FrameworkPropertyMetadata(
        new Point3D(), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
        (s, e) => ((DraggableSphere) s).PositionChanged(e)));

    /// <summary>
    ///   Identifies the <see cref="TargetTransform" /> dependency property.
    /// </summary>
    public static readonly DependencyProperty TargetTransformProperty =
      DependencyProperty.Register(
        "TargetTransform",
        typeof (Transform3D),
        typeof (DraggableSphere),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    /// <summary>
    ///   Identifies the <see cref="Material" /> dependency property.
    /// </summary>
    public static readonly DependencyProperty MaterialProperty =
      DependencyProperty.Register("Material", typeof (Material), typeof (DraggableSphere), new PropertyMetadata(null));

    /// <summary>
    ///   Identifies the <see cref="BackMaterial" /> dependency property.
    /// </summary>
    public static readonly DependencyProperty BackMaterialProperty =
      DependencyProperty.Register("BackMaterial", typeof (Material), typeof (DraggableSphere),
        new PropertyMetadata(null));

    /// <summary>
    ///   The visibility property.
    /// </summary>
    public static readonly DependencyProperty VisibleProperty = DependencyProperty.Register(
      "Visible",
      typeof (bool),
      typeof (DraggableSphere),
      new UIPropertyMetadata(true, UpdateGeometry));

    /// <summary>
    ///   The move event
    /// </summary>
    public static readonly RoutedEvent MoveEvent = EventManager.RegisterRoutedEvent("DraggableSphereMove", RoutingStrategy.Bubble,
      typeof(MoveEventHandler), typeof(DraggableSphere));


    public class MoveEventArgs : RoutedEventArgs
    {
      public readonly Point3D Point3D;
      public bool Cancel = false;

      public MoveEventArgs(RoutedEvent routedEvent, Point3D point) : base(routedEvent)
      {
        Point3D = point;
      }
    }

    public delegate void MoveEventHandler(object sender, MoveEventArgs e);

    /// <summary>
    ///   Provide CLR accessors for the move event
    /// </summary>
    public event MoveEventHandler Move
    {
      add { AddHandler(MoveEvent, value); }
      remove { RemoveHandler(MoveEvent, value); }
    }

    /// <summary>
    ///   The last point.
    /// </summary>
    private Point3D _lastPoint;

    public DraggableSphere()
    {
      Model = new GeometryModel3D();
      BindingOperations.SetBinding(Model, GeometryModel3D.MaterialProperty, new Binding("Material") {Source = this});
      BindingOperations.SetBinding(Model, GeometryModel3D.BackMaterialProperty,
        new Binding("BackMaterial") {Source = this});
      Visual3DModel = Model;
    }

    /// <summary>
    ///   Gets or sets the diameter of the manipulator arrow.
    /// </summary>
    /// <value> The diameter. </value>
    public double Radius
    {
      get { return (double) GetValue(RadiusProperty); }

      set { SetValue(RadiusProperty, value); }
    }

    /// <summary>
    ///   Gets or sets the normal of the translation.
    /// </summary>
    /// <value> The normal. </value>
    public Vector3D Normal
    {
      get { return (Vector3D) GetValue(NormalProperty); }

      set { SetValue(NormalProperty, value); }
    }

    /// <summary>
    ///   Gets or sets the color of the manipulator.
    /// </summary>
    /// <value> The color. </value>
    public Color Color
    {
      get { return (Color) GetValue(ColorProperty); }

      set { SetValue(ColorProperty, value); }
    }

    /// <summary>
    ///   Gets or sets the material of the manipulator.
    /// </summary>
    public Material Material
    {
      get { return (Material) GetValue(MaterialProperty); }
      set { SetValue(MaterialProperty, value); }
    }

    /// <summary>
    ///   Gets or sets the back material of the manipulator.
    /// </summary>
    public Material BackMaterial
    {
      get { return (Material) GetValue(BackMaterialProperty); }
      set { SetValue(BackMaterialProperty, value); }
    }

    /// <summary>
    ///   Gets or sets the position of the manipulator.
    /// </summary>
    /// <value> The position. </value>
    public Point3D Center
    {
      get { return (Point3D) GetValue(CenterProperty); }

      set { SetValue(CenterProperty, value); }
    }

    /// <summary>
    ///   Gets or sets the target transform.
    /// </summary>
    public Transform3D TargetTransform
    {
      get { return (Transform3D) GetValue(TargetTransformProperty); }

      set { SetValue(TargetTransformProperty, value); }
    }

    /// <summary>
    ///   Gets or sets a value indicating whether this <see cref="MeshElement3D" /> is visible.
    /// </summary>
    /// <value>
    ///   <c>true</c> if the element is visible; otherwise, <c>false</c>.
    /// </value>
    public bool Visible
    {
      get { return (bool) GetValue(VisibleProperty); }

      set { SetValue(VisibleProperty, value); }
    }

    public bool HasCapture
    {
      get { return Mouse.Captured == this; }
    }

    /// <summary>
    ///   Gets or sets the camera.
    /// </summary>
    protected ProjectionCamera Camera { get; set; }

    /// <summary>
    ///   Gets or sets the hit plane normal.
    /// </summary>
    protected Vector3D HitPlaneNormal { get; set; }

    /// <summary>
    ///   Gets or sets the model.
    /// </summary>
    protected GeometryModel3D Model { get; set; }

    /// <summary>
    ///   Gets or sets the parent viewport.
    /// </summary>
    protected Viewport3D ParentViewport { get; set; }

    /// <summary>
    ///   Updates the geometry.
    /// </summary>
    protected void UpdateGeometry()
    {
      var mb = new MeshBuilder(false, false);
      var p0 = new Point3D(0, 0, 0);
      Vector3D d = Normal;
      d.Normalize();

      if (Visible)
        mb.AddSphere(p0, Radius, 36, 36);

      Model.Geometry = mb.ToMesh();
    }

    /// <summary>
    ///   Invoked when an unhandled <see cref="E:System.Windows.Input.Mouse.MouseDown" /> attached event reaches an element in
    ///   its route that is derived from this class. Implement this method to add class handling for this event.
    /// </summary>
    /// <param name="e">
    ///   The <see cref="T:System.Windows.Input.MouseButtonEventArgs" /> that contains the event data. This event data reports
    ///   details about the mouse button that was pressed and the handled state.
    /// </param>
    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
      base.OnMouseDown(e);
      ParentViewport = Visual3DHelper.GetViewport3D(this);
      Camera = ParentViewport.Camera as ProjectionCamera;
      ProjectionCamera projectionCamera = Camera;
      if (projectionCamera != null)
      {
        HitPlaneNormal = projectionCamera.LookDirection;
      }
      CaptureMouse();
      Vector3D direction = ToWorld(Normal);

      Vector3D up = Vector3D.CrossProduct(Camera.LookDirection, direction);
      Point3D hitPlaneOrigin = ToWorld(Center);
      HitPlaneNormal = Vector3D.CrossProduct(up, direction);
      Point p = e.GetPosition(ParentViewport);

      Point3D? np = GetHitPlanePoint(p, hitPlaneOrigin, Normal);
      if (np == null)
      {
        return;
      }

      Point3D lp = ToLocal(np.Value);

      _lastPoint = lp;
      CaptureMouse();
    }

    /// <summary>
    ///   Invoked when an unhandled <see cref="E:System.Windows.Input.Mouse.MouseMove" /> attached event reaches an element in
    ///   its route that is derived from this class. Implement this method to add class handling for this event.
    /// </summary>
    /// <param name="e">
    ///   The <see cref="T:System.Windows.Input.MouseEventArgs" /> that contains the event data.
    /// </param>
    protected override void OnMouseMove(MouseEventArgs e)
    {
      base.OnMouseMove(e);
      if (!IsMouseCaptured) 
        return;

      var hitPlaneOrigin = ToWorld(Center);
      var p = e.GetPosition(ParentViewport);

      var nearestPoint = GetHitPlanePoint(p, hitPlaneOrigin, Normal);
      if (nearestPoint == null)
      {
        return;
      }

      var point = ToLocal(nearestPoint.Value);
      var delta = point - _lastPoint;

      if (TargetTransform != null)
      {
        var translateTransform = new TranslateTransform3D(delta);
        TargetTransform = Transform3DHelper.CombineTransform(translateTransform, TargetTransform);
      }
      else
      {
        var newcenter = Center + delta;

        var moveEventArgs = new MoveEventArgs(MoveEvent, newcenter);
        RaiseEvent(moveEventArgs);

        if (moveEventArgs.Cancel)
          return;

        Center = newcenter;
      }

      _lastPoint = point;
    }

    /// <summary>
    ///   Binds this manipulator to a given Visual3D.
    /// </summary>
    /// <param name="source">
    ///   Source Visual3D which receives the manipulator transforms.
    /// </param>
    public virtual void Bind(ModelVisual3D source)
    {
      BindingOperations.SetBinding(this, TargetTransformProperty, new Binding("Transform") {Source = source});
      BindingOperations.SetBinding(this, TransformProperty, new Binding("Transform") {Source = source});
    }

    /// <summary>
    ///   Releases the binding of this manipulator.
    /// </summary>
    public virtual void UnBind()
    {
      BindingOperations.ClearBinding(this, TargetTransformProperty);
      BindingOperations.ClearBinding(this, TransformProperty);
    }

    /// <summary>
    ///   Called when a property related to the geometry is changed.
    /// </summary>
    /// <param name="d">
    ///   The sender.
    /// </param>
    /// <param name="e">
    ///   The <see cref="System.Windows.DependencyPropertyChangedEventArgs" /> instance containing the event data.
    /// </param>
    protected static void UpdateGeometry(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ((DraggableSphere) d).UpdateGeometry();
    }

    /// <summary>
    ///   Projects the point on the hit plane.
    /// </summary>
    /// <param name="p">
    ///   The p.
    /// </param>
    /// <param name="hitPlaneOrigin">
    ///   The hit Plane Origin.
    /// </param>
    /// <param name="hitPlaneNormal">
    ///   The hit plane normal (world coordinate system).
    /// </param>
    /// <returns>
    ///   The point in world coordinates.
    /// </returns>
    protected virtual Point3D? GetHitPlanePoint(Point p, Point3D hitPlaneOrigin, Vector3D hitPlaneNormal)
    {
      return Viewport3DHelper.UnProject(ParentViewport, p, hitPlaneOrigin, hitPlaneNormal);
    }

    /// <summary>
    ///   Invoked when an unhandled <see cref="E:System.Windows.Input.Mouse.MouseUp" /> routed event reaches an element in its
    ///   route that is derived from this class. Implement this method to add class handling for this event.
    /// </summary>
    /// <param name="e">
    ///   The <see cref="T:System.Windows.Input.MouseButtonEventArgs" /> that contains the event data. The event data reports
    ///   that the mouse button was released.
    /// </param>
    protected override void OnMouseUp(MouseButtonEventArgs e)
    {
      base.OnMouseUp(e);
      ReleaseMouseCapture();
    }

    /// <summary>
    ///   Handles changes in the Center property.
    /// </summary>
    /// <param name="e">
    ///   The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.
    /// </param>
    protected virtual void PositionChanged(DependencyPropertyChangedEventArgs e)
    {
      Transform = new TranslateTransform3D(Center.X, Center.Y, Center.Z);
    }

    /// <summary>
    ///   Transforms from world to local coordinates.
    /// </summary>
    /// <param name="worldPoint">
    ///   The point (world coordinates).
    /// </param>
    /// <returns>
    ///   Transformed vector (local coordinates).
    /// </returns>
    protected Point3D ToLocal(Point3D worldPoint)
    {
      Matrix3D mat = Visual3DHelper.GetTransform(this);
      mat.Invert();
      var t = new MatrixTransform3D(mat);
      return t.Transform(worldPoint);
    }

    /// <summary>
    ///   Transforms from local to world coordinates.
    /// </summary>
    /// <param name="point">
    ///   The point (local coordinates).
    /// </param>
    /// <returns>
    ///   Transformed point (world coordinates).
    /// </returns>
    protected Point3D ToWorld(Point3D point)
    {
      Matrix3D mat = Visual3DHelper.GetTransform(this);
      var t = new MatrixTransform3D(mat);
      return t.Transform(point);
    }

    /// <summary>
    ///   Transforms from local to world coordinates.
    /// </summary>
    /// <param name="vector">
    ///   The vector (local coordinates).
    /// </param>
    /// <returns>
    ///   Transformed vector (world coordinates).
    /// </returns>
    protected Vector3D ToWorld(Vector3D vector)
    {
      Matrix3D mat = Visual3DHelper.GetTransform(this);
      var t = new MatrixTransform3D(mat);
      return t.Transform(vector);
    }

    /// <summary>
    ///   Handles changes in the Color property (this will override the materials).
    /// </summary>
    private void ColorChanged()
    {
      Material = MaterialHelper.CreateMaterial(Color);
      BackMaterial = Material;
    }
  }
}