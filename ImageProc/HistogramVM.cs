using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProc
{
    class HistogramVM : ViewModelBase
    {
        private Bitmap red;

        public Bitmap Red
        {
            get { return red; }
            set { Set(ref this.red, value); }
        }

        private Bitmap blue;

        public Bitmap Blue
        {
            get { return blue; }
            set { Set(ref this.blue, value); }
        }

        private Bitmap green;

        public Bitmap Green
        {
            get { return green; }
            set { Set(ref this.green, value); }
        }

        private Bitmap gray;

        public Bitmap Gray
        {
            get { return gray; }
            set { Set(ref this.gray, value); }
        }

        public HistogramVM(Bitmap gray, Bitmap blue, Bitmap green, Bitmap red)
        {
            this.Gray = gray;
            this.Blue = blue;
            this.Green = green;
            this.Red = red;
        }
    }
}
