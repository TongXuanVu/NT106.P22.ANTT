   M S B A M L   `   `   `  ���� H  DLANSPYproject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:LANSPYprojectLANSPYproject  U QLiveCharts.Wpf, Version=0.9.7.0, Culture=neutral, PublicKeyToken=0bc1f845d1ebb8dfG4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.WpfLiveCharts.Wpf     LANSPYproject.Dashboard   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     -localclr-namespace:LANSPYproject     Dlvc4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.Wpf     ���#F5F7FA.���� $
��Auto=�$��Disabled=�.��� $��20q�$	��600��$	��600��  � RowDefinitions  �� $
��Auto ��� $
��Auto ��� $��* �.+��� $��🧭 Tổng quan mạng��$
��Bold"�$��32(�$��Center=�$��
0,-14,0,35q�$��0��� $��0,45,0,0q�$��2�� � ColumnDefinitions �� $��* ��� $��* �.+��� 
������� �� CornerRadius$ 10�� �� Padding$ 20q�$��	0,0,19,20q�$��0��._��� .+���  �� Orientation$ 
Horizontal=�$	��0,5q�.+��� $
��📶��$��0,0,5,0q��� $��SSID:��$
��Bold"�$��15(��� $��10,0,0,0q�$��15(�$��CharacterEllipsis=����� *
WifiSSID+�� $ 
Horizontal=�$	��0,5q�.+��� $
��📡��$��0,0,5,0q��� $��BSSID:��$
��Bold"�$��15(��� $��10,0,0,0q�$��15(�$��CharacterEllipsis=����� *	WifiBSSID+�� $ 
Horizontal=�$	��0,5q�.+��� $	��⚡��$��0,0,5,0q��� $��Tốc độ kết nối:��$
��Bold"�$��15(��� $��10,0,0,0q�$��15(����� *	WifiSpeed+�� $ 
Horizontal=�$	��0,5q�.+��� $��⬆️��$��0,0,5,0q��� $��Upload:��$
��Bold"�$��15(��� $��10,0,0,0q�$��15(����� *UploadSpeed+�� $ 
Horizontal=�$	��0,5q�.+��� $��⬇️��$��0,0,5,0q��� $��	Download:��$
��Bold"�$��15(��� $��10,0,0,0q�$��15(����� *DownloadSpeed+�� $ 
Horizontal=�$	��0,5q�.+��� $	��⏱��$��0,0,5,0q��� $��Độ trễ:��$
��Bold"�$��15(��� $��10,0,0,0q�$��15(����� *WifiLatency+�� 
�������$ 10��$ 20q�$��0,0,0,20q�$��1��._��� .+��� $ 
Horizontal=�$	��0,5q�.+��� $
��📱��$��0,0,5,0q��� $%��Thiết bị đang kết nối:��$
��Bold"�$��15(��� $��10,0,0,0q�$��15(����� *OnlineDevicesCount+�� $	�� / ��$��15(��� $��10,0,0,0q�$��15(����� *TotalDevicesCount+�� $ 
Horizontal=�$
��0,10q�.+��� $
��👁��$��0,0,5,0q��� $$��Cảnh báo thiết bị lạ:��$
��Bold"�$��15(��� $��10,0,0,0q�$��15(����� *StrangeDeviceAlerts+�� $ 
Horizontal=�$
��0,10q�.+��� $
��🔄��$��0,0,5,0q��� $��Lần quét gần nhất:��$
��Bold"�$��15(��� $��10,0,0,0q�$��15(����� *LastScanTime+�� -   $	��120��$��35��$��Cập nhật��
�������$
��Bold"�$��14(� � Cursor$
 Hand��$��10,20q�����  �� 
StartPoint$	 0,0&� �� EndPoint$	 1,0&�.���  � Color$ #123288��	 � Offset$	 0Q�� $ #295EEC��$	 1Q����� 
 �� 
TargetType"
 ��.���  �� Property � �� �� Value ��  �� 
TargetType" ��.R��� #��z��$ 20��$	 8,4q�._��� $��Center=�$��Center=� �� Triggers P�  P� Property �@J�IsMouseOver P� Value$
 True��. ���  � �� �� $	 0,0&�$	 1,0&�.��� $ #1A47B8��$	 0Q�� $ #4B7BFF��$	 1Q��� 
�������$ 10��$ 20q�$��2��$��2��$��Stretch=�._��� .+��� $*��$📊 Biểu đồ kết nối mạng��$
��Bold"�$��16(�$��Center=��� $��Chú thích: ��$��SemiBold"�$��14(�$��	0,10,0,10q�$��Center=��� $k��e- Biểu đồ hiển thị tốc độ tải lên (Upload) và tốc độ tải xuống (Download).��$��12(�$��Center=��� $O��I- Tốc độ được tính theo đơn vị KB/s (Kilobytes per second).��$��12(�$��Center=�#  LiveCharts.Wpf.CartesianChart  -     Name$ TrafficChart��$	��270��$
��1000��&   LiveCharts.Wpf.Charts.Base.Chart   LegendLocation$ Right=�   Series �� *ChartSeries+   AxisY   LiveCharts.Wpf.Axis     Title$ Tốc độ (KB/s)��   FontSize$ 16Q�   LabelFormatter �� *
YFormatter+   AxisX   $ Thời gian (HH:mm:ss)��$ 16Q� �� *
XFormatter+