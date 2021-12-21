using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EasySave_3.Views
{
    /// <summary>
    /// Logique d'interaction pour ExecuteView.xaml
    /// </summary>
    public partial class ExecuteView : UserControl
    {
        public ExecuteView()
        {
            InitializeComponent();
        }

        public void FreezProgress(object sender, RoutedEventArgs e)
        {
            ProgBar.IsIndeterminate = false;
        }

        public void UnblockProgress(object sender, RoutedEventArgs e)
        {
            ProgBar.IsIndeterminate = true;
        }
    }
}
