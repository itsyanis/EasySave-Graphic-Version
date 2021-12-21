using EasySave_3.Stores;
using EasySave_3.ViewModels;
using System;
using System.ComponentModel;
using System.Text;

namespace EasySave_3.Commands
{
    public class AddExtensionCommand : CommandBase
    {
        private NavigationStore _navigationStore;
        private SettingsViewModel _settingsViewModel;


        public AddExtensionCommand(SettingsViewModel settingsViewModel, NavigationStore navigationStore)
        {
            _settingsViewModel = settingsViewModel;
            _navigationStore = navigationStore;
            _settingsViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        public override bool CanExecute(object parameter)
        {
            return !string.IsNullOrEmpty(_settingsViewModel.ExtensionName) && _settingsViewModel.CanCreate && base.CanExecute(parameter);
        }

        public override void Execute(object parameter)
        {
            FileDirectoryProcessing.SetExtensions(_settingsViewModel.ExtensionName,false);
            _navigationStore.CurrentViewModel = new SettingsViewModel(_navigationStore);
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SettingsViewModel.ExtensionName))
            {
                OnCanExecutedChanged();
            }
        }
    }
}
