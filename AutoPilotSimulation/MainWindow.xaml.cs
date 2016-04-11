using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using AutoPilotSimulation.Geometric;
using AutoPilotSimulation.Simulation;

namespace AutoPilotSimulation
{


  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    private readonly DispatcherTimer _timer;
    private readonly World world;
    
    public MainWindow()
    {
      InitializeComponent();

      world = new World();

      _timer = new DispatcherTimer();
      _timer.Tick += Update;
      _timer.Interval = new TimeSpan(0, 0, 1);
      _timer.Start();
    }

    private void Update(object sender, EventArgs e)
    {
      world.Update();
    }
  }
}
