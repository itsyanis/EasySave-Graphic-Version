using EasySave_3.Commands;
using EasySave_3.Stores;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace EasySave_3.ViewModels
{
    public class MainMenuViewModel : ViewModelBase
    {
        //Commands used
        public ICommand AddBackupJobScreen { get; }
        public ICommand ManageBackupJobScreen { get; }
        public ICommand SettingsScreen { get; }
        public MainMenuViewModel(NavigationStore navigationStore)
        {
            AddBackupJobScreen = new NavigateAddBackupCommand(navigationStore);         //Command to navigate to the add backup view
            ManageBackupJobScreen = new NavigateManageBackupCommand(navigationStore);   //Command to navigate to the manage backup view
            SettingsScreen = new NavigateSettingsCommand(navigationStore);              //Command to navigate to the settings view
        }
    }
}
