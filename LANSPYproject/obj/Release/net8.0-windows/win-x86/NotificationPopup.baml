   M S B A M L   `   `   `  ����   LANSPYproject%    LANSPYproject.NotificationPopup   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$  Thông báo��$	��200��$	��400�� � WindowStartupLocation$ CenterOwner=� � 
ResizeMode$ NoResize=� � WindowStyle$
 None=� � AllowsTransparency$
 True��
������  � Topmost$
 True�� � ShowInTaskbar$ False��.���� 
����������#E0E0E0$��1q� �� CornerRadius$ 10�� J� Effect 3  -System.Windows.Media.Effects.DropShadowEffect  	   Color$	 Black��
   	Direction$	
 270Q�   ShadowDepth$ 5Q�   Opacity$	 0.3Q�   
BlurRadius$ 15Q�._��  � RowDefinitions �� $��50 ��� $��* ��� $��50 �.+��� -   $��HeaderBorder��$��0�����#4CAF50$ 	10,10,0,0��._��  � ColumnDefinitions �� $
��Auto ��� $��* ��� $
��Auto �.+�A� -   $��IconEllipse��$��0��$��30��$��30��$��15,0,0,0q�����  �� Color$ White���� -   $��IconText��$��0��$��!��$��18(�$
��Bold"����#4CAF50$��Center=�$��Center=�$��15,0,0,0q��� -   $��	TitleText��$��1��$��Thông báo thiết bị��$��16(�$
��Bold"�
�������$��Center=�$��15,0,0,0q��� -   $��CloseButton��$��2��$��×��$��20(�$
��Bold"�
�������
������ $��0q�$��30��$��30��$��0,0,10,0q� � Cursor$
 Hand������  �� 
TargetType" ��.R��� #��z��$ 15��._��� $��Center=�$��Center=�����  �� 
TargetType" ��.���  �� Property � �� �� Value
 ����  �� Triggers P�  P� Property �@J�IsMouseOver P� Value$
 True��. ���  � �� �	#80FFFFFF� $��1��$��20q� �� $
��Auto ��� $
��Auto ��� $
��Auto �.+��� -   $��DeviceInfoText��$��0��$��14(�$
��Bold"�$��0,0,0,5q��� -   $��IPAddressText��$��1��$��12(����#666666$��0,0,0,5q��� -   $��TimestampText��$��2��$��11(����#999999�� $��2�����#F5F5F5$ 	0,0,10,10��._���  �� Orientation$ 
Horizontal=�$��Right=�$��10q�.+��� -	   $��Đóng��$��80��$��30�����#E0E0E0$��0q�$
 Hand��