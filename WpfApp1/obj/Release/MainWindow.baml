   M S B A M L   `   `   `  ���� P  LAutomater, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9d9bf72c0d89ee0e!clr-namespace:WpfApp1WpfApp1  -clr-namespace:_treeListView_treeListView      WpfApp1.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     'localclr-namespace:WpfApp1     )lclr-namespace:_treeListView     g cdevDept.Eyeshot.Control.Wpf.v9, Version=9.0.473.0, Culture=neutral, PublicKeyToken=5385d83d2fb4d7d9=ddes2http://schemas.devdept.com/winfx/2008/xaml/control  ` \devDept.Graphics.Wpf.v9, Version=9.0.473.0, Culture=neutral, PublicKeyToken=5385d83d2fb4d7d9>ddgr3http://schemas.devdept.com/winfx/2008/xaml/graphics  -     � WindowStyle$
  None=� � Title$ 
MainWindow�� � AllowsTransparency .  � 	Resources    myHeaderStyle �� 
TargetType �� Property �� Value  ExpandCollapseToggleStyle �� 
TargetType �� CornerRadius	 J� SnapsToDevicePixels
 �� 
StartPoint �� EndPoint � Color � Offset F�Name �� Triggers P� Property P� Value �� 
TargetName*   $_treeListView.LevelToIndentConverter  LevelToIndentConverter  ReadOnly_TLVItem d�Name �� 	ClickMode �� 	Converter �� RelativeSource �� AncestorType$   _treeListView.TreeListViewItem d� 	IsChecked �� Path �� Triggers �� Value �� Binding  CellTemplate_Name"   _treeListView.SpecialTextBox  Name"   _treeListView.SpecialTextBox   	LostFocus   GotFocus    GotKeyboardFocus!!   PreviewMouseLeftButtonDown
" � Tag#   
AxisNumber$ �� Padding% ��Name& �� Content' �� Columns    _treeListView.TreeListView( ��Name) ]� 
Conditions* �� Property+ �� Value��%�  &	        &	 �     &	 �    &	 �    7  7  &	 �    '  �    '  �    �� " ��.���  � ��
 ���� ��  � ��
 ���� ��  � ��
 �������  � ��
 �������  � y�$ Hidden=��� " d�.���  � |� .  ��  � ��$ 19����  � ��$ 13����  � �� �� " d�.R��� $��19��$��13��
������ ._��� $��9��$��9��$��1q����	#FF7898B5$ 1��	 . ���� $	
 0,0&�$	 1,1&���� $ White��$ .2Q�� $ 	#FFC0B7A6��$ 1Q�._�F� $ 
ExpandPath��$��1,1,1,1q�
���   �s����    �� �    ��- р� ��- р� @KL ���- @KL ���- ��- �@KL ��- �@KL �� ���- �� ���-     р�     р� �� 	 P�  �@d�	IsChecked$
 TrueY�. ��� $ 
ExpandPath�� � ��+ ��    �� �    ��- �@KL ��- �@KL �� 	  �� .R�]� .+�d� $ Expander��8��  $ Press=����� *Level+8   �� "   �� $ 
IsExpanded� �� "  �� ���� *Name+ �� $ False�� �� $ HasItems� �� "  .a��� $ Expander�� � y�$ Hidden=��� .R�]� .+�  $ tb�� TLVTextBox_LostFocus TLVTextBox_GotFocus  TLVTextBox_GotKeyBoardFocus ! SelectivelyIgnoreMouseButton
����������� *Value+��Z� ��Z� " �� *
Property+# �� *Axis+{��� *UserCanEdit+�� "  .���  � �� �� "  .R��� .+��� $��Bd��
������ 
�������$	��0.5q�
�������#$ z��._��� $% PART_Header��#& z��' �� $ Columns� �� "  �� $( 	ItemsHost�� P�  �@Q�
IsExpanded .  . ��� $ 	ItemsHost�� � y�$ 	Collapsed=�]� ) �� * � ��+ .  �� * � ��$
+ Auto��./��� $ PART_Header�� � ��$ 75��]� ) �� * � ��+ .  �� * � ��$
+ Auto��./��� $ PART_Header�� � ��$ 19��P�  �@Q�
IsSelected . . ���  � ��
 ������� $ Bd��# � �� � ���� # � �� � ��]� ) �� * �@Q�
