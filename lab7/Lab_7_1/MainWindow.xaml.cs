using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows.Media.Imaging;

namespace Lab_7_1
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;
        private double angle = 0;
        private double radius = 150;
        private Random rand = new Random();

        private string[] imagePaths = new string[]
        {
            "Images/img1.jpg",
            "Images/img2.jpg",
            "Images/img3.jpg"
        };

        public MainWindow()
        {
            InitializeComponent();

            // Початкова позиція
            UpdateOrbitingPanelPosition();

            // Таймер
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(50);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            angle += 5;
            if (angle >= 360) angle = 0;

            UpdateOrbitingPanelPosition();
            ChangeRandomColorAndImage();
        }

        private void UpdateOrbitingPanelPosition()
        {
            double centerX = Canvas.GetLeft(CenterPanel) + CenterPanel.Width / 2;
            double centerY = Canvas.GetTop(CenterPanel) + CenterPanel.Height / 2;

            double rad = angle * Math.PI / 180;
            double x = centerX + radius * Math.Cos(rad) - OrbitingPanel.Width / 2;
            double y = centerY + radius * Math.Sin(rad) - OrbitingPanel.Height / 2;

            Canvas.SetLeft(OrbitingPanel, x);
            Canvas.SetTop(OrbitingPanel, y);
        }

        private void ChangeRandomColorAndImage()
        {
            // Випадковий колір
            OrbitingPanel.Background = new SolidColorBrush(Color.FromRgb(
                (byte)rand.Next(256),
                (byte)rand.Next(256),
                (byte)rand.Next(256)));

            // Випадкова картинка
            string imgPath = imagePaths[rand.Next(imagePaths.Length)];
            try
            {
                ImageBrush imgBrush = new ImageBrush();
                imgBrush.ImageSource = new BitmapImage(new Uri(imgPath, UriKind.Relative));
                OrbitingPanel.Background = imgBrush;
            }
            catch { } // якщо зображення не знайдене
        }
    }
}
