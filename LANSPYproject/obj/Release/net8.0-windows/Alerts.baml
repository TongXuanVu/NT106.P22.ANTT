   M S B A M L   `   `   `  ���� H  DLANSPYproject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:LANSPYprojectLANSPYproject      LANSPYproject.Alerts   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     -localclr-namespace:LANSPYproject     ���#F5F7FA.���   � ColumnDefinitions  �� $	��180 �$	��150��$	��250���� $��* �.+��� $
��Auto=�$��0��� $��1�� � RowDefinitions �� $
��Auto ��� $��* �.+��� $��1��$
��Auto=�$
��Auto=�$��-100,0,100,0q�.���� $��Stretch=�$	��Top=�$��20q�.+��� $��CẢNH BÁO MẠNG��$
��Bold"�$��0,0,0,20q�$��Center=�$
��Wrap=�����  �� 
TargetType" ��.���  �� Property � �� �� Value$ 18(� �� Triggers ��  �� Binding ��  �� RelativeSource ��  �� AncestorType
 Window	 �� Path$	 ActualWidth�
 �� Value
 800.a���  � ��$ 16(���  ��  �� 
 Window$	 ActualWidth�
 600.a���  � ��$ 14(���  �� CornerRadius$ 10��
������� �� Padding$ 20q�$	��300��$	��700��$��0,0,0,20q�._�&   System.Windows.Controls.DataGrid  -     Name$ AlertDataGrid��   AutoGenerateColumns$ False��   HeadersVisibility$ Column=�   CanUserAddRows$ False��   CanUserDeleteRows$ False��   CanUserResizeRows$ False��
�������$��0q�   RowBackground
 �����   AlternatingRowBackground �#F9FAFB   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ Thời gian��   Width5  /System.Windows.Controls.DataGridLengthConverter$	 200 1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *
ThoiGian+  $ Loại��$	 200  �� *Loai+  $ 
Nội Dung��$ *  �� *	NoiDung+��  �� Orientation$ 
Horizontal=�$��Center=�$��	0,20,20,0q�.+��� $	��120��$��35��$��Lọc theo loại��
�������$
��Bold"�$��14(� � Cursor$
 Hand��$��0,0,30,0q�����  �� 
StartPoint$	 0,0&� �� EndPoint$	 1,0&�.���  � Color$ #123288�� � Offset$ 0Q�� $ #295EEC��$ 1Q����� " ��.���  � �� ��  �� 
TargetType" ��.R��� #��z��$ 20��$	 8,4q�._��� $��Center=�$��Center=� P�   P� Property  �@J�IsMouseOver! P� Value$
! True��. ���  � �� �� $	 0,0&�$	 1,0&�.��� $ #1A47B8��$ 0Q�� $ #4B7BFF��$ 1Q��� $	��135��$��35��$��Xóa cảnh báo cũ��
�������$
��Bold"�$��14(�$
 Hand��$��0,0,30,0q����� $	 0,0&�$	 1,0&�.��� $ #123288��$ 0Q�� $ #295EEC��$ 1Q����� " ��.���  � �� �� " ��.R��� #��z��$ 20��$	 8,4q�._��� $��Center=�$��Center=� P�   �@J�IsMouseOver$
! True��. ���  � �� �� $	 0,0&�$	 1,0&�.��� $ #4B7BFF��$ 0Q�� $ #709EFF��$ 1Q��� $	��120��$��35��$��Xuất báo cáo��
�������$
��Bold"�$��14(�$
 Hand������ $	 0,0&�$	 1,0&�.��� $ #123288��$ 0Q�� $ #295EEC��$ 1Q����� " ��.���  � �� �� " ��.R��� #��z��$ 20��$	 8,4q�._��� $��Center=�$��Center=� P�   �@J�IsMouseOver$
! True��. ���  � �� �� $	 0,0&�$	 1,0&�.��� $ #1A47B8��$ 0Q�� $ #4B7BFF��$ 1Q