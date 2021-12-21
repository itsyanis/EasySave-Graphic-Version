using EasySave_3.Stores;
using EasySave_3.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EasySave_3.Commands
{
    public class DeleteAppJobCommand : CommandBase
    {
        private NavigationStore _navigationStore;

        public DeleteAppJobCommand(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }


        public override void Execute(object parameter)
        {
            FileDirectoryProcessing.SetSoftwares(parameter.ToString(), true);
            _navigationStore.CurrentViewModel = new SettingsViewModel(_navigationStore);
        }
    }
}