IsSelected+ . �� * �@Q�IsSelectionActive+ .  ./��� $ Bd��# � �� � ���� # � �� � ��P�  � {� .  . ��� # � �� � ���� "  .���  � �� �� "  .R�]� .+��� $	��Top=�' �� $ Columns� �� *TemplatedParent+�� , J� Effect, 3  -System.Windows.Media.Effects.DropShadowEffect  .��� ���� $ 0.5,1&�- � MappingMode$- RelativeToBoundingBox=�$
 0.5,0&�.��� $ 	#FF0E0E0E��$ 1Q�� $ 	#FF4B4B4B��$ 0.373Q�. � RowDefinitions. �� $��* ��� $��11 ��� $��* ��� $��11 ��� $	��152 �.+��� $��55(�$��Center=�$��Center=�$
��Wrap=��� $	�Top���� $��1��$��5��$��Stretch=�
���   ��� $��3��$��5��$��Stretch=�
���   �� $��0��.+�$  devDept.Eyeshot.ViewportLayout  -   /  Name$/ viewPortLayout1��$��Stretch=�$��0,0,0,0q�$��Stretch=�0   	HideSmall0 . 1 K� ZIndex$1 1��2   ShowFps2 . 3   ProgressBar3 !  devDept.Eyeshot.ProgressBar  4   Lighting4 .  5   	Viewports5 	  devDept.Eyeshot.Viewport	  6 	  DisplayMode$6 Rendered=�7 	  InitialView$7 Other=�8 	  
Background8 )
  #devDept.Graphics.BackgroundSettings
  9 
  BottomColor9 �	#FF434752: 
  	StyleMode$: LinearGradient=�; 
  TopColor; �	#FF222329< 	  CoordinateSystemIcon< *  $devDept.Eyeshot.CoordinateSystemIcon  *  $devDept.Eyeshot.CoordinateSystemBase=   Lighting= .  $y�Hidden=�> 	  Camera>   devDept.Eyeshot.Camera  ?   Distance$	? 600Q�@   ProjectionMode$@ Perspective=�A   Rotation\ XdevDept.Geometry.v9, Version=9.0.473.0, Culture=neutral, PublicKeyToken=5385d83d2fb4d7d95  /devDept.Geometry.Converters.QuaternionConverter$QA K0.0868240888334652, 0.150383733180435, 0.492403876506104, 0.852868531952443 B   Target2  ,devDept.Geometry.Converters.Point3DConverter$B 0, 0, 50 C   
