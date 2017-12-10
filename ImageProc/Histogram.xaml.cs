using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ImageProc
{
    /// <summary>
    /// Interaction logic for Histogram.xaml
    /// </summary>
    public partial class Histogram : Window
    {
        HistogramVM hvm;

        public Histogram(Bitmap gray, Bitmap blue, Bitmap green, Bitmap red)
        {
            InitializeComponent();
            hvm = new HistogramVM(gray, blue, green, red);
            this.DataContext = hvm;
        }

        //private void Window_Loaded(object sender, RoutedEventArgs e)
        //{
        //    float maxH = 0;
        //    for (int i = 0; i < histogram.Length; i++)
        //    {
        //        if (histogram[i] > maxH)
        //        {
        //            maxH = histogram[i];
        //        }
        //    }
        //    MessageBox.Show(maxH.ToString());


        //    int histHeight = 200;
        //    Bitmap img = new Bitmap(256, histHeight + 10);
        //    using (Graphics g = Graphics.FromImage(img))
        //    {
        //        for (int i = 0; i < histogram.Length; i++)
        //        {
        //            float pct = histogram[i] / maxH;   // What percentage of the max is this value?
        //            g.DrawLine(Pens.Red,
        //                new System.Drawing.Point(i, img.Height - 5),
        //                new System.Drawing.Point(i, img.Height - 5 - (int)(pct * histHeight))  // Use that percentage of the height
        //                );
        //        }
        //    }
        //    img.Save("test.jpg");
        //}
    }
}
