using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using HomographySharp;

namespace HomographyVisualizer
{
    public class VisualizerViewModel : INotifyPropertyChanged
    {
        private ReactiveProperty<bool> _drawingSrc = new ReactiveProperty<bool>(false);
        private ReactiveProperty<bool> _drawingDst = new ReactiveProperty<bool>(false);

        public ReactiveProperty<string> PointNumString { get; }

        public ReactiveProperty<bool> EnableTextBox { get; } = new ReactiveProperty<bool>(true);
        public ReactiveCommand DrawSrcAreaCommand { get; }
        public ReactiveCommand DrawDstAreaCommand { get; }
        public ReactiveCommand CreateTranslatePointCommand { get; } = new ReactiveCommand();
        public ReactiveCommand ClearCommand { get; } = new ReactiveCommand();

        private readonly List<Point2<double>> _srcPoints = new List<Point2<double>>();
        private readonly List<Point2<double>> _dstPoints = new List<Point2<double>>();

        private Line _cacheLine;
        private readonly Canvas _drawCanvas;

        public event PropertyChangedEventHandler PropertyChanged;

        private HomographyMatrix<double> _homo;
        private Ellipse _cacheEllipse;
        private int _pointNum;

        public VisualizerViewModel(Canvas draCanvas)
        {
            _drawCanvas = draCanvas;

            DrawSrcAreaCommand = _drawingSrc.Select(x => x == false).ToReactiveCommand();
            DrawDstAreaCommand = _drawingDst.Select(x => x == false).ToReactiveCommand();

            PointNumString = new ReactiveProperty<string>("4");
            //PointNumString.SetValidateAttribute(() => PointNumString);
            //PointNumString.
            PointNumString.Subscribe(s =>
            {
                var success = int.TryParse(s?.ToString(), out var result);

                if (success && 4 <= result)
                {
                    _pointNum = result;
                    return;
                }
                PointNumString.Value = "4";
            });

            DrawSrcAreaCommand.Subscribe(() =>
            {
                _drawingSrc.Value = true;
                _drawCanvas.MouseDown += SrcMouseDown;
                _drawCanvas.MouseMove += SrcMouseMove;
                EnableTextBox.Value = false;
            });

            DrawDstAreaCommand.Subscribe(() =>
            {
                _drawingDst.Value = true;
                _drawCanvas.MouseDown += DstMouseDown;
                _drawCanvas.MouseMove += DstMouseMove;
                EnableTextBox.Value = false;
            });

            ClearCommand.Subscribe(() =>
            {
                _drawCanvas.Children.RemoveRange(0, _drawCanvas.Children.Count);
                _drawingDst.Value = false;
                _drawingSrc.Value = false;
                _cacheEllipse = null;
                _cacheLine = null;
                _srcPoints.Clear();
                _dstPoints.Clear();
                _homo = null;
                _drawCanvas.MouseDown -= SrcMouseDown;
                _drawCanvas.MouseMove -= SrcMouseMove;
                _drawCanvas.MouseDown -= DstMouseDown;
                _drawCanvas.MouseMove -= DstMouseMove;
                EnableTextBox.Value = true;
            });

            CreateTranslatePointCommand.Subscribe(() =>
            {
                try
                {
                    _homo = HomographyHelper.FindHomography(_srcPoints, _dstPoints);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    return;
                }

                var pointX = _srcPoints.Average(v => v.X);
                var pointY = _srcPoints.Average(v => v.Y);

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

                var result = _homo.Translate( pointX, pointY);
                var (translateX, translateY) = (result.X, result.Y);
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
                    var result2 = _homo.Translate(newPoint.X, newPoint.Y);
                    Canvas.SetLeft(dstEllipse, result2.X - srcEllipse.Width / 2);
                    Canvas.SetTop(dstEllipse, result2.Y - srcEllipse.Height / 2);
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

        public void MouseDown(MouseButtonEventArgs e, List<Point2<double>> pointsList, Brush pointBrush, Brush strokeBrush, DrawingState state)
        {
            _cacheLine = null;

            var point = e.GetPosition(_drawCanvas);

            pointsList.Add(new Point2<double>(point.X, point.Y));

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

            if (pointsList.Count == _pointNum)
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
                    X1 = start.X,
                    Y1 = start.Y,
                    X2 = end.X,
                    Y2 = end.Y,
                    StrokeThickness = 3,
                    Stroke = strokeBrush
                };
                _drawCanvas.Children.Add(line);
            }
        }

        public void MouseMove(MouseEventArgs e, List<Point2<double>> pointsList, Brush strokeBrush)
        {
            if (!pointsList.Any()) return;
            if (_cacheLine == null)
            {
                var startPoint = pointsList.Last();
                _cacheLine = new Line
                {
                    X1 = startPoint.X,
                    Y1 = startPoint.Y,
                    X2 = startPoint.X,
                    Y2 = startPoint.Y,
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