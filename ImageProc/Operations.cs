using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;

namespace ImageProc
{
    static class Operations
    {
        // Pont alapú műveletek
        public static Bitmap Grayscale(Bitmap image)
        {
            unsafe
            {
                // bitek zárolása, hogy módosíthassuk a képet
                BitmapData imageData = image.LockBits(
                    new Rectangle(0, 0, image.Width, image.Height),
                    ImageLockMode.ReadWrite,
                    image.PixelFormat);

                // magasság (pixelben)
                int height = imageData.Height;

                // szélesség (byte-ban)
                int bytePixel = Bitmap.GetPixelFormatSize(image.PixelFormat) / 8;
                int width = imageData.Width * bytePixel;

                // kép kezdetét tartalmazó pointer
                byte* start = (byte*)imageData.Scan0;

                // az rgb csatornák átlaga fog a változóba kerülni
                int newIntensity = 0;

                for (int y = 0; y < height; y++)
                {
                    // az adott sor kezdetének a számítása: start + y * kép_szélessége_pixelben
                    byte* row = start + (y * imageData.Stride);
                    for (int x = 0; x < width; x = x + bytePixel)
                    {
                        newIntensity = (row[x] + row[x + 1] + row[x + 2]) / 3;

                        row[x] = (byte)newIntensity;
                        row[x + 1] = (byte)newIntensity;
                        row[x + 2] = (byte)newIntensity;
                    }
                };

                image.UnlockBits(imageData);
            }

            return image;
        }

        public static Bitmap Invert(Bitmap image)
        {
            unsafe
            {
                // bitek zárolása, hogy módosíthassuk a képet
                BitmapData imageData = image.LockBits(
                    new Rectangle(0, 0, image.Width, image.Height),
                    ImageLockMode.ReadWrite,
                    image.PixelFormat);

                // magasság (pixelben)
                int height = imageData.Height;

                // szélesség (byte-ban)
                int bytePixel = Bitmap.GetPixelFormatSize(image.PixelFormat) / 8;
                int width = imageData.Width * bytePixel;

                // kép kezdetét tartalmazó pointer
                byte* start = (byte*)imageData.Scan0;

                Parallel.For(0, height, y =>
                {
                    // az adott sor kezdetének a számítása: start + y * kép_szélessége_pixelben
                    byte* row = start + (y * imageData.Stride);
                    for (int x = 0; x < width; x = x + bytePixel)
                    {
                        row[x] = (byte)(255 - row[x]);
                        row[x + 1] = (byte)(255 - row[x + 1]);
                        row[x + 2] = (byte)(255 - row[x + 2]);
                    }
                });

                image.UnlockBits(imageData);
            }

            return image;
        }

        public static Bitmap Treshold(Bitmap image, int treshold)
        {
            unsafe
            {
                // bitek zárolása, hogy módosíthassuk a képet
                BitmapData imageData = image.LockBits(
                    new Rectangle(0, 0, image.Width, image.Height),
                    ImageLockMode.ReadWrite,
                    image.PixelFormat);

                // magasság (pixelben)
                int height = imageData.Height;

                // szélesség (byte-ban)
                int bytePixel = Bitmap.GetPixelFormatSize(image.PixelFormat) / 8;
                int width = imageData.Width * bytePixel;

                // kép kezdetét tartalmazó pointer
                byte* start = (byte*)imageData.Scan0;

                Parallel.For(0, height, y =>
                {
                    // az adott sor kezdetének a számítása: start + y * kép_szélessége_pixelben
                    byte* row = start + (y * imageData.Stride);
                    for (int x = 0; x < width; x = x + bytePixel)
                    {
                        // adott pixel intenzitása
                        int currentPixel = row[x];

                        // küszöbölés
                        if (currentPixel < treshold)
                        {
                            row[x] = 0;
                            row[x + 1] = 0;
                            row[x + 2] = 0;
                        }
                        else
                        {
                            row[x] = 255;
                            row[x + 1] = 255;
                            row[x + 2] = 255;
                        }
                    }
                });

                image.UnlockBits(imageData);
            }

            return image;
        }

