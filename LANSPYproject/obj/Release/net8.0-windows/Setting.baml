   M S B A M L   `   `   `  ���� H  DLANSPYproject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:LANSPYprojectLANSPYproject      LANSPYproject.Setting   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     -localclr-namespace:LANSPYproject     $	��450��$	��800��  � 	Resources     ModernToggleButtonStyle �� 
TargetType �� Property �� Value �� 
TargetType ��Name �� CornerRadius A�Name �� Triggers	 P� Property
 P� Value �� 
TargetName  RoundedButtonStyle��%�  &	        &	 t    �� " d�.���  � ��$ 50����  � ��$ 25����  � �� �� " d�.R�� .+��� $ SwitchBorder��$
 12.5��
�������$��50��$��25��A� $ 
SwitchKnob��$��21��$��21��$��2q�
�������$
��Left=�$��Center=� P� 	 �@d�	IsChecked$

 TrueY�. ��� $ SwitchBorder�� � �� �#16DBCC�� $ 
SwitchKnob�� � ��$ Right=��� " ��.���  � �� �� " ��.R��� #��z��$ 22��#��z��#��z��._��� $��Center=�$��Center=�.���  � ColumnDefinitions �� $	��140 ��� $��* �.+�� $��1�� � RowDefinitions �� $
��Auto ��� $��* �.+��� $��2��$��
1,10,40,20q�.+��� $��CÀI ĐẶT��$��18(�$
��Bold"����#334155$��0,0,0,10q��� 
�������$ 16�� �� Padding$ 30q�$	��880��$
��Left=�._��� .+���  �� Orientation$ 
Horizontal=�$��Center=�$	��0,5q�.+�d� -    d�Name$ ToggleStartup��#��[   d� 	IsChecked$
 TrueY�$��0,0,10,0q��� $"��Khởi động cùng Windows��$��Center=��� $ 
Horizontal=�$��Center=�$	��0,5q�.+�d� -   $ ToggleNewDevice��#��[  $��0,0,10,0q��� $;��5Gửi cảnh báo khi phát hiện thiết bị mới��$��Center=��� $ 
Horizontal=�$��Center=�$	��0,5q�.+�d� -   $ ToggleDisconnect��#��[  $
 TrueY�$��0,0,10,0q��� $:��4Gửi cảnh báo khi thiết bị mất kết nối��$��Center=��� $ 
Horizontal=�$��Center=�$	��0,5q�.+�d� -   $ ToggleUnknownMAC��#��[  $
 TrueY�$��0,0,10,0q��� $A��;Gửi cảnh báo khi có thiết bị lạ (MAC chưa rõ)��$��Center=��� $ 
Horizontal=�$��Center=�$��0,15,0,5q�.+��� $7��1Ngưỡng cảnh báo số lượng thiết bị:��$��Center=��� -    ��Name$ DeviceThreshold��$��60��$��10,0,0,0q�$��50��$��Center=��� $ 
Horizontal=�$��Center=�$��0,5,0,10q�.+��� $&�� Thời gian quét lại (giây):��$��Center=��� -   $ ScanInterval��$��60��$��10,0,0,0q�$	��300��$��Center=��� $ 
Horizontal=�$��Center=�$��0,10,0,0q� � Cursor$
 Hand��.+��� -   $��Lưu cài đặt��$	��150��$��40��#��[ 
�������$
��Bold"�$��0,0,20,0q���Z� ����  �� 
StartPoint$	 0,0&� �� EndPoint$	 1,0&�.���  � Color$ #123288�� � Offset$ 0Q�� $ #295EEC��$ 1Q��� -   $ ��Khôi phục mặc định��$	��150��$��40��#��[ 
�������$
��Bold"���Z� ���� $	 0,0&�$	 1,0&�.��� $ #123288��$ 0Q�� $ #295EEC��$ 1Q