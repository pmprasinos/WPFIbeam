   M S B A M L   `   `   `  ���� P  LAutomater, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9d9bf72c0d89ee0e-clr-namespace:CustomControlCustomControl  !clr-namespace:WpfApp1WpfApp1  -clr-namespace:_treeListView_treeListView      WpfApp1.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     0controlsclr-namespace:CustomControl     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     'localclr-namespace:WpfApp1     )lclr-namespace:_treeListView     g cdevDept.Eyeshot.Control.Wpf.v9, Version=9.0.473.0, Culture=neutral, PublicKeyToken=5385d83d2fb4d7d9=ddes2http://schemas.devdept.com/winfx/2008/xaml/control  ` \devDept.Graphics.Wpf.v9, Version=9.0.473.0, Culture=neutral, PublicKeyToken=5385d83d2fb4d7d9>ddgr3http://schemas.devdept.com/winfx/2008/xaml/graphics  -     � WindowStyle$
  None=� � Title$ 
MainWindow�� � AllowsTransparency .  � 	Resources    myHeaderStyle �� 
TargetType=  7System.Windows.Controls.Primitives.DataGridColumnHeader �� Property �� Value  ExpandCollapseToggleStyle �� 
TargetType �� CornerRadius	 J� SnapsToDevicePixels
 �� 
StartPoint �� EndPoint � Color � Offset F�Name �� Triggers P� Property P� Value �� 
TargetName*   $_treeListView.LevelToIndentConverter  LevelToIndentConverter  ReadOnly_TLVItem d�Name �� 	ClickMode �� 	Converter �� RelativeSource �� AncestorType$   _treeListView.TreeListViewItem d� 	IsChecked �� Path �� Triggers �� Value �� Binding  CellTemplate_Name"   _treeListView.SpecialTextBox  Name"   _treeListView.SpecialTextBox   	LostFocus   GotKeyboardFocus!    PreviewMouseLeftButtonDown!   KeyDown
" � Tag#   
AxisNumber$   Property% �� Padding& ��Name' �� Content( �� Columns    _treeListView.TreeListView) ��Name* ]� 
Conditions+ �� Property, �� Value��%�  &	        &	 �     &	 �    &	 �    7  7  &	 �    '  �    '  �    �� "  .���  � ��
 ���� ��  � ��
 ���� ��  � ��
 �������  � ��
 �������  � y�$ Hidden=��� " d�.���  � |� .  ��  � ��$ 14����  � ��$ 15����  � �� �� " d�.R��� $��17��$��13��
������ ._��� $��15��$��19��$��1q����	#FF7898B5$ 1��	 . ���� $	
 0,0&�$	 2,2&���� $ White��$ .2Q�� $ 	#FFC0B7A6��$ 1Q�._�F� $ 
ExpandPath��$��3,3,1,1q�
���   �s����    �� �    ��- р� ��- р� @KL ���- @KL ���- ��- �@KL ��- �@KL �� ���- �� ���-     р�     р� �� 	 P�  �@d�	IsChecked$
 TrueY�. ��� $ 
ExpandPath�� � ��+ ��    �� �    ��- �@KL ��- �@KL �� 	  �� .R�]� .+�d� $ Expander��8��  $ Press=�$��18(����� *Level+8   �� "   �� $ 
IsExpanded� �� "  �� ���� *Name+ �� $ False�� �� $ HasItems� �� "  .a��� $ Expander�� � y�$ Hidden=��� .R�]� .+�  $	 stb�� TLVTextBox_LostFocus TLVTextBox_GotKeyBoardFocus   SelectivelyIgnoreMouseButton! TLVTextBox_KeyDown
�������$��18(����� *Value+��Z� ��Z� " �� *
Property+# �� *
AxisNumber+{��� *UserCanEdit+$ �� *
Property+�� "  .���  � �� �� "  .R��� .+��� $��Bd��
������ 
�������$	��0.5q�
�������#% z��._��� $& PART_Header��#' z��( �� $ Columns� �� "  �� $) 	ItemsHost�� P�  �@Q�
IsExpanded .  . ��� $ 	ItemsHost�� � y�$ 	Collapsed=�]� * �� + � ��, .  �� + � ��$
, Auto��./��� $ PART_Header�� � ��$ 75��]� * �� + � ��, .  �� + � ��$
, Auto��./��� $ PART_Header�� � ��$ 19��P�  �@Q�
IsSelected . . ���  � ��
 ������� $ Bd��# � �� � ���� # � �� � ��]� * �� + �@Q�
