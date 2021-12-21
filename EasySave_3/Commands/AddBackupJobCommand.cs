using EasySave_3.ViewModels;
using System;
using System.ComponentModel;
using System.Text;
using EasySave_3.Stores;

namespace EasySave_3.Commands
{
    public class AddBackupJobCommand : CommandBase
    {
        private AddBackupJobViewModel _addBackupJobViewModel;
        private NavigationStore _navigationStore;

        public AddBackupJobCommand(AddBackupJobViewModel addBackupJobViewModel, NavigationStore navigationStore)
        {
            _addBackupJobViewModel = addBackupJobViewModel;
            _navigationStore = navigationStore;
            _addBackupJobViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }



        public override bool CanExecute(object parameter)
        {
            return _addBackupJobViewModel.CanCreate
                && !string.IsNullOrEmpty(_addBackupJobViewModel.AddBackupName)
                && !string.IsNullOrEmpty(_addBackupJobViewModel.AddSourcePath)
                && !string.IsNullOrEmpty(_addBackupJobViewModel.AddDestinationPath)
                && !string.IsNullOrEmpty(_addBackupJobViewModel.AddSaveType)
                && _addBackupJobViewModel.AddDestinationPath != _addBackupJobViewModel.AddSourcePath
                && base.CanExecute(parameter);
        }

        public override void Execute(object parameter)
        {
            BackupJob backupJob = new BackupJob(_addBackupJobViewModel.AddBackupName, 
                _addBackupJobViewModel.AddSourcePath,
                _addBackupJobViewModel.AddDestinationPath,
                _addBackupJobViewModel.AddSaveType);

            backupJob.WriteOnFile(@"C:\EasySave\Backup.json",backupJob);

            _navigationStore.CurrentViewModel = new ManageBackupJobViewModel(_navigationStore);
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(AddBackupJobViewModel.AddBackupName) 
                || e.PropertyName == nameof(AddBackupJobViewModel.AddSourcePath)
                || e.PropertyName == nameof(AddBackupJobViewModel.AddDestinationPath)
                || e.PropertyName == nameof(AddBackupJobViewModel.AddSaveType))
            {
                OnCanExecutedChanged();
            }
        }
    }
}
