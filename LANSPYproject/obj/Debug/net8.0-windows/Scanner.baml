   M S B A M L   `   `   `  ����H  DLANSPYproject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:LANSPYprojectLANSPYproject  5          LANSPYproject.Scanner   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      -localclr-namespace:LANSPYproject     5      ���#F5F7FA  � 	Resources  5      0   *LANSPYproject.BoolToOnlineOfflineConverter    BoolToOnlineOfflineConverter(   "LANSPYproject.BoolToBrushConverter  BoolToBrushConverter��%:   &	        &	        5   
   6-     5	   
   6%   5
      .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   � $��0��6   $��20q�6    5      �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    .+�5      �� $��0��6   $"��🔍 Quét mạng nội bộ��6%   $��18(�6@   $
��Bold"�6N   $��	0,10,0,10q�6`   � 5      $��1��6   $��0,0,0,10q�6%   .+�5      �� $��📡 Mạng: ��6   $
��Bold"�6-   �� 5      ���#2563EB$��5,0,20,0q�6V   ��6   �� *CurrentNetworkRange+6V   �� 5       $��⏱ Trạng thái: ��6   $
��Bold"�62   �� 5!      -   5         ��Name$ StatusTextBlock��5!      $��Chưa quét��65   
�������$��5,0,0,0q�6X   5"      �� 5$      $��2��6    �� Orientation$ 
Horizontal=�6&   $��0,0,0,10q�6?   $
��Left=�6Q   .+�5%      �� -   5         ��Name$ 
ScanButton��5%      $��Quét mạng��6-   $	��120��6A   $��35��6M   $	��8,4q�6Y   $
��Bold"�6g   $��0q�5&      $��0,0,10,0q�6F    � Cursor$
 Hand��6X   
