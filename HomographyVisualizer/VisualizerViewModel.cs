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
using Reactive.Bindings;

namespace HomographyVisualizer
{
    public class VisualizerViewModel : INotifyPropertyChanged
    {
        public ReactiveCommand DrawSrcAreaCommand { get; set; } = new ReactiveCommand();
        public ReactiveCommand DrawDstAreaCommand { get; set; } = new ReactiveCommand();

        private List<DenseVector> _srcPoints { get; set; } = new List<DenseVector>();
        private List<DenseVector> _dstPoints { get; set; } = new List<DenseVector>();

        private Line _cacheLine;
        private Canvas _drawCanvas;

        private Brush _pointBrush;
        private Brush _strokeBrush;

        public event PropertyChangedEventHandler PropertyChanged;

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

        public void MouseDown(MouseButtonEventArgs e,List<DenseVector> pointsList,Brush pointBrush,Brush strokeBrush, DrawingState state)
        {
            _cacheLine = null;

            var point = e.GetPosition(_drawCanvas);

            var v = DenseVector.OfArray(new double[] { point.X, point.Y });
            pointsList.Add(v);

            var elipse = new Ellipse();
            elipse.Width = 10;
            elipse.Height = 10;
            elipse.Fill = pointBrush;

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