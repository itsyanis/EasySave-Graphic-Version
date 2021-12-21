using EasySave_3.Commands;
using EasySave_3.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace EasySave_3.ViewModels
{
    public class ManageBackupJobViewModel : ViewModelBase
    {
        private readonly ObservableCollection<BackupJobViewModel> _backupJob;   //Collection that will contains the list of backupjob
        public IEnumerable<BackupJobViewModel> BackupJob => _backupJob;     //Interface that will be bind to the view

        //All the commands
        public ICommand AddBackupCommand { get; }
        public ICommand ExecuteCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ReturnCommand { get; }

        
        //Constructor
        public ManageBackupJobViewModel(NavigationStore navigationStore)
        {
            _backupJob = new ObservableCollection<BackupJobViewModel>();    //Initialize the collection
            
            ShowJobs();     //Get all the jobs

            AddBackupCommand = new NavigateAddBackupCommand(navigationStore);       //Command to navigate to the add backup view
            ReturnCommand = new NavigateHomeCommand(navigationStore);               //Command to navigate to the main menu
            ExecuteCommand = new ExecuteCommand(navigationStore, _backupJob);       //Command to execute selected backup jobs
            DeleteCommand = new DeleteBackupJobCommand(navigationStore, _backupJob);//Command to delete selected backup jobs
        }

        //Reads the backup.json to get all the backup job 
        private void ShowJobs()
        {
            List<BackupJob> Backups = EasySave_3.BackupJob.ShowAllJobs();   //List contains all the lines

            //Adds each line to the collection
            foreach (BackupJob Backup in Backups)
            {
                _backupJob.Add(new BackupJobViewModel(Backup));
            }
        }
    }
}
