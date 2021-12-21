using EasySave_3.Stores;
using EasySave_3.ViewModels;
using System;
using System.ComponentModel;
using System.Text;

namespace EasySave_3.Commands
{
    public class AddAppJobCommand : CommandBase
    {
        private NavigationStore _navigationStore;
        private SettingsViewModel _settingsViewModel;


        public AddAppJobCommand(SettingsViewModel settingsViewModel,NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _settingsViewModel = settingsViewModel;
            _settingsViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        public override bool CanExecute(object parameter)
        {
            return !string.IsNullOrEmpty(_settingsViewModel.AppJobName) && _settingsViewModel.CanCreate && base.CanExecute(parameter);
        }

        public override void Execute(object parameter)
        {
            FileDirectoryProcessing.SetSoftwares(_settingsViewModel.AppJobName, false);
            _navigationStore.CurrentViewModel = new SettingsViewModel(_navigationStore);
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SettingsViewModel.AppJobName))
            {
                OnCanExecutedChanged();
            }
        }
    }
}