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

namespace WPF_STUM.Pages
{
    /// <summary>
    /// Interaction logic for editPersonData.xaml
    /// </summary>
    public partial class editPersonData : Window
    {
        public editPersonData()
        {
            InitializeComponent();
        }
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove(); // Allows dragging of the window
            }
        }
        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
