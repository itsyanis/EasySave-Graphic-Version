using EasySave_3.Stores;
using EasySave_3.ViewModels;
using EasySave_3.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace EasySave_3.Commands
{
    public class ExecuteCommand : CommandBase
    {
        private NavigationStore _navigationStore;

        private readonly ObservableCollection<BackupJobViewModel> _backupJobList;
        public IEnumerable<BackupJobViewModel> BackupJobList;

        public ExecuteCommand(NavigationStore navigationStore, ObservableCollection<BackupJobViewModel> backupJob)
        {
            _backupJobList = new ObservableCollection<BackupJobViewModel>();
            _navigationStore = navigationStore;
            BackupJobList = backupJob;
        }

        /*public override bool CanExecute(object parameter)
        {
            return _backupJobList.Count != 0 && base.CanExecute(parameter);
        }*/
        public override void Execute(object parameter)
        {
             GetSelectedBackupJob();
            _navigationStore.CurrentViewModel = new ExecuteViewModel(_navigationStore,_backupJobList);
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
