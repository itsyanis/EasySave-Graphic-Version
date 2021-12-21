using EasySave_3.Stores;
using EasySave_3.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace EasySave_3.Commands
{
    public class NavigateExecuteCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;
        private ObservableCollection<BackupJobViewModel> listBackup;

        public NavigateExecuteCommand(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }
        public override void Execute(object parameter)
        {
            _navigationStore.CurrentViewModel = new ExecuteViewModel(_navigationStore,listBackup);
        }
    }
}