        public static Bitmap ContrastStretch(Bitmap image, int low, int high)
        {
            unsafe
            {
                // bitek zárolása, hogy módosíthassuk a képet
                BitmapData imageData = image.LockBits(
                    new Rectangle(0, 0, image.Width, image.Height),
                    ImageLockMode.ReadWrite,
                    image.PixelFormat);

                // magasság (pixelben)
                int height = imageData.Height;

                // szélesség (byte-ban)
                int bytePixel = Bitmap.GetPixelFormatSize(image.PixelFormat) / 8;
                int width = imageData.Width * bytePixel;

                // kép kezdetét tartalmazó pointer
                byte* start = (byte*)imageData.Scan0;

                // az új intenzitás érték kiszámításához előre elvégezzük a kivonást
                int scale = high - low;

                Parallel.For(0, height, y =>
                {
                    // az adott sor kezdetének a számítása: start + y * kép_szélessége_pixelben
                    byte* row = start + (y * imageData.Stride);
                    for (int x = 0; x < width; x = x + bytePixel)
                    {
                        row[x] = (byte)((row[x] * scale) / 255 + low);
                        row[x + 1] = (byte)((row[x + 1] * scale) / 255 + low);
                        row[x + 2] = (byte)((row[x + 2] * scale) / 255 + low);
                    }
                });

                image.UnlockBits(imageData);
            }

            return image;
        }

        public static Bitmap LogaritmicScale(Bitmap image)
        {
            unsafe
            {
                // bitek zárolása, hogy módosíthassuk a képet
                BitmapData imageData = image.LockBits(
                    new Rectangle(0, 0, image.Width, image.Height),
                    ImageLockMode.ReadWrite,
                    image.PixelFormat);

                // magasság (pixelben)
                int height = imageData.Height;

                // szélesség (byte-ban)
                int bytePixel = Bitmap.GetPixelFormatSize(image.PixelFormat) / 8;
                int width = imageData.Width * bytePixel;

                // kép kezdetét tartalmazó pointer
                byte* start = (byte*)imageData.Scan0;

                // skálázási tényező számítása
                int c = (int)(255 / Math.Log(256, 10));

                Parallel.For(0, height, y =>
                {
                    // az adott sor kezdetének a számítása: start + y * kép_szélessége_pixelben
                    byte* row = start + (y * imageData.Stride);
                    for (int x = 0; x < width; x = x + bytePixel)
                    {
                        row[x] = (byte)(c * Math.Log(1 + row[x], 10));
                        row[x + 1] = (byte)(c * Math.Log(1 + row[x + 1], 10));
                        row[x + 2] = (byte)(c * Math.Log(1 + row[x + 2], 10));
                    }
                });

                image.UnlockBits(imageData);
            }

            return image;
        }


