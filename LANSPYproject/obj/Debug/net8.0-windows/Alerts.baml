   M S B A M L   `   `   `  ����H  DLANSPYproject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:LANSPYprojectLANSPYproject  5          LANSPYproject.Alerts   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      -localclr-namespace:LANSPYproject     5      ���#F5F7FA.��5      �   � ColumnDefinitions  5	   
   �� 5
      $	��180 �6   $	��150��6+   $	��250��6:   �� 5      $��* �6   .+�5   
   �� $
��Auto=�6   $��0��6:   � 5   
   $��1��6    � RowDefinitions 5      �� 5      $
��Auto �6    �� 5      $��* �6    .+�5      �� $��1��6   $
��Auto=�6(   $
��Auto=�6K   $��-100,0,100,0q�6p   .��5      �� $��Stretch=�6   $	��Top=�6;   $��20q�6S   .+�5      �� $��CẢNH BÁO MẠNG��6    $
��Bold"�5       $��0,0,0,20q�5       $��Center=�5       $
��Wrap=�5       ��5      �� 5       �� 
TargetType" ��6$   .�5   "   ��  �� Property � �� �� Value$ 18(�6=    �� Triggers 5    "   �� 5!   &    �� Binding 62   ��  �� RelativeSource ��  �� AncestorType
 Window	 �� Path$	 ActualWidth�
 �� Value
 8006�   .a�5"   *   ��  � ��$ 16(�6E   5#   '   �� 5$   &    62   ��  �� 
 Window$	 ActualWidth�
 6006�   .a�5%   *   ��  � ��$ 14(�6E   5&   '   5(      5*      �� 5,       �� CornerRadius$ 10��6   
������� �� Padding$ 20q�6B   $	��300��6O   $	��700��6^   $��0,0,0,20q�6m   ._�5-      &   System.Windows.Controls.DataGrid  -   5          Name$ AlertDataGrid��5-   #      AutoGenerateColumns$ False��6:      HeadersVisibility$ Column=�6V      CanUserAddRows$ False��5.   #      CanUserDeleteRows$ False��6:      CanUserResizeRows$ False��6T   
�������$��0q�5/   6      RowBackground
 �����   AlternatingRowBackground �#F9FAFB   Columns 50      0  *System.Windows.Controls.DataGridTextColumn  51   "   ,  &System.Windows.Controls.DataGridColumn   Header$ Thời gian��65      Width5  /System.Windows.Controls.DataGridLengthConverter$	 200 6e   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6H   �� *
ThoiGian+6e     52   "   $ Loại��65   $	 200 6\    6C   �� *Loai+6\     53   "   $ 
Nội Dung��65   $ * 6c    6G   �� *	NoiDung+6c   55      56      �� 59       �� Orientation$ 
Horizontal=�6!   $��Center=�6:   $��0,20,0,0q�6W   $	��Top=�6i   .+�5;      �� -   5         ��Name$ DeleteOldAlertsButton��5;   !   $	��135��6@   $��35��6L   $��Xóa cảnh báo cũ��6X   
�������$
��Bold"�6�   $��14(�6�    � Cursor$
 Hand��6�   $��0,0,30,0q�6�   ��5=      �� 5>   "    �� 
StartPoint$	 0,0&�66    �� EndPoint$	 1,0&�6G   .��5?   &   �  � Color$ #123288��63    � Offset$ 0Q�6C   � 5@   &   $ #295EEC��63   $ 1Q�6C   5A   #   ��5C      �� 5D   "   " ��6(   .�5E   &   ��  � �� 5F   *   �� 5G   .     �� 
TargetType"  ��6>   .R�5H   2   �� #��z��69   $ 20��6c   $	 8,4q�6u   ._�5I   6   �� $��Center=�6G   $��Center=�6d   5J   3   5K   /   5M   '    5N   &   P� 5O   *   ! P� Property! �@J�IsMouseOver" P� Value$
" True��6I   . �5P   .   ��  � �� 5Q   2   �� 5R   6   $	 0,0&�6J   $	 1,0&�6[   .��5S   :   � $ #4B7BFF��6G   $ 0Q�6W   � 5T   :   $ #709EFF��6G   $ 1Q�6W   5U   7   5W   /   5X   +   5Z   #   5\      �� 5^      -   5        $ ExportAlertsButton��5^   !   $	��120��6=   $��35��6I   $��Xuất báo cáo��6U   
�������$
��Bold"�6   $��14(�6�   $
 Hand��6�   ��5`      �� 5a   "   $	 0,0&�66   $	 1,0&�6G   .��5b   &   � $ #123288��63   $ 0Q�6C   � 5c   &   $ #295EEC��63   $ 1Q�6C   5d   #   ��5f      �� 5g   "   " ��6(   .�5h   &   ��  � �� 5i   *   �� 5j   .   "  ��6>   .R�5k   2   �� #��z��69   $ 20��6c   $	 8,4q�6u   ._�5l   6   �� $��Center=�6G   $��Center=�6d   5m   3   5n   /   5p   '    5q   &   P� 5r   *   ! �@J�IsMouseOver$
" True��6I   . �5s   .   ��  � �� 5t   2   �� 5u   6   $	 0,0&�6J   $	 1,0&�6[   .��5v   :   � $ #1A47B8��6G   $ 0Q�6W   � 5w   :   $ #4B7BFF��6G   $ 1Q�6W   5x   7   5z   /   5{   +   5}   #   5      5�      5�      5�      5�      5�      5�      