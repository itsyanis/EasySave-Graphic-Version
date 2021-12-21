using System;
using System.Collections.Generic;
using System.Text;
using EasySave_3.Stores;
using EasySave_3.ViewModels;

namespace EasySave_3.Commands
{
    public class DeleteFilePriorityCommand : CommandBase
    {
        private NavigationStore _navigationStore;

        public DeleteFilePriorityCommand(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }

        public override void Execute(object parameter)
        {
            FileDirectoryProcessing.SetFilePriority(parameter.ToString(), true);
            _navigationStore.CurrentViewModel = new SettingsViewModel(_navigationStore);
        }
    }
}
