using CommunityToolkit.Mvvm.DependencyInjection;
using MiniOP.Model;
using System;
using System.Collections.Generic;
using System.IO.Ports;
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

namespace MiniOP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        

        public MainWindow()
        {
            InitializeComponent();
            DataContext = Ioc.Default.GetService<ViewModels.MainViewModel>();
            comboBoxPorts.DataContext = SerialPort.GetPortNames();
            
        }

        private void connectPort_Click(object sender, RoutedEventArgs e)
        {
            string? selectedPort = comboBoxPorts.SelectedItem as string;
            (DataContext as ViewModels.MainViewModel).ConnectCOMCommand(selectedPort);
            
        }

        private void disconnectPort_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as ViewModels.MainViewModel).DisconnectCOMCommand();
        }
    }
}