        // Maszk alapú műveletek
        public static Bitmap MeanFilter(Bitmap image)
        {
            unsafe
            {
                // bitek zárolása, hogy módosíthassuk a képet
                BitmapData imageData = image.LockBits(
                    new Rectangle(0, 0, image.Width, image.Height),
                    ImageLockMode.ReadWrite,
                    image.PixelFormat);

                // magasság (pixelben)
                int height = imageData.Height;

                // szélesség (byte-ban)
                int bytePixel = Bitmap.GetPixelFormatSize(image.PixelFormat) / 8;
                int width = imageData.Width * bytePixel;

                // kép kezdetét tartalmazó pointer
                byte* start = (byte*)imageData.Scan0;

                // LUT létrehozása és feltöltése
                int[,] oldRed = new int[imageData.Width, height];
                int[,] oldGreen = new int[imageData.Width, height];
                int[,] oldBlue = new int[imageData.Width, height];

                Parallel.For(0, height, y =>
                {
                    int x = 0;
                    byte* row = start + (y * imageData.Stride);
                    for (int index = 0; index < width; index = index + bytePixel)
                    {
                        oldBlue[x, y] = row[index];
                        oldGreen[x, y] = row[index + 1];
                        oldRed[x, y] = row[index + 2];
                        x++;
                    }
                });

                // Átlagolás 3x3-as maszkkal
                Parallel.For(1, height - 1, y =>
                {
                    int x = 1;
                    byte* row = start + (y * imageData.Stride);
                    for (int index = bytePixel; index < width - bytePixel; index = index + bytePixel)
                    {
                        row[index] = (byte)((oldBlue[x - 1, y - 1] + oldBlue[x - 1, y] + oldBlue[x - 1, y + 1] + oldBlue[x, y - 1] + oldBlue[x, y] + oldBlue[x, y + 1] + oldBlue[x + 1, y - 1] + oldBlue[x + 1, y] + oldBlue[x + 1, y + 1]) / 9);
                        row[index + 1] = (byte)((oldGreen[x - 1, y - 1] + oldGreen[x - 1, y] + oldGreen[x - 1, y + 1] + oldGreen[x, y - 1] + oldGreen[x, y] + oldGreen[x, y + 1] + oldGreen[x + 1, y - 1] + oldGreen[x + 1, y] + oldGreen[x + 1, y + 1]) / 9);
                        row[index + 2] = (byte)((oldRed[x - 1, y - 1] + oldRed[x - 1, y] + oldRed[x - 1, y + 1] + oldRed[x, y - 1] + oldRed[x, y] + oldRed[x, y + 1] + oldRed[x + 1, y - 1] + oldRed[x + 1, y] + oldRed[x + 1, y + 1]) / 9);
                        x++;
                    }
                });

                image.UnlockBits(imageData);
            }

            return image;
        }

        public static Bitmap GaussianFilter(Bitmap image)
        {
            unsafe
            {
                // bitek zárolása, hogy módosíthassuk a képet
                BitmapData imageData = image.LockBits(
                    new Rectangle(0, 0, image.Width, image.Height),
                    ImageLockMode.ReadWrite,
                    image.PixelFormat);

                // magasság (pixelben)
                int height = imageData.Height;

                // szélesség (byte-ban)
                int bytePixel = Bitmap.GetPixelFormatSize(image.PixelFormat) / 8;
                int width = imageData.Width * bytePixel;

                // kép kezdetét tartalmazó pointer
                byte* start = (byte*)imageData.Scan0;

                // LUT létrehozása és feltöltése
                int[,] oldRed = new int[imageData.Width, height];
                int[,] oldGreen = new int[imageData.Width, height];
                int[,] oldBlue = new int[imageData.Width, height];

                Parallel.For(0, height, y =>
                {
                    int x = 0;
                    byte* row = start + (y * imageData.Stride);
                    for (int index = 0; index < width; index = index + bytePixel)
                    {
                        oldBlue[x, y] = row[index];
                        oldGreen[x, y] = row[index + 1];
                        oldRed[x, y] = row[index + 2];
                        x++;
                    }
                });

                Parallel.For(1, height - 1, y =>
                {
                    int x = 2;
                    byte* row = start + (y * imageData.Stride);
                    for (int index = bytePixel + bytePixel; index < width - 2 * bytePixel; index = index + bytePixel)
                    {
                        row[index] = (byte)((oldBlue[x - 2, y] + 4 * oldBlue[x - 1, y] + 6 * oldBlue[x, y] + 4 * oldBlue[x + 1, y] + oldBlue[x + 2, y]) / 16);
                        row[index + 1] = (byte)((oldGreen[x - 2, y] + 4 * oldGreen[x - 1, y] + 6 * oldGreen[x, y] + 4 * oldGreen[x + 1, y] + oldGreen[x + 2, y]) / 16);
                        row[index + 2] = (byte)((oldRed[x - 2, y] + 4 * oldRed[x - 1, y] + 6 * oldRed[x, y] + 4 * oldRed[x + 1, y] + oldRed[x + 2, y]) / 16);
                        x++;
                    }
                });

                Parallel.For(2, height - 2, y =>
                {
                    int x = 2;
                    byte* row = start + (y * imageData.Stride);
                    for (int index = bytePixel + bytePixel; index < width - 2 * bytePixel; index = index + bytePixel)
                    {
                        row[index] = (byte)((oldBlue[x, y - 2] + 4 * oldBlue[x, y - 1] + 6 * oldBlue[x, y] + 4 * oldBlue[x, y + 1] + oldBlue[x, y + 2]) / 16);
                        row[index + 1] = (byte)((oldGreen[x, y - 2] + 4 * oldGreen[x, y - 1] + 6 * oldGreen[x, y] + 4 * oldGreen[x, y + 1] + oldGreen[x, y + 2]) / 16);
                        row[index + 2] = (byte)((oldRed[x, y - 2] + 4 * oldRed[x, y - 1] + 6 * oldRed[x, y] + 4 * oldRed[x, y + 1] + oldRed[x, y + 2]) / 16);
                        x++;
                    }
                });

                image.UnlockBits(imageData);
            }
            return image;
        }

