   M S B A M L   `   `   `  ���� H  DLANSPYproject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:LANSPYprojectLANSPYproject      LANSPYproject.Scanner   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -localclr-namespace:LANSPYproject     ���#F5F7FA  � 	Resources  0   *LANSPYproject.BoolToOnlineOfflineConverter    BoolToOnlineOfflineConverter(   "LANSPYproject.BoolToBrushConverter  BoolToBrushConverter��%   &	        &	          .���  � RowDefinitions �� $
��Auto ��� $��* �.+�� $��0��$��20q� �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto �.+��� $��0��$"��🔍 Quét mạng nội bộ��$��18(�$
��Bold"�$��	0,10,0,10q�� $��1��$��0,0,0,10q�.+��� $��📡 Mạng: ��$
��Bold"��� ���#2563EB$��5,0,20,0q����� *CurrentNetworkRange+�� $��⏱ Trạng thái: ��$
��Bold"��� -    ��Name$ StatusTextBlock��$��Chưa quét��
�������$��5,0,0,0q��� $��2�� �� Orientation$ 
Horizontal=�$��0,0,0,10q�$
��Left=�.+��� -    ��Name$ 
ScanButton��$��Quét mạng��$	��120��$��35��$	��8,4q�$
��Bold"�$��0q�$��0,0,10,0q� � Cursor$
 Hand��
�������$��14(�����  �� 
TargetType" ��.R���  ��Name$ border��#��z�� �� CornerRadius$ 20��	 �� Padding#	 z��'  !System.Windows.VisualStateManager
   VisualStateGroups
 %  System.Windows.VisualStateGroup  CommonStates��   States.    System.Windows.VisualState  
Normal��  	MouseOver��   
Storyboard. �� .����  �� 
TargetName$ border�� �� TargetProperty$@ :(Border.Background).(GradientBrush.GradientStops)[0].Color�	 �� To #1A47B8 g� Duration$ 0:0:0.3D��� $ border��$@ :(Border.Background).(GradientBrush.GradientStops)[1].Color� #4B7BFF$ 0:0:0.3D�  Disabled��. �� .���� $ border��$@ :(Border.Background).(GradientBrush.GradientStops)[0].Color� #A9A9A9$ 0:0:0.3D��� $ border��$@ :(Border.Background).(GradientBrush.GradientStops)[1].Color� #A9A9A9$ 0:0:0.3D�._��� $��Center=�$��Center=�����  �� 
StartPoint$	 0,0&� �� EndPoint$	 1,0&�-   buttonBackground��.���  � Color$ #123288�� � Offset$ 0Q�� $ #295EEC��$ 1Q�����  �� 
TargetType" �� �� BasedOn# [B��.���  �� Property � �� �� Value
 �������  �@�Cursor$
 Hand�� �� Triggers P�  P� Property � {� P� Value$ False��. ���  � �� �#DDDDDD��  �@�Cursor$ Arrow���� -   $ 
StopButton��$��Dừng quét��$	��120��$��35��$	��8,4q�$
��Bold"�$��0q�${�False��$
 Hand��
�������$��14(����� " ��.R��� $ border��#��z��$ 20��#	 z��
   CommonStates��.   
Normal��  	MouseOver��. �� .���� $ border��$@ :(Border.Background).(GradientBrush.GradientStops)[0].Color� #F87171$ 0:0:0.3D��� $ border��$@ :(Border.Background).(GradientBrush.GradientStops)[1].Color� #FCA5A5$ 0:0:0.3D�  Disabled��. �� .���� $ border��$@ :(Border.Background).(GradientBrush.GradientStops)[0].Color� #A9A9A9$ 0:0:0.3D��� $ border��$@ :(Border.Background).(GradientBrush.GradientStops)[1].Color� #A9A9A9$ 0:0:0.3D�._��� $��Center=�$��Center=����� $	 0,0&�$	 1,0&�-   buttonBackgroundStop��.��� $ #EF4444��$ 0Q�� $ #DC2626��$ 1Q����� " ��# [B��.���  � ��
 �������  �@�Cursor$
 Hand�� P�  � {�$ False��. ���  � �� �#DDDDDD��  �@�Cursor$ Arrow���� $��3��$��Danh sách thiết bị��$
��Bold"�$��0,0,0,10q��� $��1��
�������$ 10��$��20q�$	 10q�._�&   System.Windows.Controls.DataGrid  -     Name$ deviceDataGrid��   AutoGenerateColumns$ False��   CanUserAddRows$ False��   HeadersVisibility$ Column=�
�������
�������$��1q�    
IsReadOnly$
  True��!   	RowHeight$! 30Q�$
��Auto=�"   Columns" 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn#   Header$# ID��$   Width5	  /System.Windows.Controls.DataGridLengthConverter$$ 50	 1
  +System.Windows.Controls.DataGridBoundColumn% 
  Binding% �� *ID+  $# IP��$	$ 150	 % �� *IP+  $	# MAC��$	$ 200	 % �� *MAC+  $
# Name��$$ *	 % �� *Name+  $
# Date��$	$ 150	 % �� *
ScanDate+4  .System.Windows.Controls.DataGridTemplateColumn  $# Status��$	$ 120	 &   
IsReadOnly$
& True��'   CellTemplate' �� .R��� $ 
Horizontal=�$��Center=�$��Center=�.+�A� $��14��$��14��$��0,0,6,0q����� *IsOn+( �� 	Converter#( [ �� $��Center=�$
��Bold"�
���   �) �� TextAlignment$) Center=����� *IsOn+#( [  