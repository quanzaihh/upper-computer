   M S B A M L   `   `   `  ���� C  ?NEW_DEMO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:NEW_DEMONEW_DEMO  U QSystem.Runtime, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a7,clr-namespace:System;assembly=System.RuntimeSystem O8clr-namespace:System.Collections;assembly=System.RuntimeSystem.Collections     NEW_DEMO.MainWindow   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QScottPlot.WPF, Version=4.1.61.0, Culture=neutral, PublicKeyToken=e53b06131e34a3aae aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     S OHandyControl, Version=3.4.0.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b0hc'https://handyorg.github.io/handycontrol  "localclr-namespace:NEW_DEMO  6sys,clr-namespace:System;assembly=System.Runtime  LsysCollection8clr-namespace:System.Collections;assembly=System.Runtime    � Title$  
MainWindow��$	��800��$
��1080�� � 	Resources  �� MergedDictionaries �� Source��  �� $K Epack://application:,,,/HandyControl;component/Themes/SkinDefault.xaml?��� $E ?pack://application:,,,/HandyControl;component/Themes/Theme.xaml?�%    .���  � RowDefinitions �� $��1* ��� $	��20* �.+�� $��0�����#D3D3D3� $��1�� � ColumnDefinitions �� $��1* ��� $��4* �.+�� $��0��
�����_�.+���  �� Orientation$ Vertical=�.+��� $ 
Horizontal=�.+��� $��
串口号:��$��20(�$��宋体)�$��	10,5,10,0q�$��Center=��� -   $��Serial_port��$	��100��$��Stretch=�$��25��$��	10,5,10,0q��� $ 
Horizontal=�.+��� $��
波特率:��$��20(�$��宋体)�$��	10,5,10,0q�$��Center=��� -   $��Bondrate��$	��100��$��Stretch=�$��25��$��	10,5,10,0q� �� SelectedIndex$ 2������    	Boundrate �� Source# [  �� $ 
Horizontal=�.+��� $��
数据位:��$��20(�$��宋体)�$��	10,5,10,0q�$��Center=��� -   $��Databit��$	��100��$��Stretch=�$��25��$��	10,5,10,0q�$ 3������   Databit# [ �� $ 
Horizontal=�.+��� $��
检验位:��$��20(�$��宋体)�$��	10,5,10,0q�$��Center=��� -   $��Checkbit��$	��100��$��Stretch=�$��25��$��	10,5,10,0q�$ 0������   Checkbit# [ �� $ 
Horizontal=�.+��� $��
停止位:��$��20(�$��宋体)�$��	10,5,10,0q�$��Center=��� -   $��Stopbit��$	��100��$��Stretch=�$��25��$��	10,5,10,0q�$ 0������   Stopbit# [ �� -   	 ��Name$	 Connect_serial��$��连接串口��$	��185��$��35��$��1,5,5,5q�$��18(�$��宋体)����#D3D3D3�� $	��190��$��1��$��Center=����#D3D3D3�� -   $	 Channel1��$��	打开LD1��$	��150��$��35��$��1,20,5,5q�$��18(�$��宋体)����#D3D3D3�� -   $	 Channel2��$��	打开LD2��$	��150��$��35��$��1,10,5,5q�$��18(�$��宋体)����#D3D3D3�� -	   $	 Channel3��$��	打开LD3��$	��150��$��35��$��1,10,5,5q�$��18(�$��宋体)����#D3D3D3�� -
   $	 Channel4��$��	打开LD4��$	��150��$��35��$��1,10,5,5q�$��18(�$��宋体)����#D3D3D3� $��1��
������� �� $��2* ��� $��4* �.+�� $��0�� �� $��2* ��� $��1* �.+��� $ Vertical=�$��0��$��Center=�.+��� $ 
Horizontal=�$
��Left=�.+��� $��通道��$��18(�$��宋体)�$��	20,10,0,0q�$��Center=�$
��Bold"��� $��实际温度��$��18(�$��宋体)�$��	25,10,0,0q�$��Center=�$
��Bold"��� $��/��$��18(�$��宋体)�$��0,10,0,0q�$��Center=�$
��Bold"��� $��预设温度��$��18(�$��宋体)�$��0,10,0,0q�$��Center=�$
��Bold"��� $��实际电流��$��18(�$��宋体)�$��	20,10,0,0q�$��Center=�$
��Bold"��� $��/��$��18(�$��宋体)�$��0,10,0,0q�$��Center=�$
��Bold"��� $��预设电流��$��18(�$��宋体)�$��0,10,0,0q�$��Center=�$
��Bold"��� $��  设置电压��$��18(�$��宋体)�$��0,10,0,0q�$��Center=�$
��Bold"��� $ 
Horizontal=�$
��Left=�.+��� $
��LD1:��$��18(�$��Times New Roman)�$��	20,10,0,0q�$��Center=��� -   
 ��Name$
 
LD1_acTemp��$��default��$��18(�$��Times New Roman)�$��	35,10,0,0q�$��Center=�$��Right=��� $��/��$��25(�$��Times New Roman)�$��	10,10,0,0q�$��Center=�$
��Bold"��� -    ��Name$ LD1_setTemp��$��default��$��18(�$��Times New Roman)�$��	10,10,0,0q�$��Center=�$��Center=�
�������$��65���� -   $
 LD1_acCurrent��$��default��$��18(�$��Times New Roman)�$��	30,10,0,0q�$��Center=�$��Right=��� $��/��$��25(�$��Times New Roman)�$��	10,10,0,0q�$��Center=�$
��Bold"��� -   $ LD1_setCurrent��$��default��$��18(�$��Times New Roman)�$��	10,10,0,0q�$��Center=�$��Center=�
�������$��65���� $�� ��$��25(�$��15��$��Times New Roman)�$��	10,10,0,0q�$��Center=�$
��Bold"��� -   $ 
LD1_setVol��$��default��$��18(�$��Times New Roman)�$��	10,10,0,0q�$��Center=�$��Center=�
�������$��65���� $ 
Horizontal=�$
��Left=�.+��� $
��LD2:��$��18(�$��Times New Roman)�$��	20,10,0,0q�$��Center=��� -   $
 
LD2_acTemp��$��default��$��18(�$��Times New Roman)�$��	35,10,0,0q�$��Center=�$��Right=��� $��/��$��25(�$��Times New Roman)�$��	10,10,0,0q�$��Center=�$
��Bold"��� -   $ LD2_setTemp��$��default��$��18(�$��Times New Roman)�$��	10,10,0,0q�$��Center=�$��Center=�
�������$��65���� -   $
 LD2_acCurrent��$��default��$��18(�$��Times New Roman)�$��	30,10,0,0q�$��Center=�$��Right=��� $��/��$��25(�$��Times New Roman)�$��	10,10,0,0q�$��Center=�$
��Bold"��� -   $ LD2_setCurrent��$��default��$��18(�$��Times New Roman)�$��	10,10,0,0q�$��Center=�$��Center=�
�������$��65���� $�� ��$��25(�$��15��$��Times New Roman)�$��	10,10,0,0q�$��Center=�$
��Bold"��� -   $ 
LD2_setVol��$��default��$��18(�$��Times New Roman)�$��	10,10,0,0q�$��Center=�$��Center=�
�������$��65���� $ 
Horizontal=�$
��Left=�.+��� $
��LD3:��$��18(�$��Times New Roman)�$��	20,10,0,0q�$��Center=��� -   $
 
LD3_acTemp��$��default��$��18(�$��Times New Roman)�$��	35,10,0,0q�$��Center=�$��Right=��� $��/��$��25(�$��Times New Roman)�$��	10,10,0,0q�$��Center=�$
��Bold"��� -   $ LD3_setTemp��$��default��$��18(�$��Times New Roman)�$��	10,10,0,0q�$��Center=�$��Center=�
�������$��65���� -   $
 LD3_acCurrent��$��default��$��18(�$��Times New Roman)�$��	30,10,0,0q�$��Center=�$��Center=��� $��/��$��25(�$��Times New Roman)�$��	10,10,0,0q�$��Center=�$
��Bold"��� -   $ LD3_setCurrent��$��default��$��18(�$��Times New Roman)�$��	10,10,0,0q�$��Center=�$��Center=�
�������$��65���� $�� ��$��25(�$��15��$��Times New Roman)�$��	10,10,0,0q�$��Center=�$
��Bold"��� -   $ 
LD3_setVol��$��default��$��18(�$��Times New Roman)�$��	10,10,0,0q�$��Center=�$��Center=�
�������$��65���� $ 
Horizontal=�$
��Left=�.+��� $
��LD4:��$��18(�$��Times New Roman)�$��	20,10,0,0q�$��Center=��� -   $
 
LD4_acTemp��$��default��$��18(�$��Times New Roman)�$��	35,10,0,0q�$��Center=�$��Right=��� $��/��$��25(�$��Times New Roman)�$��	10,10,0,0q�$��Center=�$
��Bold"��� -   $ LD4_setTemp��$��default��$��18(�$��Times New Roman)�$��	10,10,0,0q�$��Center=�$��Center=�
�������$��65���� -   $
 LD4_acCurrent��$��default��$��18(�$��Times New Roman)�$��	30,10,0,0q�$��Center=�$��Right=��� $��/��$��25(�$��Times New Roman)�$��	10,10,0,0q�$��Center=�$
��Bold"��� -   $ LD4_setCurrent��$��default��$��18(�$��Times New Roman)�$��	10,10,0,0q�$��Center=�$��Center=�
�������$��65���� $�� ��$��25(�$��15��$��Times New Roman)�$��	10,10,0,0q�$��Center=�$
��Bold"��� -   $ 
LD4_setVol��$��default��$��18(�$��Times New Roman)�$��	10,10,0,0q�$��Center=�$��Center=�
�������$��65���� $ Vertical=�$��1��$��Center=�.+��� $ 
Horizontal=�$��Center=�.+��� $��P/I/D��$��20(�$��Times New Roman)�$��	10,10,0,0q�$��Center=��� -   $ set_P��$��default��$��18(�$��Times New Roman)�$��	10,10,0,0q�$��Center=�
�������$��65���� -    $ set_I��$��default��$��18(�$��Times New Roman)�$��	10,10,0,0q�$��Center=�
�������$��65���� -!   $ set_D��$��default��$��18(�$��Times New Roman)�$��	10,10,0,0q�$��Center=�
�������$��65��  ScottPlot.WpfPlot  -"   $��	Main_plot��$��1��$��Stretch=�$��0,0,0,0q�$��Stretch=�$
��auto��
�������