�������$��14(�6y   ��5(      �� 5)       �� 
TargetType" ��6*   .R�5*      ��  ��Name$ border��6%   #��z��65    �� CornerRadius$ 20��6_   	 �� Padding#	 z��6q   '  !System.Windows.VisualStateManager
   VisualStateGroups
 5+   "   %  System.Windows.VisualStateGroup  5,   &   CommonStates��   States. 5-   *      System.Windows.VisualState  
Normal��66     5.   *   	MouseOver��   
Storyboard. 5/   .   �� .��50   2   ��  �� 
TargetName$ border��6A    �� TargetProperty$@ :(Border.Background).(GradientBrush.GradientStops)[0].Color�51   A   	 �� To #1A47B852   A    g� Duration$ 0:0:0.3D�6N   �� 53   2   $ border��6A   $@ :(Border.Background).(GradientBrush.GradientStops)[1].Color�54   A    #4B7BFF55   A   $ 0:0:0.3D�6N   56   /   57   +     58   *   Disabled��. 59   .   �� .��5:   2   �� $ border��6A   $@ :(Border.Background).(GradientBrush.GradientStops)[0].Color�5;   A    #A9A9A95<   A   $ 0:0:0.3D�6N   �� 5=   2   $ border��6A   $@ :(Border.Background).(GradientBrush.GradientStops)[1].Color�5>   A    #A9A9A95?   A   $ 0:0:0.3D�6N   5@   /   5A   +   5B   '   ._�5D   "   �� $��Center=�63   $��Center=�6P   5E      5F      ��5H      �� 5I       �� 
StartPoint$	 0,0&�6.    �� EndPoint$	 1,0&�6?   -   5        buttonBackground��.��5J      �  � Color$ #123288��6+    � Offset$ 0Q�6;   � 5K      $ #295EEC��6+   $ 1Q�6;   5L      ��5N      �� 5O       �� 
TargetType" ��6     �� BasedOn# [B��64   .�5P      ��  �� Property � �� �� Value
 �����6;   �� 5Q       �@�Cursor$
 Hand��67    �� Triggers 5R      P� 5S   "    P� Property � {� P� Value$ False��6?   . �5T   &   ��  � �� �#DDDDDD6C   �� 5U   &    �@�Cursor$ Arrow��6?   5V   #   5X      5Z      �� 5\      -   5        $ 
StopButton��5\      $��Dừng quét��6-   $	��120��6A   $��35��6M   $	��8,4q�6Y   $
��Bold"�6g   $��0q�5]      ${�False��6F   $
 Hand��6X   
�������$��14(�6y   ��5_      �� 5`      " ��6*   .R�5a      �� $ border��6%   #��z��65   $ 20��6_   #	 z��6q   
 5b   "     5c   &   CommonStates��. 5d   *     
Normal��66     5e   *   	MouseOver��. 5f   .   �� .��5g   2   �� $ border��6A   $@ :(Border.Background).(GradientBrush.GradientStops)[0].Color�5h   A    #F871715i   A   $ 0:0:0.3D�6N   �� 5j   2   $ border��6A   $@ :(Border.Background).(GradientBrush.GradientStops)[1].Color�5k   A    #FCA5A55l   A   $ 0:0:0.3D�6N   5m   /   5n   +     5o   *   Disabled��. 5p   .   �� .��5q   2   �� $ border��6A   $@ :(Border.Background).(GradientBrush.GradientStops)[0].Color�5r   A    #A9A9A95s   A   $ 0:0:0.3D�6N   �� 5t   2   $ border��6A   $@ :(Border.Background).(GradientBrush.GradientStops)[1].Color�5u   A    #A9A9A95v   A   $ 0:0:0.3D�6N   5w   /   5x   +   5y   '   ._�5{   "   �� $��Center=�63   $��Center=�6P   5|      5}      ��5      �� 5�      $	 0,0&�6.   $	 1,0&�6?   -   5        buttonBackgroundStop��.��5�      � $ #EF4444��6+   $ 0Q�6;   � 5�      $ #DC2626��6+   $ 1Q�6;   5�      ��5�      �� 5�      " ��6    # [B��64   .�5�      ��  � ��
 �����6;   �� 5�       �@�Cursor$
 Hand��67    5�      P� 5�   "    � {�$ False��6?   . �5�   &   ��  � �� �#DDDDDD6C   �� 5�   &    �@�Cursor$ Arrow��6?   5�   #   5�      5�      5�      �� 5�      $��3��6   $��Danh sách thiết bị��6%   $
��Bold"�6?   $��0,0,0,10q�6Q   5�      �� 5�   
   $��1��6   
�������$ 10��61   $��20q�6C   $	 10q�6O   ._�5�      &   System.Windows.Controls.DataGrid  -   5          Name$ deviceDataGrid��5�         AutoGenerateColumns$ False��5�         CanUserAddRows$ False��5�         HeadersVisibility$ Column=�5�      
�������
�������$��1q�5�          
IsReadOnly$
  True��5�      !   	RowHeight$! 30Q�5�      $
��Auto=�5�      "   Columns" 5�      0  *System.Windows.Controls.DataGridTextColumn  5�      ,  &System.Windows.Controls.DataGridColumn#   Header$# ID��6)   $   Width5	  /System.Windows.Controls.DataGridLengthConverter$$ 50	 6L   1
  +System.Windows.Controls.DataGridBoundColumn% 
  Binding% 65   �� *ID+6L     5�      $# IP��6)   $	$ 150	 6L   % 65   �� *IP+6L     5�      $	# MAC��6)   $	$ 200	 6N   % 66   �� *MAC+6N     5�      $
# Name��6)   $$ *	 6P   % 67   �� *Name+6P     5�      $
# Date��6)   $	$ 150	 6T   % 67   �� *
ScanDate+6T   4  .System.Windows.Controls.DataGridTemplateColumn  5�      $# Status��6-   $	$ 120	 6=   &   
IsReadOnly$
& True��6I   '   CellTemplate' 5�      �� 5�      .R�5�   "   �� $ 
Horizontal=�6-   $��Center=�6F   $��Center=�6a   .+�5�   &   A� $��14��6.   $��14��69   $��0,0,6,0q�6E   ��5�   .   �� *IsOn+( �� 	Converter#( [ �� 5�   &   $��Center=�60   $
��Bold"�6K   
���   �) �� TextAlignment$) Center=�6p   ��5�   0   �� *IsOn+#( [  5�   #   5�      5�      5�      5�      5�      5�      