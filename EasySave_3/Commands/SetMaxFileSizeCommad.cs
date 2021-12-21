using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using EasySave_3.Stores;
using EasySave_3.ViewModels;

namespace EasySave_3.Commands
{
    public class SetMaxFileSizeCommad : CommandBase
    {
        private NavigationStore _navigationStore;
        private SettingsViewModel _settingsViewModel;

        public SetMaxFileSizeCommad(SettingsViewModel settingsViewModel, NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _settingsViewModel = settingsViewModel;
            _settingsViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        public override bool CanExecute(object parameter)
        {
            return !string.IsNullOrEmpty(_settingsViewModel.MaxFileSize) && _settingsViewModel.CanCreate && base.CanExecute(parameter);
        }

        public override void Execute(object parameter)
        {
            long maxfile = Convert.ToInt64(_settingsViewModel.MaxFileSize);
            FileDirectoryProcessing.SetMaxFile(maxfile);
            _navigationStore.CurrentViewModel = new SettingsViewModel(_navigationStore);
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SettingsViewModel.MaxFileSize))
            {
                OnCanExecutedChanged();
            }
        }
    }
}
