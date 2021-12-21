using EasySave_3.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Logique d'interaction pour ManageBackupJobView.xaml
    /// </summary>
    public partial class ManageBackupJobView : UserControl
    {
        public ObservableCollection<BackupJobViewModel> listBackup;
        public ManageBackupJobView()
        {
            InitializeComponent();
        }

        public void ListBackupJob_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            listBackup = new ObservableCollection<BackupJobViewModel>();
            string banane = "";
            BackupJobViewModel selectedBackup = (BackupJobViewModel)ListBackupJob.SelectedItem;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
