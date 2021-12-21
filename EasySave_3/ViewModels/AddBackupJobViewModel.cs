using EasySave_3.Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Input;
using EasySave_3.Properties;

namespace EasySave_3.ViewModels
{
    public class AddBackupJobViewModel : ViewModelBase
    {
        //Get the backup name given by the user in the view
        private string _addBackupName;
        public string AddBackupName
        {
            get { return _addBackupName; }
            set
            {
                _addBackupName = value;
                _errorsViewModel.ClearErrors(nameof(AddBackupName));    //Remove error
                if (GetBackupJob(_addBackupName))   //If already in the backup job list
                {
                    _errorsViewModel.AddError(nameof(AddBackupName), strings.ABJVMNameError);   //Add error
                }
                OnPropertyChanged(nameof(AddBackupName));   //Update property state
            }
        }

        //Get the path given by the user in the view
        private string _addSourcePath;
        public string AddSourcePath
        {
            get { return _addSourcePath; }
            set
            {
                _addSourcePath = value;
                _errorsViewModel.ClearErrors(nameof(AddSourcePath));    //remove error
                if (!Directory.Exists(_addSourcePath))   //If directory doesnt exist
                {
                    _errorsViewModel.AddError(nameof(AddSourcePath), strings.ABJVMPathError); // add error
                }
                OnPropertyChanged(nameof(AddSourcePath));   //update property state
            }
        }

        //Get the path given by the user in the view
        private string _addDestinationPath;
        public string AddDestinationPath
        {
            get { return _addDestinationPath; }
            set
            {
                _addDestinationPath = value;
                _errorsViewModel.ClearErrors(nameof(AddDestinationPath)); //remove error
                if (!Directory.Exists(_addDestinationPath)) //If empty or directory doesn't exist
                {
                    _errorsViewModel.AddError(nameof(AddDestinationPath), strings.ABJVMPathError);    //add error
                }
                OnPropertyChanged(nameof(AddDestinationPath));  //update proterty state
            }
        }

        //Get the save type given by the user in the view
        private string _addSaveType;
        public string AddSaveType
        {
            get { return _addSaveType; }
            set
            {
                _addSaveType = value;
                _errorsViewModel.ClearErrors(nameof(AddSaveType));  //Remove error
                if (string.IsNullOrEmpty(nameof(_addSaveType)))    //if empty
                {
                    _errorsViewModel.AddError(nameof(AddSaveType), strings.ABJVMSaveTypeError);    //Add error
                }
                OnPropertyChanged(nameof(AddSaveType)); //update property state
            }
        }

        private readonly ObservableCollection<BackupJobViewModel> _backupJob;

        //Commands used
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand ReturnCommand { get; }



        private readonly ErrorsViewModel _errorsViewModel;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        public bool CanCreate => !_errorsViewModel.HasErrors;

        public AddBackupJobViewModel(Stores.NavigationStore navigationStore)
        {
            _errorsViewModel = new ErrorsViewModel();

            SaveCommand = new AddBackupJobCommand(this, navigationStore);
            CancelCommand = new NavigateManageBackupCommand(navigationStore);
            ReturnCommand = new NavigateHomeCommand(navigationStore);               //Command to navigate to the main menu

        }

        //Get the list of all backup job
        private bool GetBackupJob(string Name)
        {
            List<BackupJob> Backups = BackupJob.ShowAllJobs();

            foreach (BackupJob Backup in Backups)
            {
                if (Backup.BackupName == Name) return true; //return true if the name is already in the file
            }
            return false;
        }

        //Get the errors
        public IEnumerable GetErrors(string propertyName)
        {
            return _errorsViewModel.GetErrors(propertyName);
        }

        //Update the state of the error
        private void ErrorsViewModel_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
            OnPropertyChanged(nameof(CanCreate));
        }
    }
}