ZoomFactor$C 2Q�D 	  GridsD   devDept.Eyeshot.Grid  E   
ColorAxisYE �	#7F202020F   
ColorAxisXF �	#7F202020G   LightingG .  H   MajorLineColorH �	#7F5A5A5AI   MajorLinesEvery$I 5��J   Step$J 5Q�K   VisibleK .  L 	  OriginSymbolL "  devDept.Eyeshot.OriginSymbol  = .  $y�Hidden=�M 	  RotateM $  devDept.Eyeshot.RotateSettings  *  $devDept.Eyeshot.MovementSettingsBaseN   EnabledN . O 	  ToolBarO   devDept.Eyeshot.ToolBar  P   ButtonsP '  !devDept.Eyeshot.HomeToolBarButton  2  ,devDept.Eyeshot.MagnifyingGlassToolBarButton  -  'devDept.Eyeshot.ZoomWindowToolBarButton  '  !devDept.Eyeshot.ZoomToolBarButton  &   devDept.Eyeshot.PanToolBarButton  )  #devDept.Eyeshot.RotateToolBarButton  *  $devDept.Eyeshot.ZoomFitToolBarButton  Q 	  ViewCubeIconQ "  devDept.Eyeshot.ViewCubeIcon  R   LightingR .  �� -   S ��Name$S ShadeRectangle1��T �� Stretch$
T Fill=�$��1��$��
10,10,10,0q�$	1 100�����	#FFB6B6B6$��5��, -  'System.Windows.Media.Effects.BlurEffect  U   Radius$U 10Q��� -   V ��Name$V 	ModeLabel��$��Simulation Mode Active��$
��Left=�$	��481��$��	22,23,0,0q�$	��Top=�$	��419�����	#FF272727$	1 100��$��24(��� -   W ��Name$W SafteyResetButton��$��Reset Saftey��$
��Left=�$��74��$��
337,23,0,0q�$	��Top=�$	��240�����	#FF742F2F$��3q�
���   �$	1 100��X J� Opacity$	X 0.5Q�$��24(�$
��Bold"���Z� ,   Y   
BlurRadius$Y 40Q�Z   Color$	Z Red��[   ShadowDepth$[ 0Q�\   	Direction$	\ 315Q�]   RenderingBias$] Quality=�^   Opacity$	^ 100Q��� -   $W VirtualModeButton��$��Simulate Motion��$
��Left=�$��74��$��
615,23,0,0q�$	��Top=�$	��240�����	#FF7E8D3D$��3q�
���   �$	1 100��$	X 0.5Q�$��24(�$
��Bold"�_ � ToolTip$_ No Error����Z� ,   $Y 20Q�$Z Yellow��$[ 0Q��� -   ` ��Name$` 	ErrorIcon��$��obj/Error.png��$	1 180��$_  ��$	��112��$��Bottom=�$��Right=�$	��118��� -   a �Name$a DataGrid_Prop��$��2��b � ColumnDefinitionsb �� $	��500 ��� $��5 ��� .+�� $��2��. �� $��* ��� $��5 ��� $��* �.+��� $��1��$��Stretch=�$��Stretch=�
���   ��� -	   $��Store AxisGroup State��$��Stretch=�$��Bottom=�$��46��$��2��$��1��
���   �
�������$��16(�$ Press=�
�������$��0��  -
   c  Name$	c TLV��    _treeListView.TreeListViewd   SelectedItemChanged$&d  TreeListView_SelectedItemChanged��e   PreviewMouseDown$e TLV_PreviewMouseDown��$��Stretch=�$��Stretch=�$��
0,-25,0,46q����	#FF814A4A
���   �
�������  TreeViewItemStyle1#���  $��16(�$
1 1000��$��0��f   Columnsf �� $�� ��g �� CellTemplate#g [ h �� Width$	h 150��i �� HeaderContainerStyle#i [  �� $
��Type��$	h 150��#g [ #i [  
�������j �� DisplayMemberBindingj �� *
Property+�� $��Value��$h 0��#g [ #i [  
�������j �� *Value+�� $��PreparedValue��#g [ $	h 100��#i [  �� $��1��$��Stretch=�$��Stretch=�
���   ��� -   $V Label1��$��Label��$
��Left=�$��87��$��
85,161,0,0q�$	��Top=�$	��300��� $��4��k � ContextMenuk �� .@�m� -   $��JoyStick Setup������ $ 0.5,1&�$
 0.5,0&�.��� $ 	#FF131313��$ 0Q�� $ 	#FF404040��$ 1Q�b �� $	��265 ��� $��5 ��� $	��265 ��� $��5 ��� $	��265 ��� $��5 ��� $	��265 ��� .+��� $��1��$��Stretch=�$��Stretch=�
���   ��� $��3��$��5��$��Stretch=�
���   ��� $��5��$��5��$��Stretch=�
���   ��� -   $	` JS1��$��
0,10,107,0q�$
��Fill=�$��0��$	X 0.5Q�l J� RenderTransformOrigin$l 0.142,0.873&��� -   $	` JS2��$��	0,0,112,0q�$
��Fill=�$��2��$	X 0.5Q�$��Right=�$	��148��$l 0.041,1&�$	��142��$��Bottom=��� -   $	` JS3��$��	0,0,112,0q�$
��Fill=�$��4��$	X 0.5Q�$��Right=�$	��148��$	��142��$��Bottom=��� -   $	` JS4��$��
0,0,107,10q�$
��Fill=�$��6��$	X 0.5Q�$��Right=�$	��148��$	��142��$��Bottom=��� $<��6AxisGroup: iBeam_1Twist: ElevationUp/Down: Pitch    ��$��Right=�$	��142��$	��Top=�$	��107��$��0��
�������$��Right=��� $��Label��$
��Left=�$	��142��$��	153,0,0,0q�$	��Top=�$	��107��$��2��
��������� $��Label��$��Right=�$	��142��$	��Top=�$	��107��$��4��
��������� $��Label��$
��Left=�$	��142��$��	158,0,0,0q�$	��Top=�$	��107��$��6��
��������� -   $��Button��$
��Left=�$��83��$��0,69,0,0q�$	��Top=�$��94��