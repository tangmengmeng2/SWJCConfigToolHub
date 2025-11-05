using MyUtility.Utility;
using SWJCTool.Service;
using SWJCTool.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SWJCTool
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel MainWindowVM = SingletonProvider<MainWindowViewModel>.Instance;
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = MainWindowVM;
        }
        private void CreatFile(object sender, RoutedEventArgs e)
        {
            CreateSWJCService service = new CreateSWJCService();
            service.CreateSWJCConfig();
        }
    }
}