        public static Bitmap MedianFilter(Bitmap image)
        {
            unsafe
            {
                // bitek zárolása, hogy módosíthassuk a képet
                BitmapData imageData = image.LockBits(
                    new Rectangle(0, 0, image.Width, image.Height),
                    ImageLockMode.ReadWrite,
                    image.PixelFormat);

                // magasság (pixelben)
                int height = imageData.Height;

                // szélesség (byte-ban)
                int bytePixel = Bitmap.GetPixelFormatSize(image.PixelFormat) / 8;
                int width = imageData.Width * bytePixel;

                // kép kezdetét tartalmazó pointer
                byte* start = (byte*)imageData.Scan0;

                // LUT létrehozása és feltöltése
                int[,] oldRed = new int[imageData.Width, height];
                int[,] oldGreen = new int[imageData.Width, height];
                int[,] oldBlue = new int[imageData.Width, height];

                Parallel.For(0, height, y =>
                {
                    int x = 0;
                    byte* row = start + (y * imageData.Stride);
                    for (int index = 0; index < width; index = index + bytePixel)
                    {
                        oldBlue[x, y] = row[index];
                        oldGreen[x, y] = row[index + 1];
                        oldRed[x, y] = row[index + 2];
                        x++;
                    }
                });


                Parallel.For(1, height - 1, y =>
                {
                    int x = 1;
                    byte* row = start + (y * imageData.Stride);
                    for (int index = bytePixel; index < width - bytePixel; index = index + bytePixel)
                    {
                        row[index] = (byte)Median(new[] { oldBlue[x - 1, y - 1], oldBlue[x - 1, y], oldBlue[x - 1, y + 1], oldBlue[x, y - 1], oldBlue[x, y], oldBlue[x, y + 1], oldBlue[x + 1, y - 1], oldBlue[x + 1, y], oldBlue[x + 1, y + 1] });
                        row[index + 1] = (byte)Median(new[] { oldGreen[x - 1, y - 1], oldGreen[x - 1, y], oldGreen[x - 1, y + 1], oldGreen[x, y - 1], oldGreen[x, y], oldGreen[x, y + 1], oldGreen[x + 1, y - 1], oldGreen[x + 1, y], oldGreen[x + 1, y + 1] });
                        row[index + 2] = (byte)Median(new[] { oldRed[x - 1, y - 1], oldRed[x - 1, y], oldRed[x - 1, y + 1], oldRed[x, y - 1], oldRed[x, y], oldRed[x, y + 1], oldRed[x + 1, y - 1], oldRed[x + 1, y], oldRed[x + 1, y + 1] });
                        x++;
                    }
                });

                //Parallel.For(1, height - 1, y =>
                //{
                //    int x = 1;
                //    byte* row = start + (y * imageData.Stride);
                //    for (int index = bytePixel; index < width - bytePixel; index = index + bytePixel)
                //    {
                //        row[index] = (byte)Sort2DArray(new[,] { { oldBlue[x - 1, y - 1], oldBlue[x - 1, y], oldBlue[x - 1, y + 1] }, { oldBlue[x, y - 1], oldBlue[x, y], oldBlue[x, y + 1] }, { oldBlue[x + 1, y - 1], oldBlue[x + 1, y], oldBlue[x + 1, y + 1] } });
                //        row[index + 1] = (byte)Sort2DArray(new[,] { { oldGreen[x - 1, y - 1], oldGreen[x - 1, y], oldGreen[x - 1, y + 1] }, { oldGreen[x, y - 1], oldGreen[x, y], oldGreen[x, y + 1] }, { oldGreen[x + 1, y - 1], oldGreen[x + 1, y], oldGreen[x + 1, y + 1] } });
                //        row[index + 2] = (byte)Sort2DArray(new[,] { { oldRed[x - 1, y - 1], oldRed[x - 1, y], oldRed[x - 1, y + 1] }, { oldRed[x, y - 1], oldRed[x, y], oldRed[x, y + 1] }, { oldRed[x + 1, y - 1], oldRed[x + 1, y], oldRed[x + 1, y + 1] } });
                //        x++;
                //    }
                //});

                image.UnlockBits(imageData);
            }
            return image;
        }

