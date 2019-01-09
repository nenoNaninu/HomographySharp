using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Controls;
using System.Windows.Input;
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

        public ReactiveCommand DrawSrcAreaCommand { get; set; } = new ReactiveCommand();
        public ReactiveCommand DrawDstAreaCommand { get; set; } = new ReactiveCommand();

        private List<DenseVector> _srcPoints { get; set; } = new List<DenseVector>();
        private List<DenseVector> _dstPoints { get; set; } = new List<DenseVector>();

        private ReactiveProperty<bool> _isDrawingSrcArea = new ReactiveProperty<bool>(false);
        private ReactiveProperty<bool> _isDrawingDstArea = new ReactiveProperty<bool>(false);

        private Line _cacheLine;

        public event PropertyChangedEventHandler PropertyChanged;

        public VisualizerViewModelReactive(Canvas draCanvas)
        {
            _drawCanvas = draCanvas;

            DrawSrcAreaCommand = _isDrawingSrcArea.Select(x => x == false).ToReactiveCommand();
            DrawDstAreaCommand = _isDrawingDstArea.Select(x => x == false).ToReactiveCommand();

            DrawSrcAreaCommand.Subscribe(() => { _isDrawingSrcArea.Value = true; });
            DrawDstAreaCommand.Subscribe(() => { _isDrawingDstArea.Value = true; });

            var mouseDown = Observable.FromEvent<MouseButtonEventHandler, MouseButtonEventArgs>(
                h => (s, e) => h(e),
                h => _drawCanvas.MouseDown += h,
                h => _drawCanvas.MouseDown -= h);

            var mouseMove = Observable.FromEvent<MouseEventHandler, MouseEventArgs>(
                h => (s, e) => h(e),
                h => _drawCanvas.MouseMove += h,
                h => _drawCanvas.MouseMove -= h);

            var mouseUp = Observable.FromEvent<MouseButtonEventHandler, MouseButtonEventArgs>(
                h => (s, e) => h(e),
                h => _drawCanvas.MouseUp += h,
                h => _drawCanvas.MouseUp -= h);

            var srcClick = mouseDown.SkipWhile(_ => _isDrawingSrcArea.Value)
                .Repeat(4).Finally(() => _isDrawingSrcArea.Value = false);

            var srcDrag = mouseMove.SkipUntil(srcClick)
                .TakeUntil(mouseUp).Repeat(3)
                .Finally(() =>
                {
                    var start = _srcPoints.First();
                    var end = _srcPoints.Last();
                    var line = new Line
                    {
                        X1 = start[0],
                        Y1 = start[1],
                        X2 = end[0],
                        Y2 = end[1],
                        Width = 2
                    };
                    _drawCanvas.Children.Add(line);
                });

            srcClick.Select(x => x.GetPosition(_drawCanvas))
                .Subscribe(p =>
                {
                    var elipse = new Ellipse
                    {
                        Width = 3,
                        Height = 3
                    };
                    Canvas.SetTop(elipse, p.Y);
                    Canvas.SetLeft(elipse, p.X);
                    var v = DenseVector.OfArray(new double[] { p.X, p.Y });
                    _srcPoints.Add(v);
                    _drawCanvas.Children.Add(elipse);
                    _cacheLine = null;
                });

            //srcDrag.Select(x => x.GetPosition(_drawCanvas))
            //    .Subscribe(p =>
            //    {
            //        if (_cacheLine == null)
            //        {
            //            if (_srcPoints.Any())
            //            {
            //                var startPoint = _srcPoints.Last();
            //                _cacheLine = new Line
            //                {
            //                    Width = 2,
            //                    X1 = startPoint[0],
            //                    Y1 = startPoint[1],
            //                    X2 = startPoint[0],
            //                    Y2 = startPoint[1]
            //                };
            //                _drawCanvas.Children.Add(_cacheLine);
            //            }
            //        }

            //        _cacheLine.X2 = p.X;
            //        _cacheLine.Y2 = p.Y;
            //    });
        }
    }
}