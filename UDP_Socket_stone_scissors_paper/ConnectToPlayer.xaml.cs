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
using System.Windows.Shapes;

namespace UDP_Socket_stone_scissors_paper
{
    /// <summary>
    /// Логика взаимодействия для ConnectToPlayer.xaml
    /// </summary>
    public partial class ConnectToPlayer : Window
    {
        AppVM _appVM;
        public ConnectToPlayer(AppVM appVM)
        {
            InitializeComponent();

            _appVM = appVM;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _appVM.GetLocalPort = Int32.Parse(LocalPort.Text);
            _appVM.GetRemotePort = Int32.Parse(RemotePort.Text);

            this.Close();
        }
    }
}