IsSelected, . �� + �@Q�IsSelectionActive, .  ./��� $ Bd��# � �� � ���� # � �� � ��P�  � {� .  . ��� # � �� � ���� "  .���  � �� �� "  .R�]� .+��� $	��Top=�( �� $ Columns� �� *TemplatedParent+�� - J� Effect- 3  -System.Windows.Media.Effects.DropShadowEffect  .��� ���� $ 0.5,1&�. � MappingMode$. RelativeToBoundingBox=�$
 0.5,0&�.��� $ 	#FF0E0E0E��$ 1Q�� $ 	#FF4B4B4B��$ 0.373Q�/ � RowDefinitions/ �� $��* ��� $��11 ��� $��* ��� $��11 ��� $��82 ��� $��11 ��� $	��140 �.+��� $��55(�$��Center=�$��Center=�$
��Wrap=��� $	�Top���� $��1��$��5��$��Stretch=�
���   ��� $��3��$��5��$��Stretch=�
���   ��� $��5��$��5��$��Stretch=�
���   �� $��0��.+�$  devDept.Eyeshot.ViewportLayout  -   0  Name$0 viewPortLayout1��$��Stretch=�$��0,0,0,0q�$��Stretch=�1   	HideSmall1 . 2 K� ZIndex$2 1��3   ShowFps3 . 4   ProgressBar4 !	  devDept.Eyeshot.ProgressBar	  5 	  Lighting5 .  6   	Viewports6 
  devDept.Eyeshot.Viewport
  7 
  DisplayMode$7 Rendered=�8 
  InitialView$8 Other=�9 
  
Background9 )  #devDept.Graphics.BackgroundSettings  :   BottomColor: �	#FF434752;   	StyleMode$; LinearGradient=�<   TopColor< �	#FF222329= 
  CoordinateSystemIcon= *  $devDept.Eyeshot.CoordinateSystemIcon  *  $devDept.Eyeshot.CoordinateSystemBase>   Lighting> .  $y�Hidden=�? 
  Camera?   devDept.Eyeshot.Camera  @   Distance$	@ 600Q�A   ProjectionMode$A Perspective=�B   Rotation\ XdevDept.Geometry.v9, Version=9.0.473.0, Culture=neutral, PublicKeyToken=5385d83d2fb4d7d95  /devDept.Geometry.Converters.QuaternionConverter$QB K0.0868240888334652, 0.150383733180435, 0.492403876506104, 0.852868531952443 C   Target2  ,devDept.Geometry.Converters.Point3DConverter$C 0, 0, 50 D   
ZoomFactor$D 2Q�E 
  GridsE   devDept.Eyeshot.Grid  F   
ColorAxisYF �	#7F202020G   
ColorAxisXG �	#7F202020H   LightingH .  I   MajorLineColorI �	#7F5A5A5AJ   MajorLinesEvery$J 5��K   Step$K 5Q�L   VisibleL .  M 
  OriginSymbolM "  devDept.Eyeshot.OriginSymbol  > .  $y�Hidden=�N 
  RotateN $  devDept.Eyeshot.RotateSettings  *  $devDept.Eyeshot.MovementSettingsBaseO   EnabledO . P 
  ToolBarP   devDept.Eyeshot.ToolBar  Q   ButtonsQ '  !devDept.Eyeshot.HomeToolBarButton  2  ,devDept.Eyeshot.MagnifyingGlassToolBarButton  -  'devDept.Eyeshot.ZoomWindowToolBarButton  '  !devDept.Eyeshot.ZoomToolBarButton  &   devDept.Eyeshot.PanToolBarButton  )  #devDept.Eyeshot.RotateToolBarButton  *  $devDept.Eyeshot.ZoomFitToolBarButton  R 
  ViewCubeIconR "  devDept.Eyeshot.ViewCubeIcon  S   LightingS .  �� -   T ��Name$T ShadeRectangle1��U �� Stretch$