        public static int Median(int[] t)
        {
            Array.Sort(t);
            return t[4];
        }

        //public static void CompareAndSwap(ref int a, ref int b)
        //{
        //    if (a > b)
        //    {
        //        int c = b;
        //        b = a;
        //        a = c;
        //    }
        //}

        //public static int Sort2DArray(int[,] t)
        //{
        //    CompareAndSwap(ref t[1, 0], ref t[1, 1]);
        //    CompareAndSwap(ref t[1, 1], ref t[1, 2]);
        //    CompareAndSwap(ref t[1, 0], ref t[1, 1]);

        //    CompareAndSwap(ref t[0, 1], ref t[1, 1]);
        //    CompareAndSwap(ref t[1, 1], ref t[2, 1]);
        //    CompareAndSwap(ref t[0, 1], ref t[1, 1]);

        //    return t[1, 1];
        //}

        public static Bitmap LaplaceFilter(Bitmap image)
        {
            unsafe
            {
                // bitek zárolása, hogy módosíthassuk a képet
                BitmapData imageData = image.LockBits(
                    new Rectangle(0, 0, image.Width, image.Height),
                    ImageLockMode.ReadWrite,
                    image.PixelFormat);

                // magasság (pixelben)
                int height = imageData.Height;

                // szélesség (byte-ban)
                int bytePixel = Bitmap.GetPixelFormatSize(image.PixelFormat) / 8;
                int width = imageData.Width * bytePixel;

                // kép kezdetét tartalmazó pointer
                byte* start = (byte*)imageData.Scan0;

                // LUT létrehozása és feltöltése
                int[,] oldPixel = new int[imageData.Width, height];

                Parallel.For(0, height, y =>
                {
                    int x = 0;
                    byte* row = start + (y * imageData.Stride);
                    for (int index = 0; index < width; index = index + bytePixel)
                    {
                        oldPixel[x, y] = row[index];
                        x++;
                    }
                });

                // Laplace szűrő 3x3-as maszkkal
                Parallel.For(1, height - 1, y =>
                {
                    int x = 1;
                    byte* row = start + (y * imageData.Stride);
                    for (int index = bytePixel; index < width - bytePixel; index = index + bytePixel)
                    {
                        byte newPixel = (byte)((oldPixel[x - 1, y - 1] + oldPixel[x - 1, y] + oldPixel[x - 1, y + 1] + oldPixel[x, y - 1] - 8 * oldPixel[x, y] + oldPixel[x, y + 1] + oldPixel[x + 1, y - 1] + oldPixel[x + 1, y] + oldPixel[x + 1, y + 1]) / (-16));
                        row[index] = newPixel;
                        row[index + 1] = newPixel;
                        row[index + 2] = newPixel;
                        x++;
                    }
                });

                image.UnlockBits(imageData);
            }

            return image;
        }


