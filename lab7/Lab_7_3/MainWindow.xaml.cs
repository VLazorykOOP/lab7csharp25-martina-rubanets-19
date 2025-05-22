using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using static System.Net.Mime.MediaTypeNames;

namespace Lab_7_3
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            var figures = GenerateRandomFigures(10, 450, 400);
            DrawFigures(figures);
        }

        public abstract class Figure
        {
            public Point Center { get; set; }
            public double Radius { get; set; }
            public Brush Color { get; set; }

            public Figure(Point center, double radius, Brush color)
            {
                Center = center;
                Radius = radius;
                Color = color;
            }

            public abstract void Draw(DrawingContext dc);
            public virtual void Move(double dx, double dy)
            {
                Center = new Point(Center.X + dx, Center.Y + dy);
            }

            protected List<Point> GetPolygonPoints(int sides)
            {
                var points = new List<Point>();
                double angleStep = 2 * Math.PI / sides;
                double startAngle = -Math.PI / 2; 

                for (int i = 0; i < sides; i++)
                {
                    double angle = startAngle + i * angleStep;
                    double x = Center.X + Radius * Math.Cos(angle);
                    double y = Center.Y + Radius * Math.Sin(angle);
                    points.Add(new Point(x, y));
                }
                return points;
            }
        }

        public class Pentagon : Figure
        {
            public Pentagon(Point center, double radius, Brush color) : base(center, radius, color) { }

            public override void Draw(DrawingContext dc)
            {
                DrawPolygon(dc, 5);
            }

            private void DrawPolygon(DrawingContext dc, int sides)
            {
                var points = GetPolygonPoints(sides);
                var geometry = new StreamGeometry();

                using (var ctx = geometry.Open())
                {
                    ctx.BeginFigure(points[0], true, true);
                    ctx.PolyLineTo(points.Skip(1).ToList(), true, true);
                }

                dc.DrawGeometry(Color, new Pen(Brushes.Black, 1), geometry);
            }
        }

        public class Hexagon : Figure
        {
            public Hexagon(Point center, double radius, Brush color) : base(center, radius, color) { }

            public override void Draw(DrawingContext dc)
            {
                DrawPolygon(dc, 6);
            }

            private void DrawPolygon(DrawingContext dc, int sides)
            {
                var points = GetPolygonPoints(sides);
                var geometry = new StreamGeometry();

                using (var ctx = geometry.Open())
                {
                    ctx.BeginFigure(points[0], true, true);
                    ctx.PolyLineTo(points.Skip(1).ToList(), true, true);
                }

                dc.DrawGeometry(Color, new Pen(Brushes.Black, 1), geometry);
            }
        }

        public class Octagon : Figure
        {
            public Octagon(Point center, double radius, Brush color) : base(center, radius, color) { }

            public override void Draw(DrawingContext dc)
            {
                DrawPolygon(dc, 8);
            }

            private void DrawPolygon(DrawingContext dc, int sides)
            {
                var points = GetPolygonPoints(sides);
                var geometry = new StreamGeometry();

                using (var ctx = geometry.Open())
                {
                    ctx.BeginFigure(points[0], true, true);
                    ctx.PolyLineTo(points.Skip(1).ToList(), true, true);
                }

                dc.DrawGeometry(Color, new Pen(Brushes.Black, 1), geometry);
            }
        }

        private void DrawFigures(List<Figure> figures)
        {
            int width = 450, height = 400;

            DrawingVisual visual = new DrawingVisual();
            using (DrawingContext dc = visual.RenderOpen())
            {
                dc.DrawRectangle(Brushes.White, null, new Rect(0, 0, width, height));

                foreach (var figure in figures)
                    figure.Draw(dc);
            }

            RenderTargetBitmap bmp = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(visual);
            MyImage.Source = bmp;
        }

        private List<Figure> GenerateRandomFigures(int count, int maxWidth, int maxHeight)
        {
            var rnd = new Random();
            var figures = new List<Figure>();

            for (int i = 0; i < count; i++)
            {
                Point center = new Point(rnd.Next(50, maxWidth - 50), rnd.Next(50, maxHeight - 50));
                double radius = rnd.Next(20, 40);
                Brush color = new SolidColorBrush(Color.FromRgb((byte)rnd.Next(256), (byte)rnd.Next(256), (byte)rnd.Next(256)));

                int type = rnd.Next(3);
                Figure fig = type switch
                {
                    0 => new Pentagon(center, radius, color),
                    1 => new Hexagon(center, radius, color),
                    2 => new Octagon(center, radius, color),
                    _ => null
                };

                figures.Add(fig);
            }

            return figures;
        }
    }
}
