   M S B A M L   `   `   `  ����P  LAutomater, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9d9bf72c0d89ee0e-clr-namespace:CustomControlCustomControl  5   	   !clr-namespace:WpfApp1WpfApp1  5   	       WpfApp1.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   0controlsclr-namespace:CustomControl     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:WpfApp1     5   	   g cdevDept.Eyeshot.Control.Wpf.v9, Version=9.0.473.0, Culture=neutral, PublicKeyToken=5385d83d2fb4d7d9=ddes2http://schemas.devdept.com/winfx/2008/xaml/control  5	   	   ` \devDept.Graphics.Wpf.v9, Version=9.0.473.0, Culture=neutral, PublicKeyToken=5385d83d2fb4d7d9>ddgr3http://schemas.devdept.com/winfx/2008/xaml/graphics  6I   -   5          � WindowStyle$
  None=�5   	    � Title$ 
MainWindow��5   	    � AllowsTransparency .  � 	Resources 5         myHeaderStyle �� 
TargetType=  7System.Windows.Controls.Primitives.DataGridColumnHeader �� Property �� Value  DataGridCellStyle)  #System.Windows.Controls.DataGridRow  TextBlockStyle  TextBoxStyle��%?  &	        &	 �     &	 T    &	     �� 5   
   "  6&   .�5      ��  � ��
 ���� 6+   �� 5       � ��
 ���� 6,   �� 5       � y�$ Hidden=�6+   5      �� 5   
   "  6*   .�5      ��  � ��
 ���� 6+   �� 5       � ��
 �����6+   �� 5       � ��
 ���� 65   �� 5       � ��
 �����65   5      �� 5   
   " ��6'   .�5      ��  � ��
 ���� 6+   �� 5       � ��$
 Bold"�6+   �� 5       � ��
 �����6+   �� 5        � ��$ 9(�6)   5"      �� 5$   
   " ��6%   .�5%      ��  � ��
 ���� 6+   �� 5&       � ��
 ���� 6,   �� 5'       � ��
 �����6+   �� 5(       � ��$ 9(�6)   �� 5)       � ��$ Normal"�6+   �� 5*       � ��$ 0,0,0,0q�6'   5+      5/       J� Effect 50      3  -System.Windows.Media.Effects.DropShadowEffect  51   
   .��54      � -   5        $��
WindowGrid��54      ��55   
   �� 56       �� EndPoint$ 0.5,1&�6"   	 � MappingMode$	 RelativeToBoundingBox=�63   
 �� 
StartPoint$
 0.5,0&�6W   .��57      �  � Color$ 	#FF0E0E0E��6    � Offset$ 1Q�61   � 58      $ 	#FF4B4B4B��6   $ 0.373Q�61   59       � RowDefinitions 5;   
   �� 5<      $��* �6   �� 5=      $��11 �6   �� 5>      $��* �6   �� 5?      $��11 �6   �� 5@      $	��142 �6   �� 5A      $��11 �6   �� 5B      $	��140 �6   .+�5D   
   �� $��55(�6   $��Center=�6"   $��Center=�6?   $
��Wrap=�6Z   �� 6o   $	�Top��6s   6�   �� 5E   
   $��1��6   $��5��6$   $��Stretch=�6/   
���   �6M   �� 5F   
   $��3��6   $��5��6$   $��Stretch=�6/   
���   �6M   �� 5G   
   $��5��6   $��5��6$   $��Stretch=�6/   
���   �6M   � 5H   
   $��0��6   .+�5I      $  devDept.Eyeshot.ViewportLayout  -   5          Name$ viewPortLayout1��5I   "   $��Stretch=�6;   $��0,0,0,0q�6Z   $��Stretch=�6k      	HideSmall .  K� ZIndex$ 1��6"     ShowFps .    ProgressBar 5J      !  devDept.Eyeshot.ProgressBar  5K         Lighting .  6'      	Viewports 5O        devDept.Eyeshot.Viewport     DisplayMode$ Rendered=�6$      InitialView$ Other=�6;      
Background 5P      )  #devDept.Graphics.BackgroundSettings  5Q         BottomColor �	#FF434752   	StyleMode$ LinearGradient=�6N      TopColor �	#FF2223296i      CoordinateSystemIcon 5S      *  $devDept.Eyeshot.CoordinateSystemIcon  5T      *	  $devDept.Eyeshot.CoordinateSystemBase 	  Lighting .  $y�Hidden=�6I      Camera 5V      
  devDept.Eyeshot.Camera
  5W       
  Distance$	 600Q�6*    
  ProjectionMode$ Perspective=�69     
  Rotation\ XdevDept.Geometry.v9, Version=9.0.473.0, Culture=neutral, PublicKeyToken=5385d83d2fb4d7d95  /devDept.Geometry.Converters.QuaternionConverter$Q  K0.0868240888334652, 0.150383733180435, 0.492403876506104, 0.852868531952443 6V   ! 
  Target2  ,devDept.Geometry.Converters.Point3DConverter$! 0, 0, 50 6�   " 
  