U Fill=�$��1��$��
10,10,10,0q�$	2 100�����	#FFB6B6B6$��5��- -  'System.Windows.Media.Effects.BlurEffect  V   Radius$V 10Q��� -   W ��Name$W 	ModeLabel��$��Simulation Mode Active��$
��Left=�$	��481��$��	22,23,0,0q�$	��Top=�$	��419�����	#FF272727$	2 100��$��24(��� -   X ��Name$X SafteyResetButton��$��Reset Saftey��$
��Left=�$��74��$��
337,23,0,0q�$	��Top=�$	��240�����	#FF742F2F$��3q�
���   �$	2 100��Y J� Opacity$	Y 0.5Q�$��24(�$
��Bold"���Z� -   Z   
BlurRadius$Z 40Q�[   Color$	[ Red��\   ShadowDepth$\ 0Q�]   	Direction$	] 315Q�^   RenderingBias$^ Quality=�_   Opacity$	_ 100Q��� -   $X VirtualModeButton��$��Simulate Motion��$
��Left=�$��74��$��
615,23,0,0q�$	��Top=�$	��240�����	#FF7E8D3D$��3q�
���   �$	2 100��$	Y 0.5Q�$��24(�$
��Bold"�` � ToolTip$` No Error����Z� -   $Z 20Q�$[ Yellow��$\ 0Q��� -   a ��Name$a 	ErrorIcon��$	2 180��$`  ��$	��112��$��Bottom=�$��Right=�$	��118��$��Resources/Error.png��� -   b �Name$b DataGrid_Prop��$��2��c � ColumnDefinitionsc �� $	��500 ��� $��5 ��� .+�� $��2��/ �� $��* ��� $��5 ��� $��* �.+��� $��1��$��Stretch=�$��Stretch=�
���   ��� $��1��$��Stretch=�$��Stretch=�
���   �&   System.Windows.Controls.DataGrid  -	   $��
StatesGrid��$
��Left=�$	��431��$��	10,10,0,0q�$	��Top=�$
��1124��$��3��d   AutoGenerateColumnsd .  e   SelectionMode$e Single=�
������ f   RowBackground
f ���� 
�������g   Columnsg 0   *System.Windows.Controls.DataGridTextColumn   ,!  &System.Windows.Controls.DataGridColumnh !  Header$h ID��-
   ID��i !  Width5"  /System.Windows.Controls.DataGridLengthConverter$i 0" j !  
Visibility$j Hidden=�k !  HeaderStyle#k [  1#  +System.Windows.Controls.DataGridBoundColumnl #  Bindingl �� *	StateID+   $h State��$	i 100" #k [  l �� *	StateName+   $h Notes��$	i 200" m �� LineStackingStrategy$m BlockLineHeight=�#k [  l �� *Notes+n   RowDetailsTemplaten �� .R�]� ��Z� " �� *ID+y���  �� )$  #System.Windows.Controls.DataGridRow" $ $ 
IsSelected�.+��� $
��Left=�$��64��$��10q����� *
ImageUrl+� c �� $	��255 ��� $��5 ��� $	��255 ��� $��5 ��� $	��255 ��� $��5 ��� $	��255 �.+��� $��1��$��Stretch=�$��Stretch=�
���   ��� $��3��$��Stretch=�$��Stretch=�
���   ��� $��5��$��Stretch=�$��Stretch=�
���   �� $
��0,10q�$��0��c �� $	��100 ��� $	��100 ��� $��10 ��� $	��100 ��� $	��100 ��� $��10 ��� $	��100 ��� $	��100 ��� $��10 �/ �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto �.+��� $��
AxisName: ��$
��Bold"�o �� TextAlignment$o Right=��� $" 
tbAxisName��$��1������ *
AxisName+�� $��TargetPosition: ��$
��Bold"�$��1��$o Right=��� $" tbTargetPosition��$��1��$��1��{�.  ���� *TargetPosition+�� $��Max Speed: ��$
��Bold"�$��3��$o Right=��� $" 
tbVelocity��$��1��$��3��{�.  ���� *
Velocity+�� $��Accel: ��$
��Bold"�$��0��$��3��$o Right=��� $" tbAcceleration��$��4��$��0��{�.  ���� *Acceleration+�� $��Deccel: ��$
��Bold"�$��1��$��3��$o Right=��� $" tbDeceleration��$��4��$��1��{�.  ���� *Deceleration+p �� UpdateSourceTrigger$p PropertyChanged=��� $��MoveTime(est): ��$
��Bold"�$��2��$��3��$o Right=��� $" 
tbMoveTime��$��4��$��2��{�.  ���� *
moveTime+$p PropertyChanged=��� $��CurrentPosition: ��$
��Bold"�$��3��$��3��$o Right=��� $" tbCurrentPosition��$��4��$��3��{�.  ���� *CurrentPosition+$p PropertyChanged=�� $
��0,10q�$��2��c �� $	��100 ��� $	��100 ��� $��10 ��� $	��100 ��� $	��100 ��� $��10 ��� $	��100 ��� $	��100 ��� $��10 �/ �� $
��Auto ��� $
��Auto ��� $
��Auto �.+��� $��Axis: ��$
��Bold"�$o Right=��� $��1������ *
AxisName+�� $��TargetPosition: ��$
��Bold"�$��1��$o Right=��� $��1��$��1��{�.  ���� *TargetPosition+�� $��Max Speed: ��$
��Bold"�$��3��$o Right=��� $��1��$��3��{�.  ���� *
Velocity+�� $��Accel: ��$
��Bold"�$��0��$��3��$o Right=��� $��4��$��0��{�.  ���� *Acceleration+��Z� �� $��Deccel: ��$
��Bold"�$��1��$��3��$o Right=��� $��4��$��1��{�.  ���� *Acceleration+��Z� �� $��MoveTime(est): ��$
��Bold"�$��2��$��3��$o Right=��� $��4��$��2��{�.  ���� *
moveTime+�� -   $&�� Store Selected AxisGroup Targets��$��Stretch=�$��Bottom=�$��46��$��0��
���   �
�������$��16(�$ Press=�
�������$��0��$��3��$��390,-66,415,535q�� $��4��c �� $	��265 ��� $��5 ��� $	��265 ��� $��5 ��� $	��265 ��� $��5 ��� $	��265 ��� .+�%   CustomControl.AxisControl%  -   q % Name$q AxisControl_1��$��Stretch=�$��Stretch=�r %  TextSize$r 10��$��0��s %  
AxisNumber$s 1��$��	0,-10,0,0q�%  -   $q AxisControl_2��$��Stretch=�$��Stretch=�$r 10��$��2��$s 2��$��	0,-10,0,0q�%  -   $q AxisControl_3��$��Stretch=�$��Stretch=�$r 10��$��4��$s 3��$��	0,-10,0,0q�%  -   $q AxisControl_4��$��Stretch=�$��Stretch=�$r 10��$��6��$s 4��$��	0,-10,0,0q��� $��1��$��Stretch=�$��Stretch=�
���   ��� $��3��$��Stretch=�$��Stretch=�
���   ��� $��5��$��Stretch=�$��Stretch=�
���   �� $��6��t � ContextMenut �� .@�m� -   $��JoyStick Setup������ $ 0.5,1&�$
 0.5,0&�.��� $ 	#FF131313��$ 0Q�� $ 	#FF404040��$ 1Q�c �� $	��265 ��� $��5 ��� $	��265 ��� $��5 ��� $	��265 ��� $��5 ��� $	��265 ��� .+��� $��1��$��Stretch=�$��Stretch=�
���   ��� $��3��$��5��$��Stretch=�
���   ��� $��5��$��5��$��Stretch=�
���   ��� -   $	a JS1��$��	53,0,54,0q�$
��Fill=�$��0��$	Y 0.5Q�u J� RenderTransformOrigin$u 0.142,0.873&��� -   $	a JS2��$��0,0,54,0q�$
��Fill=�$��2��$	Y 0.5Q�$��Right=�$	��148��$u 0.041,1&�$	��142��$��Bottom=��� -   $	a JS3��$��0,0,54,0q�$
��Fill=�$��4��$	Y 0.5Q�$��Right=�$	��148��$	��142��$��Bottom=��� -   $	a JS4��$��	54,0,63,0q�$
��Fill=�$��6��$	Y 0.5Q�$	��142��$��Bottom=��� -   $��Button��$
��Left=�$��83��$��0,69,0,0q�$	��Top=�$��94��