        // Él- és sarokpont detektálás
        public static Bitmap SobelEdgeDetector(Bitmap image)
        {
            unsafe
            {
                // bitek zárolása, hogy módosíthassuk a képet
                BitmapData imageData = image.LockBits(
                    new Rectangle(0, 0, image.Width, image.Height),
                    ImageLockMode.ReadWrite,
                    image.PixelFormat);

                // magasság (pixelben)
                int height = imageData.Height;

                // szélesség (byte-ban)
                int bytePixel = Bitmap.GetPixelFormatSize(image.PixelFormat) / 8;
                int width = imageData.Width * bytePixel;

                // kép kezdetét tartalmazó pointer
                byte* start = (byte*)imageData.Scan0;

                // LUT létrehozása és feltöltése
                int[,] oldPixel = new int[imageData.Width, height];

                Parallel.For(0, height, y =>
                {
                    int x = 0;
                    byte* row = start + (y * imageData.Stride);
                    for (int index = 0; index < width; index = index + bytePixel)
                    {
                        oldPixel[x, y] = row[index];
                        x++;
                    }
                });

                // x és y irányú gradiens összetevők
                int[,] gradX = new int[imageData.Width, height];
                int[,] gradY = new int[imageData.Width, height];

                Parallel.For(1, height - 1, y =>
                {
                    int x = 1;
                    byte* row = start + (y * imageData.Stride);
                    for (int index = bytePixel; index < width - bytePixel; index = index + bytePixel)
                    {
                        gradX[x, y] = (oldPixel[x - 1, y - 1] + 2 * oldPixel[x - 1, y] + oldPixel[x - 1, y + 1] - oldPixel[x + 1, y - 1] - 2 * oldPixel[x + 1, y] - oldPixel[x + 1, y + 1]);
                        gradY[x, y] = (oldPixel[x - 1, y - 1] + 2 * oldPixel[x, y - 1] + oldPixel[x + 1, y - 1] - oldPixel[x - 1, y + 1] - 2 * oldPixel[x, y + 1] - oldPixel[x + 1, y + 1]);

                        byte newIntensity = (byte)(Math.Sqrt(gradX[x, y] * gradX[x, y] + gradY[x, y] * gradY[x, y]));
                        row[index] = newIntensity;
                        row[index + 1] = newIntensity;
                        row[index + 2] = newIntensity;
                        x++;
                    }
                });

                image.UnlockBits(imageData);
            }

            return image;
        }

