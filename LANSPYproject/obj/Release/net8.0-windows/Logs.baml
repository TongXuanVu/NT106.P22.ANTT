   M S B A M L   `   `   `  ����   LANSPYproject    LANSPYproject.Logs   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     ���#F5F7FA.��� .+�� $��1��  � RowDefinitions  �� $
��Auto ��� $��* �.+��� $��1��$��20q�$
��Auto=�.���� .+��� 
������� �� CornerRadius$ 10�� �� Padding$ 20q�$��0,0,0,20q�._��� .+��� $��Tìm kiếm��$��18(�$
��Bold"����#1E3A8A$��0,0,0,10q�� $��0,10,0,0q� � ColumnDefinitions �� $��* ��� $��* �  �� $
��Auto ��� $
��Auto ��� $
��Auto �.+��� $��
IP Address��$��0��$��0��$	��0,5q��� -    ��Name$ 	IPTextBox��$��192.168.1.10��$��0��$��1��$
��10,5q��� $��MAC Address��$��1��$��0��$	��0,5q��� -   $ 	MACTexBox��$��AA-BB-CC-DD-EE-FF��$��1��$��1��$
��10,5q��� $��Thiết bị��$��2��$��0��$	��0,5q��� -   $ DeviceTextBox��$��Laptop��$��2��$��1��$
��10,5q���  �� Orientation$ 
Horizontal=�$
��Left=�$
��0,10q�.+��� $��0,0,20,0q�.+��� $��Thời gian từ��$��0,0,0,5q�(  "System.Windows.Controls.DatePicker  -     Name$ FromDatePicker��   SelectedDate 
2025-02-10�� .+��� $��Đến��$��0,0,0,5q�  -   $ ToDatePicker�� 
2025-02-14�� $ 
Horizontal=�$��0,10,0,0q�.+��� -   $	��120��$��35��$��Tìm kiếm��
�������$
��Bold"�$��14(� � Cursor$
 Hand��$��0,0,20,0q����� 	 �� 
StartPoint$		 0,0&�
 �� EndPoint$	
 1,0&�.���  � Color$ #123288�� � Offset$ 0Q�� $ #295EEC��$ 1Q�����  �� 
TargetType" ��.���  �� Property � �� �� Value ��  �� 
TargetType" ��.R��� #��z��$ 20��$	 8,4q�._��� $��Center=�$��Center=� �� Triggers P�  P� Property �@J�IsMouseOver P� Value$
 True��. ���  � �� �� $		 0,0&�$	
 1,0&�.��� $ #1A47B8��$ 0Q�� $ #4B7BFF��$ 1Q��� -   $	��120��$��35��$��
Làm mới��
�������$
��Bold"�$��14(�$
 Hand������ $		 0,0&�$	
 1,0&�.��� $ #123288��$ 0Q�� $ #295EEC��$ 1Q����� " ��.���  � �� �� " ��.R��� #��z��$ 20��$	 8,4q�._��� $��Center=�$��Center=� P�  �@J�IsMouseOver$
 True��. ���  � �� �� $		 0,0&�$	
 1,0&�.��� $ #1A47B8��$ 0Q�� $ #4B7BFF��$ 1Q��� 
�������$ 10��$ 10q�._��   �� $
��Auto ��� $��* ��� $
��Auto �.+��� $ 
Horizontal=�$��0��.+��� $'��!Lịch sử hoạt động mạng��$��18(�$
��Bold"����#1E3A8A�� $ 
Horizontal=�$��Right=�.+��� -    ��Name$ TotalLogsTextBlock��$��Tổng số log: 0��$��	10,7,10,0q��� -	   $ StrangeDevicesTextBlock��$��Thiết bị lạ: 0��$��	10,7,10,0q�&   System.Windows.Controls.DataGrid  -
     Name$ LogsDataGrid��   AutoGenerateColumns$ False��   CanUserAddRows$ False��   
IsReadOnly$
 True��$��0,10,0,0q�$��1��$
��Auto=�   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ SL No��   Width5  /System.Windows.Controls.DataGridLengthConverter$ 50 1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *ID+  $ MAC Address��$	 200  �� *MAC+  $ 
IP Address��$	 150  �� *IP+  $ Tên thiết bị��$	 200  �� *Name+  $ Thời gian��$	 150  �� *
ScanDate+  $ Ghi chú��$	 100  �� *
WifiName+�� $ 
Horizontal=�$��Right=�$��0,10,0,0q�$��2��.+��� -   $	��120��$��35��$��Xuất báo cáo��
�������$
��Bold"�$��14(�$
 Hand��$��0,0,20,0q����� $		 0,0&�$	
 1,0&�.��� $ #123288��$ 0Q�� $ #295EEC��$ 1Q����� " ��.���  � �� �� " ��.R��� #��z��$ 20��$	 8,4q�._��� $��Center=�$��Center=� P�  �@J�IsMouseOver$
 True��. ���  � �� �� $		 0,0&�$	
 1,0&�.��� $ #1A47B8��$ 0Q�� $ #4B7BFF��$ 1Q��� -   $	��180��$��35��$��Xoá log cũ (> 7 ngày)��
�������$
��Bold"�$��14(�$
 Hand��$��0,0,20,0q����� $		 0,0&�$	
 1,0&�.��� $ #123288��$ 0Q�� $ #295EEC��$ 1Q����� " ��.���  � �� �� " ��.R��� #��z��$ 20��$	 8,4q�._��� $��Center=�$��Center=� P�  �@J�IsMouseOver$
 True��. ���  � �� �� $		 0,0&�$	
 1,0&�.��� $ #1A47B8��$ 0Q�� $ #4B7BFF��$ 1Q