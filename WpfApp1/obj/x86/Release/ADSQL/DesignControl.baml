   M S B A M L   `   `   `  ����   	Automater    CustomControl.AxisControl   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     %localusing:CustomControl     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     $��88��.��� $��0,0,0,0q�$��Stretch=�$��Stretch=�.+��� $��0q�$��Stretch=�$��Stretch=�._�� $��0,0,0,0q�$��Stretch=�$��Stretch=�  � ColumnDefinitions  �� $��* ��� $��20 ��� $��* �.+�� $��0�� � RowDefinitions �� $��* ��� $��* ��� $��* ��� $��* ��� $��* �.+��� $��Stretch=�$��Stretch=�$��NoWrap=�$��Axis:��$��1�� �� TextAlignment$ Right=�
��������� $��Stretch=�$��Stretch=�$��NoWrap=�$��Status:��$��2��$ Right=�
��������� $��Stretch=�$��Stretch=�$��NoWrap=�$��CurrentPosition:��$��3��$ Right=�
��������� $��Stretch=�$��Stretch=�$��NoWrap=�$��TargetPosition:��$��4��$ Right=�
�������� $��2�� �� $��* ��� $��* ��� $��* ��� $��* ��� $��* �.+��� -    ��Name$ AxisNameTextBox�� �� TextWrapping$ NoWrap=�$��Stretch=�$��Stretch=�$��1��$��1�� �� TextAlignment$
 Left=�$��Center=�
����������� *ItemsSource.AxisName+ �� UpdateSourceTrigger$ PropertyChanged=���Z� ��Z� �� -   $ AxisStatusTextBox��$ NoWrap=�$�� ��$��Stretch=�$��Stretch=�$��2��$��1��$
 Left=�$��Center=�
���������Z� ��Z� �� -   $ CurrentPositionTextBox��$ NoWrap=�$��Stretch=�$��Stretch=�$��3��$��1��$
 Left=�$��Center=�
����������� *CurrentPosition+��Z� ��Z� �� -   $ TargetPositionTextBox��$ NoWrap=�$��Stretch=�$��Stretch=�$��4��$��1��$
 Left=�$��Center=�
����������� *TargetPosition+ �� Mode$ OneWay=�$ PropertyChanged=���Z� ��Z� 