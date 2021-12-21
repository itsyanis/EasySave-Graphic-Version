using EasySave_3.Stores;
using EasySave_3.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasySave_3.Commands
{
    public class NavigateManageBackupCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;

        public NavigateManageBackupCommand(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }
        public override void Execute(object parameter)
        {
            _navigationStore.CurrentViewModel = new ManageBackupJobViewModel(_navigationStore);
        }
    }
}
