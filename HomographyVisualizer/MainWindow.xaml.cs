using System.Windows;

namespace HomographyVisualizer
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            //var vm = new VisualizerViewModelReactive(DrawCanvas);
            InitializeComponent();

            var vm = new VisualizerViewModel(drawCanvas);
            DataContext = vm;

        }
    }
}
