using EasySave_3.Stores;
using EasySave_3.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasySave_3.Commands
{
    public class NavigateAddBackupCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;

        public NavigateAddBackupCommand(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }
        public override void Execute(object parameter)
        {
            _navigationStore.CurrentViewModel = new AddBackupJobViewModel(_navigationStore);
        }
    }
}
