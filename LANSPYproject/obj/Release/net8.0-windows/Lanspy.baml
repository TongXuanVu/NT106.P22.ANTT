   M S B A M L   `   `   `  ���� H  DLANSPYproject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:LANSPYprojectLANSPYproject      LANSPYproject.Lanspy   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     -localclr-namespace:LANSPYproject       � Title$  LANSpy��$	��400��$	��600�����#F5F7FA � 	Resources    
HoverBrush �� Color  CheckedBrush  TransparentBrush  SidebarButtonStyle �� 
TargetType �� Property �� Value �� 
TargetType ��Name �� CornerRadius	 �� Padding
 �� Triggers P� Property P� Value �� 
TargetName��%*  &	        &	      &	 &     &	 =     7  7  7   7  �� $ #EEF4FF���� $ #DEEBFF���� $ Transparent���� " 	�.���  � ��$ Segoe UI)���  � ��$ 15(���  � �� �#2C3E50��  � ��8   ��  � ��
 ���� ��  �@�Cursor$
 Hand����  � ��$
 Left=���  � ��$ 15,10q���  � ��$ 0,6,0,6q���  � �� �� " 	�.R�]� .+��� $ 	Indicator��$��4�����#2563EB$y�	Collapsed=�$��Stretch=�$ 2,0,0,2���� $ 
ButtonArea��8�� $ 8��$
	 12,6q�$
��Auto��._��� $��Center=�
 P�  �@J�IsMouseOver$
 True��. ��� $ 
ButtonArea��8   � ��P�  �@d�	IsChecked$
 TrueY�. ��� $ 	Indicator�� � y�$ Visible=��� $ 
ButtonArea��8   � ����  � �� �#2563EB��  � ��$ SemiBold"�.���  � ColumnDefinitions �� $
��Auto �$	��150���� $��* �.+��� $
��Auto=�$��0��$	��160��$	��250��.���� 
�������.+��� $��🖥 LANSpy��$
��Bold"�$��16(�$��
10,20,0,20q����#2563EB	� -   $��🏠  Dashboard��#��[  	� 	GroupName$ Sidebar��	� -   $��🔍  Scanner��#��[ $ Sidebar��	� -   $��
📄  Logs��#��[ $ Sidebar��	� -   $��⚠️  Alerts��#��[ $ Sidebar��	� -   $��⚙️  Settings��#��[ $ Sidebar��� $��1�� � RowDefinitions �� $
��Auto ��� $��* �.+�]� 
�������$��50��$	��Top=�$��False��$��0,0,0,10q�.+��� $/��)Hệ thống giám sát mạng nội bộ��$��14(�$
��Bold"�$��Center=�$��20,0,0,0q��� $��Right=�$��0,0,20,0q�$		 6,3q����#E6F0FF���#2563EB$��1q�$ 6��$��Center=� ^� ToolTip �� *WifiTooltip+._���  �� Orientation$ 
Horizontal=�$��Center=�.+��� $
��📶��$��14(�$��0,0,5,0q��� -    ��Name$ WifiNameTextBlock��$��12(�$��SemiBold"�$��Center=��� -    ��Name$ MainContent��$��1��