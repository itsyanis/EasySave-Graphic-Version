using EasySave_3.Stores;
using EasySave_3.ViewModels;
using EasySave_3.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;

namespace EasySave_3.Commands
{
    public class DeleteBackupJobCommand : CommandBase
    {
        private NavigationStore _navigationStore;

        private readonly ObservableCollection<BackupJobViewModel> _backupJobList;
        public IEnumerable<BackupJobViewModel> BackupJobList;

        public DeleteBackupJobCommand(NavigationStore navigationStore, ObservableCollection<BackupJobViewModel> backupJob)
        {
            _backupJobList = new ObservableCollection<BackupJobViewModel>();
            _navigationStore = navigationStore;
            BackupJobList = backupJob;
        }

        public override void Execute(object parameter)
        {
            GetSelectedBackupJob();
            foreach(BackupJobViewModel item in _backupJobList)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show(Properties.strings.DBJCBoxText + item.BackupName, Properties.strings.DBJCBoxTitle, MessageBoxButton.YesNo, MessageBoxImage.Warning);
                switch (messageBoxResult)
                {
                    case MessageBoxResult.Yes:
                        BackupJob backupJob = new BackupJob(item.BackupName, item.SourcePath, item.DestinationPath, item.Type);
                        backupJob.DeleteSpecificJob(item.BackupName);
                        break;
                    case MessageBoxResult.No:
                        break;
                }
                
            }
            _navigationStore.CurrentViewModel = new ManageBackupJobViewModel(_navigationStore);
        }

        public void GetSelectedBackupJob()
        {
            foreach (BackupJobViewModel item in BackupJobList)
            {
                if (item.SomeItemSelected)
                {
                    _backupJobList.Add(item);
                }
            }
        }
    }
}