ZoomFactor$" 2Q�6�   #   Grids# 5Z        devDept.Eyeshot.Grid  $   
ColorAxisY$ �	#7F202020%   
ColorAxisX% �	#7F202020&   Lighting& .  '   MajorLineColor' �	#7F5A5A5A(   MajorLinesEvery$( 5��6�   )   Step$) 5Q�6�   *   Visible* .  6�   +   OriginSymbol+ 5\      "  devDept.Eyeshot.OriginSymbol  5]       .  $y�Hidden=�6A   ,   Rotate, 5_      $  devDept.Eyeshot.RotateSettings  5`      *  $devDept.Eyeshot.MovementSettingsBase-   Enabled- . 62   .   ToolBar. 5b        devDept.Eyeshot.ToolBar  5c      /   Buttons/ 5e   &   '  !devDept.Eyeshot.HomeToolBarButton  2  ,devDept.Eyeshot.MagnifyingGlassToolBarButton  5f   &   -  'devDept.Eyeshot.ZoomWindowToolBarButton  5g   &   '  !devDept.Eyeshot.ZoomToolBarButton  5h   &   &   devDept.Eyeshot.PanToolBarButton  5i   &   )  #devDept.Eyeshot.RotateToolBarButton  5j   &   *  $devDept.Eyeshot.ZoomFitToolBarButton  5k   &   5m      0   ViewCubeIcon0 5o      "  devDept.Eyeshot.ViewCubeIcon  5p      1   Lighting1 .  60   5r      5t      �� 5u      -   5        2 ��Name$2 ShadeRectangle1��5u      3 �� Stretch$
3 Fill=�61   $��1��6@   $��
10,10,10,0q�6M   $	 100��6a   ���	#FFB6B6B6$��5��6�    5v      -  'System.Windows.Media.Effects.BlurEffect  5w      4   Radius$4 10Q�6!   5y      �� 5z      -   5        5 ��Name$5 	ModeLabel��5z      $��Simulation Mode Active��6'   $
��Left=�6H   $	��481��6c   $��	22,23,0,0q�6p   $	��Top=�6�   $	��419��6�   ���	#FF272727$	 100��6�   $��24(�6�   �� 5{      -   5        6 ��Name$6 SafteyResetButton��5{      $��Reset Saftey��60   $
��Left=�6G   $��74��6b   $��	337,0,0,0q�6n   $	��Top=�6�   $	��240��6�   ���	#FF742F2F$��3q�6�   
���   �$	 100��6�   7 J� Opacity$	7 0.5Q�6  $��24(�6  $
��Bold"�6)  ��6�   Z�  5|        5}      8   
BlurRadius$8 40Q�6'   9   Color$	9 Red��67   :   ShadowDepth$: 0Q�6C   ;   	Direction$	; 315Q�6S   <   RenderingBias$< Quality=�6c   =   Opacity$	= 100Q�6{   5      �� 5�      -   5        $6 VirtualModeButton��5�      $��Simulate Motion��60   $
��Left=�6J   $��74��6e   $��
615,23,0,0q�6q   $	��Top=�6�   $	��240��6�   ���	#FF7E8D3D$��3q�6�   
���   �$	 100��6�   $	7 0.5Q�6  $��24(�6  $
��Bold"�6-  > � ToolTip$> No Error��6Y  ��6�   Z�  5�        5�      $8 20Q�6'   $9 Yellow��67   $: 0Q�6F   5�      �� 5�      -   5        ? ��Name$? 	ErrorIcon��5�      $	 180��6)   $>  ��6<   $	��112��6G   $��Bottom=�6T   $��Right=�6o   $	��118��6�   $��Resources/Error.png��6  5�      � 5�   
   $��2��6   @ � ColumnDefinitions@ 5�      �� 5�      $	��380 �6#   �� 5�      $��5 �6#   �� 5�      .+�5�      � $��2��6    5�      �� 5�      $��* �6$   �� 5�      $��5 �6$   �� 5�      $��40 �6$   .+�5�      �� $��1��6   $��Stretch=�6,   $��Stretch=�6J   
���   �6f   � 5�      -	   5        $��AxisGrid��5�      $��0��6(   $��Stretch=�66   $��Stretch=�6T   $��0,0,0,0q�6r   @ 5�      �� 5�      $��* �6+   �� 5�      $��* �6+    5�      �� 5�      $��* �6(   �� 5�      $��* �6(   �� 5�      $��* �6(   �� 5�      $��20 �6(   �� 5�      $��20 �6(   .+�5�      �� $��3��6    $��0��6-   $��1��6=   $��SELECTED QUE: ��6Q   A �� TextAlignment$A Right=�6g   
�������6}   �� 5�      -
   5        $��SelectedQueTextBox��5�       $��3��6:   $��1��6G   $��2��6W   $��SELECTED QUE: ��6k   $
A Left=�6�   
�������6�   5�      �� 5�      -   5        $��Generate Que��5�      $��2��60   $��Stretch=�6=   $	��Top=�6[   $��2��6u   
���   �
�������$��16(�6�   B �� 	ClickMode$B Press=�6�   
�������$��0,0,0,0q�6  5�      �� 5�      $��1��6   $��Stretch=�6+   $��Stretch=�6I   
���   �6e   &   System.Windows.Controls.DataGrid  5�      -   5        $��QueGrid��5�   b   $��Stretch=�6   $��0,0,0,0q�65   $��Stretch=�6F   
�������
�������C   RowStyle#C [ 6�   D   EnableRowVirtualizationD .  E   SelectionMode$E Single=�62  ��6�   Z� 62  �� 5�      -   5        $��QueMode��5�      $��Que Mode��6'   $��0,444,53,24q�6:   $��Right=�6O   $	��158��6k   6w   �� 5�      -   5        $��
TextBlock1��5�      $
��Left=�6*   $��
30,405,0,0q�6E   $
��Wrap=�6Y   $��	TextBlock��6m   $	��Top=�6~   $��34��6�   $	��297��6�   5�      � 5�   
   -   5        $��JoyStickGrid��5�      $��4��6#   @ 5�      �� 5�      $	��265 �6#   �� 5�      $��5 �6#   �� 5�      $	��265 �6#   �� 5�      $��5 �6#   �� 5�      $	��265 �6#   �� 5�      $��5 �6#   �� 5�      $	��265 �6#   �� 5�      .+�5�      �� $��1��6   $��Stretch=�6+   $��Stretch=�6I   
���   �6e   �� 5�      $��3��6   $��Stretch=�6+   $��Stretch=�6I   
���   �6e   �� 5�      $��5��6   $��Stretch=�6+   $��Stretch=�6I   
���   �6e   5�      � 5�   
   $��6��6   F � ContextMenuF 5�      �� 5�      .@�5�      m� -   5        $��JoyStick Setup��5�      67   5�      ��5�      �� 5�      $ 0.5,1&�6&   $
 0.5,0&�67   .��5�      � $ 	#FF131313��6#   $ 0Q�65   � 5�      $ 	#FF404040��6#   $ 1Q�65   5�      @ 5�      �� 5�      $	��265 �6#   �� 5�      $��5 �6#   �� 5�      $	��265 �6#   �� 5�      $��5 �6#   �� 5�      $	��265 �6#   �� 5�      $��5 �6#   �� 5�      $	��265 �6#   �� 5�      .+�5�      �� $��1��6   $��Stretch=�6+   $��Stretch=�6I   
���   �6e   �� 5�      $��3��6   $��5��6+   $��Stretch=�65   
���   �6Q   �� 5�      $��5��6   $��5��6+   $��Stretch=�65   
���   �6Q   �� 5�      -   5        $	? JS1��5�      $��	53,0,54,0q�6"   $
��Fill=�66   $��0��6E   $	7 0.5Q�6U   G J� RenderTransformOrigin$G 0.142,0.873&�6c   �� 5�      -   5        $	? JS2��5�      $��0,0,54,0q�6#   $
��Fill=�66   $��2��6E   $	7 0.5Q�6U   $��Right=�6c   $	��148��6   $G 0.041,1&�6�   $	��142��6�   $��Bottom=�6�   �� 5�      -   5        $	? JS3��5�      $��0,0,54,0q�6#   $
��Fill=�67   $��4��6F   $	7 0.5Q�6V   $��Right=�6d   $	��148��6�   $	��142��6�   $��Bottom=�6�   �� 5�      -   5        $��Button��5�      $
��Left=�6&   $��83��6A   $��0,69,0,0q�6M   $	��Top=�6_   $��94��6w   6�   5�      5�      5�      