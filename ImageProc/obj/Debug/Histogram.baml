   M S B A M L   `   `   `  ����D  @ImageProc, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:ImageProc	ImageProc  5   	       ImageProc.Histogram   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:ImageProc     5   	     � Title$  	Histogram��5   	   $	��500��6   $	��400��6(    � 	Resources 5
      -  'ImageProc.BitmapToBitmapSourceConverter   bitmapConverter��%   &	          5   
   6H   5      .��5      �  � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   .+�5   
   �� $��0��6   $��Center=�6f   $��Center=�6�   $��Uniform=�6�   ��6   ��  �� Path$
 Gray� �� 	Converter# [  6�   �� 5   
   $��1��6   $��Center=�6e   $��Center=�6�   $��Uniform=�6�   ��6   �� $	 Red�# [  6�   �� 5   
   $��2��6   $��Center=�6g   $��Center=�6�   $��Uniform=�6�   ��6   �� $ Green�# [  6�   �� 5   
   $��3��6   $��Center=�6f   $��Center=�6�   $��Uniform=�6�   ��6   �� $
 Blue�# [  6�   5      5      