        public static Bitmap HarrisCornerDetector(Bitmap image)
        {
            unsafe
            {
                // bitek zárolása, hogy módosíthassuk a képet
                BitmapData imageData = image.LockBits(
                    new Rectangle(0, 0, image.Width, image.Height),
                    ImageLockMode.ReadWrite,
                    image.PixelFormat);

                // magasság (pixelben)
                int height = imageData.Height;

                // szélesség (byte-ban)
                int bytePixel = Bitmap.GetPixelFormatSize(image.PixelFormat) / 8;
                int width = imageData.Width * bytePixel;

                // kép kezdetét tartalmazó pointer
                byte* start = (byte*)imageData.Scan0;

                // LUT létrehozása és feltöltése
                int[,] oldPixel = new int[imageData.Width, height];

                Parallel.For(0, height, y =>
                {
                    int x = 0;
                    byte* row = start + (y * imageData.Stride);
                    for (int index = 0; index < width; index = index + bytePixel)
                    {
                        oldPixel[x, y] = row[index];
                        x++;
                    }
                });

                // x és y irányú gradiens összetevők
                int[,] gradX = new int[imageData.Width, height];
                int[,] gradY = new int[imageData.Width, height];

                // gradX*gradX, gradX*gradY és gradY*gradY képek
                int[,] imageXX = new int[imageData.Width, height];
                int[,] imageXY = new int[imageData.Width, height];
                int[,] imageYY = new int[imageData.Width, height];

                Parallel.For(1, height - 1, y =>
                {
                    int x = 1;
                    byte* row = start + (y * imageData.Stride);
                    for (int index = bytePixel; index < width - bytePixel; index = index + bytePixel)
                    {
                        gradX[x, y] = (oldPixel[x - 1, y - 1] + 1 * oldPixel[x - 1, y] + oldPixel[x - 1, y + 1] - oldPixel[x + 1, y - 1] - 1 * oldPixel[x + 1, y] - oldPixel[x + 1, y + 1]);
                        gradY[x, y] = (oldPixel[x - 1, y - 1] + 1 * oldPixel[x, y - 1] + oldPixel[x + 1, y - 1] - oldPixel[x - 1, y + 1] - 1 * oldPixel[x, y + 1] - oldPixel[x + 1, y + 1]);

                        imageXX[x, y] = gradX[x, y] * gradX[x, y];
                        imageXY[x, y] = gradX[x, y] * gradY[x, y];
                        imageYY[x, y] = gradY[x, y] * gradY[x, y];

                        x++;
                    }
                });

                // Sum(imageXX), Sum(imageXY), Sum(imageYY) 3x3-as maszkokkal
                int[,] sumXX = new int[imageData.Width, height];
                int[,] sumXY = new int[imageData.Width, height];
                int[,] sumYY = new int[imageData.Width, height];

                Parallel.For(1, height - 1, y =>
                {
                    int x = 1;
                    byte* row = start + (y * imageData.Stride);
                    for (int index = bytePixel; index < width - bytePixel; index = index + bytePixel)
                    {
                        sumXX[x, y] = imageXX[x - 1, y - 1] + imageXX[x - 1, y] + imageXX[x - 1, y + 1] + imageXX[x, y - 1] + imageXX[x, y] + imageXX[x, y + 1] + imageXX[x + 1, y - 1] + imageXX[x + 1, y] + imageXX[x + 1, y + 1];
                        sumXY[x, y] = imageXY[x - 1, y - 1] + imageXY[x - 1, y] + imageXY[x - 1, y + 1] + imageXY[x, y - 1] + imageXY[x, y] + imageXY[x, y + 1] + imageXY[x + 1, y - 1] + imageXY[x + 1, y] + imageXY[x + 1, y + 1];
                        sumYY[x, y] = imageYY[x - 1, y - 1] + imageYY[x - 1, y] + imageYY[x - 1, y + 1] + imageYY[x, y - 1] + imageYY[x, y] + imageYY[x, y + 1] + imageYY[x + 1, y - 1] + imageYY[x + 1, y] + imageYY[x + 1, y + 1];

                        int divider = (sumXX[x, y] + sumYY[x, y]);
                        int harris = 0;
                        if (divider != 0)
                        {
                            harris = (sumXX[x, y] * sumYY[x, y] - sumXY[x, y] * sumXY[x, y]) / (sumXX[x, y] + sumYY[x, y]);
                        }

                        byte newIntensity = (byte)harris;
                        //byte newIntensity = (byte)(0.5 * ((sumXX[x, y] + sumYY[x, y]) + (Math.Sqrt(4 * sumXY[x, y] * sumXY[x, y] + (sumXX[x, y] - sumYY[x, y]) * (sumXX[x, y] - sumYY[x, y])))));
                        row[index] = newIntensity;
                        row[index + 1] = newIntensity;
                        row[index + 2] = newIntensity;
                        x++;
                    }
                });
                image.UnlockBits(imageData);
            }

            return image;
        }


