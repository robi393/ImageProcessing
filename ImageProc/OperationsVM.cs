using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ImageProc
{
    class OperationsVM : ViewModelBase
    {
        Stopwatch sw;

        private Bitmap sourceImage;

        public Bitmap SourceImage
        {
            get { return sourceImage; }
            set { Set(ref this.sourceImage, value); }
        }

        private string filename;

        private string elapsedTimeMessage;

        public string ElapsedTimeMessage
        {
            get { return elapsedTimeMessage; }
            set { Set(ref this.elapsedTimeMessage, value); }
        }

        public OperationsVM()
        {
            filename = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "/Resources/default.jpg";
            sourceImage = new Bitmap(Image.FromFile(filename));
            sw = new Stopwatch();

            this.FileBrowser = new RelayCommand(FileBrowserMethod);
            this.RestoreImage = new RelayCommand(RestoreImageMethod);
            this.Grayscale = new RelayCommand(GrayscaleMethod);
            this.Invert = new RelayCommand(InvertMethod);
            this.Treshold = new RelayCommand(TresholdMethod);
            this.ContrastStretch = new RelayCommand(ContrastStretchMethod);
            this.LogaritmicScale = new RelayCommand(LogaritmicScaleMethod);
            this.MeanFilter = new RelayCommand(MeanFilterMethod);
            this.GaussFilter = new RelayCommand(GaussFilterMethod);
            this.MedianFilter = new RelayCommand(MedianFilterMethod);
            this.LaplaceFilter = new RelayCommand(LaplaceFilterMethod);
            this.SobelEdgeDetector = new RelayCommand(SobelEdgeDetectorMethod);
            this.Harris = new RelayCommand(HarrisMethod);
            this.CreateHistogram = new RelayCommand(CreateHistogramMethod);
            this.HistogramEqualization = new RelayCommand(HistogramEqualizationMethod);
        }

        public ICommand FileBrowser { get; private set; }
        public ICommand RestoreImage { get; private set; }
        public ICommand Grayscale { get; private set; }
        public ICommand Invert { get; private set; }
        public ICommand Treshold { get; private set; }
        public ICommand ContrastStretch { get; private set; }
        public ICommand LogaritmicScale { get; private set; }
        public ICommand MeanFilter { get; private set; }
        public ICommand GaussFilter { get; private set; }
        public ICommand MedianFilter { get; private set; }
        public ICommand LaplaceFilter { get; private set; }
        public ICommand SobelEdgeDetector { get; private set; }
        public ICommand Harris { get; private set; }
        public ICommand CreateHistogram { get; private set; }
        public ICommand HistogramEqualization { get; private set; }


        private void FileBrowserMethod()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            ofd.DefaultExt = ".jpg";
            ofd.Filter = ".jpg|*.jpg|.jpeg|*.jpeg|.png|*.png";
            Nullable<bool> result = ofd.ShowDialog();

            if (result == true)
            {
                filename = ofd.FileName;
                this.SourceImage = new Bitmap(Image.FromFile(filename));
            }
        }

        private void RestoreImageMethod()
        {
            SourceImage = new Bitmap(Image.FromFile(filename));
        }

        private void GrayscaleMethod()
        {
            sw.Restart();
            this.SourceImage = Operations.Grayscale((Bitmap)SourceImage.Clone());
            sw.Stop();

            this.ElapsedTimeMessage = string.Format("A művelet időtartama: {0} ms.", sw.ElapsedMilliseconds);
        }

        private void InvertMethod()
        {
            sw.Restart();
            this.SourceImage = Operations.Invert((Bitmap)SourceImage.Clone());
            sw.Stop();

            this.ElapsedTimeMessage = string.Format("A művelet időtartama: {0} ms.", sw.ElapsedMilliseconds);
        }

        private void TresholdMethod()
        {
            sw.Restart();
            this.SourceImage = Operations.Treshold((Bitmap)SourceImage.Clone(), 120);
            sw.Stop();

            this.ElapsedTimeMessage = string.Format("A művelet időtartama: {0} ms.", sw.ElapsedMilliseconds);
        }

        private void ContrastStretchMethod()
        {
            sw.Restart();
            this.SourceImage = Operations.ContrastStretch((Bitmap)SourceImage.Clone(), 30, 225);
            sw.Stop();
            this.ElapsedTimeMessage = string.Format("A művelet időtartama: {0} ms.", sw.ElapsedMilliseconds);
        }

        private void LogaritmicScaleMethod()
        {
            sw.Restart();
            this.SourceImage = Operations.LogaritmicScale((Bitmap)SourceImage.Clone());
            sw.Stop();

            this.ElapsedTimeMessage = string.Format("A művelet időtartama: {0} ms.", sw.ElapsedMilliseconds);
        }

        private void MeanFilterMethod()
        {
            sw.Restart();
            this.SourceImage = Operations.MeanFilter((Bitmap)SourceImage.Clone());
            sw.Stop();

            this.ElapsedTimeMessage = string.Format("A művelet időtartama: {0} ms.", sw.ElapsedMilliseconds);
        }

        private void GaussFilterMethod()
        {
            sw.Restart();
            this.SourceImage = Operations.GaussianFilter((Bitmap)SourceImage.Clone());
            sw.Stop();

            this.ElapsedTimeMessage = string.Format("A művelet időtartama: {0} ms.", sw.ElapsedMilliseconds);
        }

        private void MedianFilterMethod()
        {
            sw.Restart();
            this.SourceImage = Operations.MedianFilter((Bitmap)SourceImage.Clone());
            sw.Stop();

            this.ElapsedTimeMessage = string.Format("A művelet időtartama: {0} ms.", sw.ElapsedMilliseconds);
        }

        private void LaplaceFilterMethod()
        {
            this.SourceImage = Operations.Grayscale((Bitmap)SourceImage.Clone());
            sw.Restart();
            this.SourceImage = Operations.LaplaceFilter((Bitmap)SourceImage.Clone());
            sw.Stop();

            this.ElapsedTimeMessage = string.Format("A művelet időtartama: {0} ms.", sw.ElapsedMilliseconds);
        }

        private void SobelEdgeDetectorMethod()
        {
            this.SourceImage = Operations.Grayscale((Bitmap)SourceImage.Clone());
            sw.Restart();
            this.SourceImage = Operations.SobelEdgeDetector((Bitmap)SourceImage.Clone());
            sw.Stop();

            this.ElapsedTimeMessage = string.Format("A művelet időtartama: {0} ms.", sw.ElapsedMilliseconds);
        }

        private void HarrisMethod()
        {
            this.SourceImage = Operations.Grayscale((Bitmap)SourceImage.Clone());
            sw.Restart();
            this.SourceImage = Operations.HarrisCornerDetector((Bitmap)SourceImage.Clone());
            //this.SourceImage = Operations.Treshold((Bitmap)SourceImage.Clone(),135);
            sw.Stop();

            this.ElapsedTimeMessage = string.Format("A művelet időtartama: {0} ms.", sw.ElapsedMilliseconds);
        }

        private void CreateHistogramMethod()
        {
            int[,] histogramRGB = Operations.CreateHistogramRGB((Bitmap)SourceImage.Clone());
            int[] histogramGS = Operations.CreateHistogramGray(Operations.Grayscale((Bitmap)SourceImage.Clone()));

            float maxBlue = 0;
            float maxRed = 0;
            float maxGreen = 0;
            float maxGrey = 0;

            for (int i = 0; i < 256; i++)
            {
                if (histogramRGB[0, i] > maxBlue)
                {
                    maxBlue = histogramRGB[0, i];
                }
                if (histogramRGB[1, i] > maxGreen)
                {
                    maxGreen = histogramRGB[1, i];
                }
                if (histogramRGB[2, i] > maxRed)
                {
                    maxRed = histogramRGB[2, i];
                }
                if (histogramGS[i] > maxGrey)
                {
                    maxGrey = histogramGS[i];
                }
            }

            int imageHeight = 150;

            Bitmap imgGrey = new Bitmap(512, imageHeight + 10);
            Bitmap imgBlue = new Bitmap(512, imageHeight + 10);
            Bitmap imgGreen = new Bitmap(512, imageHeight + 10);
            Bitmap imgRed = new Bitmap(512, imageHeight + 10);

            using (Graphics gr = Graphics.FromImage(imgGrey))
            {
                gr.Clear(Color.White);
                for (int i = 0; i < 256; i++)
                {
                    float percentage = histogramGS[i] / maxGrey;
                    gr.DrawLine(Pens.Gray, new System.Drawing.Point(2 * i, imgGrey.Height - 10), new System.Drawing.Point(2 * i, imgGrey.Height - 10 - (int)(percentage * imageHeight)));
                    gr.DrawLine(Pens.Gray, new System.Drawing.Point(2 * i + 1, imgGrey.Height - 10), new System.Drawing.Point(2 * i + 1, imgGrey.Height - 10 - (int)(percentage * imageHeight)));
                }
            }

            using (Graphics gr = Graphics.FromImage(imgBlue))
            {
                gr.Clear(Color.White);
                for (int i = 0; i < 256; i++)
                {
                    float percentage = histogramRGB[0, i] / maxBlue;
                    gr.DrawLine(Pens.Blue, new System.Drawing.Point(2 * i, imgBlue.Height - 10), new System.Drawing.Point(2 * i, imgBlue.Height - 10 - (int)(percentage * imageHeight)));
                    gr.DrawLine(Pens.Blue, new System.Drawing.Point(2 * i + 1, imgBlue.Height - 10), new System.Drawing.Point(2 * i + 1, imgBlue.Height - 10 - (int)(percentage * imageHeight)));
                }
            }

            using (Graphics gr = Graphics.FromImage(imgGreen))
            {
                gr.Clear(Color.White);
                for (int i = 0; i < 256; i++)
                {
                    float percentage = histogramRGB[1, i] / maxGreen;
                    gr.DrawLine(Pens.Green, new System.Drawing.Point(i * 2, imgGreen.Height - 10), new System.Drawing.Point(i * 2, imgGreen.Height - 10 - (int)(percentage * imageHeight)));
                    gr.DrawLine(Pens.Green, new System.Drawing.Point(2 * i + 1, imgGreen.Height - 10), new System.Drawing.Point(2 * i + 1, imgGreen.Height - 10 - (int)(percentage * imageHeight)));
                }
            }

            using (Graphics gr = Graphics.FromImage(imgRed))
            {
                gr.Clear(Color.White);
                for (int i = 0; i < 256; i++)
                {
                    float percentage = histogramRGB[2, i] / maxRed;
                    gr.DrawLine(Pens.Red, new System.Drawing.Point(2 * i, imgRed.Height - 10), new System.Drawing.Point(2 * i, imgRed.Height - 10 - (int)(percentage * imageHeight)));
                    gr.DrawLine(Pens.Red, new System.Drawing.Point(2 * i + 1, imgRed.Height - 10), new System.Drawing.Point(2 * i + 1, imgRed.Height - 10 - (int)(percentage * imageHeight)));
                }
            }

            imgGrey.Save(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "/Histogram/Grey.jpg");
            imgBlue.Save(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "/Histogram/Blue.jpg");
            imgGreen.Save(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "/Histogram/Green.jpg");
            imgRed.Save(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "/Histogram/Red.jpg");

            new Histogram(imgGrey, imgBlue, imgGreen, imgRed).ShowDialog();
        }

        private void HistogramEqualizationMethod()
        {
            this.SourceImage = Operations.Grayscale((Bitmap)SourceImage.Clone());
            sw.Restart();
            this.SourceImage = Operations.HistogramEqualization((Bitmap)SourceImage.Clone());
            sw.Stop();

            this.ElapsedTimeMessage = string.Format("A művelet időtartama: {0} ms.", sw.ElapsedMilliseconds);
        }
    }
}
