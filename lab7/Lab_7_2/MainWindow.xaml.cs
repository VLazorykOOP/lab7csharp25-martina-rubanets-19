using Microsoft.Win32;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Lab_7_2
{
    public partial class MainWindow : Window
    {
        private Bitmap? originalBitmap;
        private Bitmap? currentBitmap;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "BMP файли (*.bmp)|*.bmp"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                originalBitmap = new Bitmap(openFileDialog.FileName);
                currentBitmap = new Bitmap(originalBitmap);
                ImageControl.Source = BitmapToImageSource(currentBitmap);
            }
        }

        private void SaveImage_Click(object sender, RoutedEventArgs e)
        {
            if (currentBitmap == null) return;

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "BMP файли (*.bmp)|*.bmp"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                currentBitmap.Save(saveFileDialog.FileName, ImageFormat.Bmp);
            }
        }

        private void Channel_Checked(object sender, RoutedEventArgs e)
        {
            if (originalBitmap == null) return;

            currentBitmap = new Bitmap(originalBitmap.Width, originalBitmap.Height);

            for (int y = 0; y < originalBitmap.Height; y++)
            {
                for (int x = 0; x < originalBitmap.Width; x++)
                {
                    System.Drawing.Color pixel = originalBitmap.GetPixel(x, y);
                    System.Drawing.Color newPixel;

                    if (RedChannel.IsChecked == true)
                        newPixel = System.Drawing.Color.FromArgb(pixel.R, 0, 0);
                    else if (GreenChannel.IsChecked == true)
                        newPixel = System.Drawing.Color.FromArgb(0, pixel.G, 0);
                    else // BlueChannel
                        newPixel = System.Drawing.Color.FromArgb(0, 0, pixel.B);

                    currentBitmap.SetPixel(x, y, newPixel);
                }
            }

            ImageControl.Source = BitmapToImageSource(currentBitmap);
        }

        private BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Bmp);
                memory.Position = 0;

                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }
    }
}