        // Hisztogram műveletek
        public static int[] CreateHistogramGray(Bitmap image)
        {
            int[] histogram = new int[256];

            unsafe
            {
                // bitek zárolása, hogy módosíthassuk a képet
                BitmapData imageData = image.LockBits(
                    new Rectangle(0, 0, image.Width, image.Height),
                    ImageLockMode.ReadWrite,
                    image.PixelFormat);

                // magasság (pixelben)
                int height = imageData.Height;

                // szélesség (byte-ban)
                int bytePixel = Bitmap.GetPixelFormatSize(image.PixelFormat) / 8;
                int width = imageData.Width * bytePixel;

                // kép kezdetét tartalmazó pointer
                byte* start = (byte*)imageData.Scan0;

                // hisztogram tömb feltöltése
                for (int y = 0; y < height; y++)
                {
                    byte* row = start + (y * imageData.Stride);
                    for (int index = 0; index < width; index = index + bytePixel)
                    {
                        histogram[row[index]] = histogram[row[index]] + 1;
                    }
                }

                image.UnlockBits(imageData);
            }
            return histogram;
        }

        public static int[,] CreateHistogramRGB(Bitmap image)
        {
            int[,] histogramRGB = new int[3,256];

            unsafe
            {
                // bitek zárolása, hogy módosíthassuk a képet
                BitmapData imageData = image.LockBits(
                    new Rectangle(0, 0, image.Width, image.Height),
                    ImageLockMode.ReadWrite,
                    image.PixelFormat);

                // magasság (pixelben)
                int height = imageData.Height;

                // szélesség (byte-ban)
                int bytePixel = Bitmap.GetPixelFormatSize(image.PixelFormat) / 8;
                int width = imageData.Width * bytePixel;

                // kép kezdetét tartalmazó pointer
                byte* start = (byte*)imageData.Scan0;

                // hisztogram tömb feltöltése
                for (int y = 0; y < height; y++)
                {
                    
                    byte* row = start + (y * imageData.Stride);
                    for (int index = 0; index < width; index = index + bytePixel)
                    {
                        histogramRGB[0,row[index]] = histogramRGB[0,row[index]] + 1;
                        histogramRGB[1, row[index+1]] = histogramRGB[1, row[index+1]] + 1;
                        histogramRGB[2, row[index+2]] = histogramRGB[2, row[index+2]] + 1;
                    }
                }

                image.UnlockBits(imageData);
            }

            return histogramRGB;
        }

        public static Bitmap HistogramEqualization(Bitmap image)
        {
            float[] histogram = new float[256];

            unsafe
            {
                // bitek zárolása, hogy módosíthassuk a képet
                BitmapData imageData = image.LockBits(
                    new Rectangle(0, 0, image.Width, image.Height),
                    ImageLockMode.ReadWrite,
                    image.PixelFormat);

                // magasság (pixelben)
                int height = imageData.Height;

                // szélesség (byte-ban)
                int bytePixel = Bitmap.GetPixelFormatSize(image.PixelFormat) / 8;
                int width = imageData.Width * bytePixel;

                // kép kezdetét tartalmazó pointer
                byte* start = (byte*)imageData.Scan0;

                int pixelekszama = 0;
                // hisztogram tömb feltöltése
                for (int y = 0; y < height; y++)
                {
                    byte* row = start + (y * imageData.Stride);
                    for (int index = 0; index < width; index = index + bytePixel)
                    {
                        histogram[row[index]] = histogram[row[index]] + 1;
                        pixelekszama++;
                    }
                }

                // halmozott hisztogram
                for (int i = 1; i < histogram.Length; i++)
                {
                    histogram[i] = histogram[i] + histogram[i - 1];
                }

                // új pixelértékek
                for (int i = 1; i < histogram.Length; i++)
                {
                    histogram[i] = histogram[i] / pixelekszama * 255;
                }

                // transzformálás
                for (int y = 0; y < height; y++)
                {
                    byte* row = start + (y * imageData.Stride);
                    for (int index = 0; index < width; index = index + bytePixel)
                    {
                        byte newIntensity = (byte)histogram[row[index]];
                        row[index] = newIntensity;
                        row[index + 1] = newIntensity;
                        row[index + 2] = newIntensity;
                    }
                }
            }
            return image;
        }
    }
}
