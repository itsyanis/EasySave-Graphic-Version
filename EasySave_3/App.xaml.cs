using EasySave_3.Stores;
using EasySave_3.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;

namespace EasySave_3
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly NavigationStore _navigationStore;
        private ObservableCollection<BackupJobViewModel> listBackup;
        public App()
        {
            listBackup = new ObservableCollection<BackupJobViewModel>();
            _navigationStore = new NavigationStore();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            FileDirectoryProcessing fileDirectoryProcessing = new FileDirectoryProcessing();
            fileDirectoryProcessing.GenerateConfigFiles();
            fileDirectoryProcessing.Language();
            _navigationStore.CurrentViewModel = CreateMainMenuViewModel();
            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(_navigationStore)
            };
            MainWindow.Show();
            base.OnStartup(e);
        }


        private MainMenuViewModel CreateMainMenuViewModel()
        {
            return new MainMenuViewModel(_navigationStore);
        }
    }
}
