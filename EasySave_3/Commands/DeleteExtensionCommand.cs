using EasySave_3.Stores;
using EasySave_3.ViewModels;
using EasySave_3.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;

namespace EasySave_3.Commands
{
    public class DeleteExtensionCommand : CommandBase
    {
        private NavigationStore _navigationStore;

        public DeleteExtensionCommand(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }
        public override void Execute(object parameter)
        {
            FileDirectoryProcessing.SetExtensions(parameter.ToString(), true);
            _navigationStore.CurrentViewModel = new SettingsViewModel(_navigationStore);
        }
    }
}
