using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.Statistics;
using Reactive.Bindings;

namespace HomographyVisualizer
{
    public class VisualizerViewModel : INotifyPropertyChanged
    {
        public ReactiveCommand DrawSrcAreaCommand { get; set; } = new ReactiveCommand();
        public ReactiveCommand DrawDstAreaCommand { get; set; } = new ReactiveCommand();
        public ReactiveCommand CreateTranslatePointCommand { get; set; } = new ReactiveCommand();

        private List<DenseVector> _srcPoints { get; set; } = new List<DenseVector>();
        private List<DenseVector> _dstPoints { get; set; } = new List<DenseVector>();

        private Line _cacheLine;
        private readonly Canvas _drawCanvas;

        public event PropertyChangedEventHandler PropertyChanged;

        private DenseMatrix _homo;
        private Ellipse _cacheEllipse;

        public enum DrawingState
        {
            Src,
            Dst,
        }

        public VisualizerViewModel(Canvas draCanvas)
        {
            _drawCanvas = draCanvas;

            DrawSrcAreaCommand.Subscribe(() =>
            {
                _drawCanvas.MouseDown += SrcMouseDown;
                _drawCanvas.MouseMove += SrcMouseMove;
            });

            DrawDstAreaCommand.Subscribe(() =>
            {
                _drawCanvas.MouseDown += DstMouseDown;
                _drawCanvas.MouseMove += DstMouseMove;
            });

            CreateTranslatePointCommand.Subscribe(() =>
            {
                try
                {
                    _homo = HomographySharp.HomographyHelper.FindHomography(_srcPoints, _dstPoints);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    return;
                }

                var pointX = _srcPoints.Select(v => v[0]).Mean();
                var pointY = _srcPoints.Select(v => v[1]).Mean();

                Console.WriteLine(pointX);
                Console.WriteLine(pointY);

                var elipse1 = new Ellipse
                {
                    Name = "SrcTarget",
                    Width = 10,
                    Height = 10,
                    Fill = Brushes.DarkViolet,
                };

                var elipse2 = new Ellipse
                {
                    Name = "DstTarget",
                    Width = 10,
                    Height = 10,
                    Fill = Brushes.DarkSlateBlue,
                };

                Canvas.SetLeft(elipse1, pointX - elipse1.Width / 2);
                Canvas.SetTop(elipse1, pointY - elipse1.Height / 2);

                (var translateX, var translateY) = HomographySharp.HomographyHelper.Translate(_homo, pointX, pointY);

                Canvas.SetLeft(elipse2, translateX - elipse1.Width / 2);
                Canvas.SetTop(elipse2, translateY - elipse1.Height / 2);

                _drawCanvas.Children.Add(elipse1);
                _drawCanvas.Children.Add(elipse2);

                elipse1.MouseDown += (sender, args) =>
                {
                    _cacheEllipse = elipse1;
                    elipse1.CaptureMouse();
                };

                elipse1.MouseMove += (sender, args) =>
                {
                    if(_cacheEllipse == null) return;
                    var newPoint = args.GetPosition(_drawCanvas);
                    Canvas.SetLeft(elipse1, newPoint.X - elipse1.Width / 2);
                    Canvas.SetTop(elipse1, newPoint.Y - elipse1.Height / 2);
                    (var newTranslateX, var newTranslateY) = HomographySharp.HomographyHelper.Translate(_homo, newPoint.X, newPoint.Y);
                    Canvas.SetLeft(elipse2, newTranslateX - elipse1.Width / 2);
                    Canvas.SetTop(elipse2, newTranslateY - elipse1.Height / 2);
                };

                elipse1.MouseUp += (sender, args) =>
                {
                    _cacheEllipse = null; 
                    elipse1.ReleaseMouseCapture();
                };
            });
        }

        public void SrcMouseDown(object obj, MouseButtonEventArgs e)
        {
            MouseDown(e, _srcPoints, Brushes.Blue, Brushes.Aqua, DrawingState.Src);
        }

        public void SrcMouseMove(object obj, MouseEventArgs e)
        {
            MouseMove(e, _srcPoints, Brushes.Aqua);

        }

        public void DstMouseDown(object obj, MouseButtonEventArgs e)
        {
            MouseDown(e, _dstPoints, Brushes.Crimson, Brushes.Coral, DrawingState.Dst);
        }

        public void DstMouseMove(object obj, MouseEventArgs e)
        {
            MouseMove(e, _dstPoints, Brushes.Coral);
        }

        public void MouseDown(MouseButtonEventArgs e, List<DenseVector> pointsList, Brush pointBrush, Brush strokeBrush, DrawingState state)
        {
            _cacheLine = null;

            var point = e.GetPosition(_drawCanvas);

            var v = DenseVector.OfArray(new double[] { point.X, point.Y });
            pointsList.Add(v);

            var elipse = new Ellipse
            {
                Width = 7,
                Height = 7,
                Fill = pointBrush
            };

            Console.WriteLine(point.X - elipse.Width / 2);
            Console.WriteLine(point.Y - elipse.Height / 2);

            Canvas.SetLeft(elipse, point.X - elipse.Width / 2);
            Canvas.SetTop(elipse, point.Y - elipse.Height / 2);

            _drawCanvas.Children.Add(elipse);

            if (pointsList.Count == 4)
            {
                if (state == DrawingState.Src)
                {
                    _drawCanvas.MouseDown -= SrcMouseDown;
                    _drawCanvas.MouseMove -= SrcMouseMove;
                }
                else
                {
                    _drawCanvas.MouseDown -= DstMouseDown;
                    _drawCanvas.MouseMove -= DstMouseMove;
                }

                var start = pointsList.First();
                var end = pointsList.Last();
                var line = new Line
                {
                    X1 = start[0],
                    Y1 = start[1],
                    X2 = end[0],
                    Y2 = end[1],
                    StrokeThickness = 3,
                    Stroke = strokeBrush
                };
                _drawCanvas.Children.Add(line);
            }
        }

        public void MouseMove(MouseEventArgs e, List<DenseVector> pointsList, Brush strokeBrush)
        {
            if (!pointsList.Any()) return;
            if (_cacheLine == null)
            {
                var startPoint = pointsList.Last();
                _cacheLine = new Line
                {
                    X1 = startPoint[0],
                    Y1 = startPoint[1],
                    X2 = startPoint[0],
                    Y2 = startPoint[1],
                    StrokeThickness = 3,
                    Stroke = strokeBrush,
                };

                _drawCanvas.Children.Add(_cacheLine);
            }
            var endPoint = e.GetPosition(_drawCanvas);
            _cacheLine.X2 = endPoint.X;
            _cacheLine.Y2 = endPoint.Y;
        }
    }
}