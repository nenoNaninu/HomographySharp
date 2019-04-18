using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using MathNet.Numerics.LinearAlgebra.Double;
using Reactive.Bindings;

namespace HomographyVisualizer
{
    /// <summary>
    /// Rxらいくに書こうとして断念。Rx難しい。。。
    /// </summary>
    public class VisualizerViewModelReactive : INotifyPropertyChanged
    {
        private Canvas _drawCanvas;

        private ReactiveProperty<bool> _drawingSrc = new ReactiveProperty<bool>(false);
        private ReactiveProperty<bool> _drawingDst = new ReactiveProperty<bool>(false);

        public ReactiveProperty<string> PointNumString { get; }

        public ReactiveProperty<bool> EnableTextBox { get; } = new ReactiveProperty<bool>(true);
        public ReactiveCommand DrawSrcAreaCommand { get; }
        public ReactiveCommand DrawDstAreaCommand { get; }
        public ReactiveCommand CreateTranslatePointCommand { get; } = new ReactiveCommand();
        public ReactiveCommand ClearCommand { get; } = new ReactiveCommand();

        private readonly List<DenseVector> _srcPoints = new List<DenseVector>();
        private readonly List<DenseVector> _dstPoints = new List<DenseVector>();

        public event PropertyChangedEventHandler PropertyChanged;

        private DenseMatrix _homo;
        private Ellipse _cacheEllipse;
        private int _pointNum;

        private IObservable<MouseButtonEventArgs> _mouseDown;
        private IObservable<MouseEventArgs> _mouseMove;
        private IObservable<MouseButtonEventArgs> _mouseUp;

        List<Line> _srcLines = new List<Line>(4);
        List<Line> _dstLines = new List<Line>(4);

        public VisualizerViewModelReactive(Canvas drawCanvas)
        {
            _drawCanvas = drawCanvas;

            DrawSrcAreaCommand = _drawingSrc.Select(x => x == false).ToReactiveCommand();
            DrawDstAreaCommand = _drawingDst.Select(x => x == false).ToReactiveCommand();

            PointNumString = new ReactiveProperty<string>("4");

            _mouseDown = Observable.FromEvent<MouseButtonEventHandler, MouseButtonEventArgs>(
                h => (s, e) => h(e),
                h => _drawCanvas.MouseDown += h,
                h => _drawCanvas.MouseDown -= h);

            _mouseMove = Observable.FromEvent<MouseEventHandler, MouseEventArgs>(
                h => (s, e) => h(e),
                h => _drawCanvas.MouseMove += h,
                h => _drawCanvas.MouseMove -= h);

            _mouseUp = Observable.FromEvent<MouseButtonEventHandler, MouseButtonEventArgs>(
                h => (s, e) => h(e),
                h => _drawCanvas.MouseUp += h,
                h => _drawCanvas.MouseUp -= h);


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
                EnableTextBox.Value = false;
                CreateDrawingStream(_srcPoints, _srcLines, Brushes.Blue, Brushes.Aqua);
            });

            DrawDstAreaCommand.Subscribe(() =>
            {
                _drawingDst.Value = true;
                EnableTextBox.Value = false;
                CreateDrawingStream(_dstPoints, _dstLines, Brushes.Crimson, Brushes.Coral);
            });
        }

        private Line CreateLine(double x0, double y0, double x1, double y1, double strokeThickness, Brush strokeBrush)
        {
            return new Line
            {
                X1 = x0,
                Y1 = y0,
                X2 = x1,
                Y2 = y1,
                StrokeThickness = strokeThickness,
                Stroke = strokeBrush,
            };
        }

        public void CreateDrawingStream(List<DenseVector> pointsList, List<Line> lineList, Brush pointBrush, Brush strokeBrush)
        {

            _mouseDown.Take(_pointNum)
                .Subscribe(x =>
                {
                    var point = x.GetPosition(_drawCanvas);

                    var v = DenseVector.OfArray(new double[] { point.X, point.Y });
                    pointsList.Add(v);

                    var elipse = new Ellipse
                    {
                        Width = 7,
                        Height = 7,
                        Fill = pointBrush
                    };

                    Canvas.SetLeft(elipse, point.X - elipse.Width / 2);
                    Canvas.SetTop(elipse, point.Y - elipse.Height / 2);

                    _drawCanvas.Children.Add(elipse);

                    var line = CreateLine(point.X, point.Y, point.X + 10, point.Y + 10, 3, strokeBrush);
                    
                    lineList.Add(line);
                    _drawCanvas.Children.Add(line);
                },
                () =>
                {
                    var firstPoint = pointsList.First();
                    var lastPoint = pointsList.Last();

                    var latestLine = lineList.Last();

                    latestLine.X2 = firstPoint[0];
                    latestLine.Y2 = firstPoint[1];
                });

            //ドラッグする時の描画
            _mouseDown
                .SelectMany(_mouseMove)
                .TakeUntil(_mouseDown.Skip(1))
                .Repeat(_pointNum)
                .Subscribe(x =>
                {
                    if (!lineList.Any()) return;

                    var point = x.GetPosition(_drawCanvas);

                    var latestLine = lineList.Last();

                    latestLine.X2 = point.X;
                    latestLine.Y2 = point.Y;
                },
                () =>
                {
                    Console.WriteLine("Complete Dracking");
                });
        }
    }
}