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

            InitializeComponent();

            //var vm = new VisualizerViewModel(drawCanvas);
            var vm = new VisualizerViewModelReactive(drawCanvas);
            DataContext = vm;
        }
    }
}
