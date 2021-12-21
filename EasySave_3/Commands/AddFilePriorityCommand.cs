using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using EasySave_3.Stores;
using EasySave_3.ViewModels;

namespace EasySave_3.Commands
{
    public class AddFilePriorityCommand : CommandBase
    {
        private NavigationStore _navigationStore;
        private SettingsViewModel _settingsViewModel;

        public AddFilePriorityCommand(SettingsViewModel settingsViewModel, NavigationStore navigationStore)
        {
            _settingsViewModel = settingsViewModel;
            _navigationStore = navigationStore;
            _settingsViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        public override bool CanExecute(object parameter)
        {
            return !string.IsNullOrEmpty(_settingsViewModel.FilePriorityName) && _settingsViewModel.CanCreate && base.CanExecute(parameter);
        }

        public override void Execute(object parameter)
        {
            FileDirectoryProcessing.SetFilePriority(_settingsViewModel.FilePriorityName, false);
            _navigationStore.CurrentViewModel = new SettingsViewModel(_navigationStore);
        }
        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SettingsViewModel.FilePriorityName))
            {
                OnCanExecutedChanged();
            }
        }
    }
}
