   M S B A M L   `   `   `  ����[  WHelixToolkit.Wpf, Version=2013.1.29.1, Culture=neutral, PublicKeyToken=52aa3500039caf0dM8clr-namespace:HelixToolkit.Wpf;assembly=HelixToolkit.WpfHelixToolkit.Wpf  5   	   N JAutoPilotSimulation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:AutoPilotSimulationAutoPilotSimulation 5   	   M+clr-namespace:AutoPilotSimulation.AutoPilotAutoPilotSimulation.AutoPilot 5   	   ?$clr-namespace:AutoPilotSimulation.UIAutoPilotSimulation.UI 5	   	   '   !AutoPilotSimulation.UI.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Fh8clr-namespace:HelixToolkit.Wpf;assembly=HelixToolkit.Wpf     5   	   AautoPilotSimulation!clr-namespace:AutoPilotSimulation     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   A	autoPilot+clr-namespace:AutoPilotSimulation.AutoPilot     5   	   3ui$clr-namespace:AutoPilotSimulation.UI     5	   	     � Title$  AutoPilot Simulation��5   	   $	��768��6&   $
��1024��63    � WindowState$ Normal=�6@    J� CommandBindings 5      )  #System.Windows.Input.CommandBinding  5   
   -   5           Command$ Pause��5      6)     5   
   -   5        $
 Help��5      6<     5   
   -   5        '  !AutoPilotSimulation.UI.MainWindow   SwitchCameraCommand# Z 5      6P    J� InputBindings 5      �� 5   
   
 �� Key$ Space��6    �� Command$ Pause��6!   63   �� 5   
   $ D1��6    �� 	Modifiers$	 Altf�6   # Z 6.   	 �� CommandParameter$	 0��6e   6|   �� 5   
   $ D2��6   $	 Altf�6   # Z 6.   $	 1��6e   6|   �� 5   
   $ D3��6   $	 Altf�6   # Z 6.   $	 2��6e   6|   �� 5   
   $ H��6   $
 Help��6   6.   �� 5   
   $ F1��6   $
 Help��6   6/   .��5      � .+�5   
   ]� .+�5      �� $��Bottom=�6   .@�5      ]� ��.  ��6   �� 
 �� Path$
 Parent.ActualWidth� �� RelativeSource �� *Self+.+�5      �� $��Camera:��6   $��Center=�6.   �� 5       -   5         ��Name$ CameraViewsCombobox��5        �� SelectedIndex$ 1��6<    �� DisplayMemberPath$ Value��6�    �� SelectedValuePath$	 Key��6�   $��Center=�6�    �� SelectedValue 6N   �� $
 CameraViewsSelectedValue���6�   �� $
 CameraViewsItemsSource�6�   �� 5!      -   5         ��Name$ TogglePause��5!      $��Pause��65   $��RightToLeft=�6E   $��Center=�6a   $��8,0,0,0q�6|   6�   �� 5"      -   5         ��Name$ 
ButtonExit��5"      $
��Exit��61   $��75��6@    �� IsCancel . $��Right=�6v   $��Right=�6�   $��0,0,6,0q�6�   5#      5%      &    HelixToolkit.Wpf.HelixViewport3D  5'      -   5          Name$ ViewPort��5'          ZoomAroundMouseDownPoint . $	��Top=�6R      CameraRotationMode$ 	Turntable=�6g      ShowCameraTarget . ��5(      �� 5)       �� EndPoint$ 0.5,1&�6*    �� 
StartPoint$ 0.5,0&�6;   .��5*      �  � Color$ 	#FF75A7FF��6'    � Offset$ 0Q�69   � 5+      $ 	#FF02040A��6'   $ 1Q�69   5,         Children. 50         HelixToolkit.Wpf.SunLight  (   "HelixToolkit.Wpf.GridLinesVisual3D  53      -   5        Grid��   Width$	 100Q�53   4      Length$	 100Q�6@      MinorDistance$ 1Q�6M       	Thickness$
  0.01Q�6_   !   Center$! 0,0,01�6p   $   HelixToolkit.Wpf.MeshElement3D"   Fill
" �����6   "   HelixToolkit.Wpf.BoxVisual3D  54      #   Width$	# 100Q�6    $   Length$	$ 100Q�6,   %   Height$% 2Q�6�   &   Center$& 
0,0,-1.0011�6�   '   BackMaterial' 69   (   "HelixToolkit.Wpf.MaterialExtension  *Wheat+(   SpecularIntensity$( .5Q�)   Opacity$) 1Q�*   Material* 6|     *	#ff1a2d21+$( .7Q�$) .9Q�56      g� 59      + g� 	Transform+ 5:      Y� 5;      .�5<      �� , �� Rotation, 5=   "   �� 5>   &   - �� Axis$- 1 0 05�6:   . �� Angle$. 90Q�6G   6T   5@      �� 5A      / �� ScaleX$
/ .002Q�6/   0 �� ScaleY$
0 .002Q�6=   1 �� ScaleZ$
1 .002Q�6K   6[   �� 5B      , 5C   "   �� 5D   &   $- 0 0 15�6:   $. 90Q�6G   6T   5F      �� 5G      , 5H   "   �� 5I   &   -	   5        BoatBearing��$- 0 0 -15�5I   O   $. 15Q�6]   6j   5K      S� 5L      -
   5        BoatPosition��5M      5N      .2�5Q      (	   "HelixToolkit.Wpf.FileModelVisual3D	  -   5        FileModelVisual3D��2 	  DefaultMaterial2 5Q   E     *
SeaShell+6o   5R      ,
  &AutoPilotSimulation.UI.DraggableSphere
  5T      ,  &AutoPilotSimulation.UI.DraggableSphere3   Move$3 Waypoint_OnMove��6e   -   5        Waypoint��4 
  Normal$4 0 0 15�5T   V   5 
  Material5 67     *Orange+6~   5U      5W      $  AutoPilotSimulation.UI.InfoBox  5X   
   -   5        6  Name$6 InfoBox��5X      7 J� Opacity$	7 0.6Q�6&   
�������$��Right=�6G   $	��Top=�6c   $��10q�6{   6�   5Y      5[      