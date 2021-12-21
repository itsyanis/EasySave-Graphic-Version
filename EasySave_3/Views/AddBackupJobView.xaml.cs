using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Path = System.IO.Path;
using Ookii.Dialogs.Wpf;

namespace EasySave_3.Views
{
    /// <summary>
    /// Logique d'interaction pour AddBackupJobView.xaml
    /// </summary>
    public partial class AddBackupJobView : UserControl
    {
        public AddBackupJobView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog dlg = new VistaFolderBrowserDialog();
            if (dlg.ShowDialog() == true)
            {
                Spath.Text= dlg.SelectedPath;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            VistaFolderBrowserDialog dlg = new VistaFolderBrowserDialog();
            if (dlg.ShowDialog() == true)
            {
                Dpath.Text = dlg.SelectedPath;
            }
        }
    }
}