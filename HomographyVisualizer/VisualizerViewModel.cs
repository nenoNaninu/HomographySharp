using MathNet.Numerics.LinearAlgebra.Double;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace HomographyVisualizer
{
    public class VisualizerViewModel : INotifyPropertyChanged
    {
        public ReactiveCommand DrawSrcAreaCommand { get; } = new ReactiveCommand();
        public ReactiveCommand DrawDstAreaCommand { get; } = new ReactiveCommand();
        public ReactiveCommand CreateTranslatePointCommand { get; } = new ReactiveCommand();

        private readonly List<DenseVector> _srcPoints = new List<DenseVector>();
        private readonly List<DenseVector> _dstPoints = new List<DenseVector>();

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

                var pointX = _srcPoints.Average(v => v[0]);
                var pointY = _srcPoints.Average(v => v[1]);

                Console.WriteLine(pointX);
                Console.WriteLine(pointY);

                var srcEllipse = new Ellipse
                {
                    Name = "SrcTarget",
                    Width = 10,
                    Height = 10,
                    Fill = Brushes.DarkViolet,
                };

                var dstEllipse = new Ellipse
                {
                    Name = "DstTarget",
                    Width = 10,
                    Height = 10,
                    Fill = Brushes.DarkSlateBlue,
                };

                Canvas.SetLeft(srcEllipse, pointX - srcEllipse.Width / 2);
                Canvas.SetTop(srcEllipse, pointY - srcEllipse.Height / 2);

                (var translateX, var translateY) = HomographySharp.HomographyHelper.Translate(_homo, pointX, pointY);

                Canvas.SetLeft(dstEllipse, translateX - srcEllipse.Width / 2);
                Canvas.SetTop(dstEllipse, translateY - srcEllipse.Height / 2);

                _drawCanvas.Children.Add(srcEllipse);
                _drawCanvas.Children.Add(dstEllipse);

                srcEllipse.MouseDown += (sender, args) =>
                {
                    _cacheEllipse = srcEllipse;
                    srcEllipse.CaptureMouse();
                };

                srcEllipse.MouseMove += (sender, args) =>
                {
                    if (_cacheEllipse == null) return;
                    var newPoint = args.GetPosition(_drawCanvas);
                    Canvas.SetLeft(srcEllipse, newPoint.X - srcEllipse.Width / 2);
                    Canvas.SetTop(srcEllipse, newPoint.Y - srcEllipse.Height / 2);
                    (var newTranslateX, var newTranslateY) = HomographySharp.HomographyHelper.Translate(_homo, newPoint.X, newPoint.Y);
                    Canvas.SetLeft(dstEllipse, newTranslateX - srcEllipse.Width / 2);
                    Canvas.SetTop(dstEllipse, newTranslateY - srcEllipse.Height / 2);
                };

                srcEllipse.MouseUp += (sender, args) =>
                {
                    _cacheEllipse = null;
                    srcEllipse.ReleaseMouseCapture();
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